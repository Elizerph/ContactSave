using Microsoft.EntityFrameworkCore;

namespace ContactSave
{
    public partial class SavingForm : Form, IProgress<string>
    {
        public Task Operation { get; set; }
        public CancellationTokenSource TokenSource { get; set; }

        public SavingForm()
        {
            InitializeComponent();
        }

        public void Report(string value)
        {
            tbLog.AppendText(value);
            tbLog.AppendText(Environment.NewLine);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (TokenSource != null)
                TokenSource.Cancel();
            btnCancel.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbLog.Text);
        }

        private async void SavingForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (Operation != null)
                    await Operation;
            }
            catch (OperationCanceledException)
            {
                Report("Operation cancelled.");
            }
            catch (Exception ex)
            {
                var exception = ex;
                while (exception != null)
                {
                    Report($"Error during operation: {exception.Message}");
                    exception = exception.InnerException;
                }
            }
            finally
            {
                btnCancel.Enabled = false;
                btnClose.Enabled = true;
                btnCopyToClipboard.Enabled = true;
            }
        }
    }
}
