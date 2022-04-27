using ContactSave.DataAccess;
using ContactSave.Model;

namespace ContactSave
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (ofdMain.ShowDialog() == DialogResult.OK)
                tbFile.Text = ofdMain.FileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var file = tbFile.Text;
            if (string.IsNullOrWhiteSpace(file))
            {
                MessageBox.Show("File is not specified.");
                return;
            }
            if (!File.Exists(file))
            {
                MessageBox.Show($"File {file} does not exists.");
                return;
            }

            using var savingForm = new SavingForm();
            var tokenSource = new CancellationTokenSource();
            var operation = Run(file, savingForm, tokenSource.Token);
            savingForm.Operation = operation;
            savingForm.TokenSource = tokenSource;
            savingForm.ShowDialog(this);
        }

        private async Task Run(string file, IProgress<string> progress, CancellationToken cancellationToken = default)
        {
            using var db = new ContactContext();
            var upload = new Upload
            {
                File = file,
                Comment = tbComment.Text,
                Timestamp = DateTime.UtcNow
            };
            await db.Uploads.AddAsync(upload, cancellationToken);
            const int logBatchSize = 2000;
            var currentRowIndex = 0;
            var logBatchIndex = 0;

            progress.Report("Reading contacts file…");
            using var consumer = await BuildConsumer(db);
            var reader = new ContactStubReader(TextTableReader.GetSemicolon());
            await foreach (var stub in reader.Read(FileUtils.ReadLinesAsync(file)))
            {
                if (stub.Validate(out var contact))
                    await consumer.Consume(Tuple.Create(stub, contact));
                currentRowIndex++;
                if (currentRowIndex == logBatchSize)
                {
                    progress.Report($"{logBatchIndex * logBatchSize + currentRowIndex} rows handled…");
                    cancellationToken.ThrowIfCancellationRequested();
                    currentRowIndex = 0;
                    logBatchIndex++;
                }
            }
            if (currentRowIndex != 0)
                progress.Report($"{logBatchIndex * logBatchSize + currentRowIndex} rows handled…");

            progress.Report("Saving contacts to the database…");
            await db.SaveChangesAsync(cancellationToken);
            progress.Report("Saved.");
        }

        private async Task<IDataConsumer<Tuple<ContactStub, Contact>>> BuildConsumer(ContactContext dbContext)
        { 
            var result = new CompositeDataConsumer<Tuple<ContactStub, Contact>>();
            result.AddConsumer(new DataGridDataConsumer(dgvPreview));
            result.AddConsumer(new DbContextConsumer(dbContext));
            await result.Init();
            return result;
        }
    }
}