using System;
using System.Windows.Forms;

namespace CKAN
{
    public partial class SetCachePathDialog : Form
    {
        public SetCachePathDialog()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            BrowseButton.Enabled = AcceptChangesButton.Enabled = CancelChangesButton.Enabled = true;
        }

        private FolderBrowserDialog browseDialog = new FolderBrowserDialog();
        public KSP CurrentInstance { get; set; }

        private void SetCachePathDialog_Load(object sender, EventArgs e)
        {
            browseDialog.ShowNewFolderButton = true;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (browseDialog.ShowDialog() == DialogResult.OK)
            {
                var path = browseDialog.SelectedPath;

                var registry = RegistryManager.Instance(CurrentInstance).registry;
                //registry.DownloadCacheDir = KSPPathUtils.NormalizePath(path);
            }
        }
    }
}
