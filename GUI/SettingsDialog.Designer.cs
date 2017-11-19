namespace CKAN
{
    partial class SettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            this.NewRepoButton = new System.Windows.Forms.Button();
            this.RepositoryGroupBox = new System.Windows.Forms.GroupBox();
            this.DownRepoButton = new System.Windows.Forms.Button();
            this.UpRepoButton = new System.Windows.Forms.Button();
            this.DeleteRepoButton = new System.Windows.Forms.Button();
            this.ReposListBox = new System.Windows.Forms.ListBox();
            this.CacheGroupBox = new System.Windows.Forms.GroupBox();
            this.SetCKANCacheButton = new System.Windows.Forms.Button();
            this.CKANCachePathLabel = new System.Windows.Forms.Label();
            this.ClearCKANCacheButton = new System.Windows.Forms.Button();
            this.CKANCacheLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.HideEpochsCheckbox = new System.Windows.Forms.CheckBox();
            this.RefreshOnStartupCheckbox = new System.Windows.Forms.CheckBox();
            this.CheckUpdateOnLaunchCheckbox = new System.Windows.Forms.CheckBox();
            this.InstallUpdateButton = new System.Windows.Forms.Button();
            this.LatestVersionLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LocalVersionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CheckForUpdatesButton = new System.Windows.Forms.Button();
            this.RepositoryGroupBox.SuspendLayout();
            this.CacheGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // NewRepoButton
            // 
            this.NewRepoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewRepoButton.Location = new System.Drawing.Point(12, 224);
            this.NewRepoButton.Name = "NewRepoButton";
            this.NewRepoButton.Size = new System.Drawing.Size(56, 26);
            this.NewRepoButton.TabIndex = 2;
            this.NewRepoButton.Text = "New";
            this.NewRepoButton.UseVisualStyleBackColor = true;
            this.NewRepoButton.Click += new System.EventHandler(this.NewRepoButton_Click);
            // 
            // RepositoryGroupBox
            // 
            this.RepositoryGroupBox.Controls.Add(this.DownRepoButton);
            this.RepositoryGroupBox.Controls.Add(this.UpRepoButton);
            this.RepositoryGroupBox.Controls.Add(this.DeleteRepoButton);
            this.RepositoryGroupBox.Controls.Add(this.ReposListBox);
            this.RepositoryGroupBox.Controls.Add(this.NewRepoButton);
            this.RepositoryGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RepositoryGroupBox.Location = new System.Drawing.Point(12, 6);
            this.RepositoryGroupBox.Name = "RepositoryGroupBox";
            this.RepositoryGroupBox.Size = new System.Drawing.Size(476, 262);
            this.RepositoryGroupBox.TabIndex = 0;
            this.RepositoryGroupBox.TabStop = false;
            this.RepositoryGroupBox.Text = "Metadata repositories";
            // 
            // DownRepoButton
            // 
            this.DownRepoButton.Enabled = false;
            this.DownRepoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DownRepoButton.Location = new System.Drawing.Point(136, 224);
            this.DownRepoButton.Name = "DownRepoButton";
            this.DownRepoButton.Size = new System.Drawing.Size(56, 26);
            this.DownRepoButton.TabIndex = 4;
            this.DownRepoButton.Text = "Down";
            this.DownRepoButton.UseVisualStyleBackColor = true;
            this.DownRepoButton.Click += new System.EventHandler(this.DownRepoButton_Click);
            // 
            // UpRepoButton
            // 
            this.UpRepoButton.Enabled = false;
            this.UpRepoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpRepoButton.Location = new System.Drawing.Point(74, 224);
            this.UpRepoButton.Name = "UpRepoButton";
            this.UpRepoButton.Size = new System.Drawing.Size(56, 26);
            this.UpRepoButton.TabIndex = 3;
            this.UpRepoButton.Text = "Up";
            this.UpRepoButton.UseVisualStyleBackColor = true;
            this.UpRepoButton.Click += new System.EventHandler(this.UpRepoButton_Click);
            // 
            // DeleteRepoButton
            // 
            this.DeleteRepoButton.Enabled = false;
            this.DeleteRepoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteRepoButton.Location = new System.Drawing.Point(408, 224);
            this.DeleteRepoButton.Name = "DeleteRepoButton";
            this.DeleteRepoButton.Size = new System.Drawing.Size(56, 26);
            this.DeleteRepoButton.TabIndex = 5;
            this.DeleteRepoButton.Text = "Delete";
            this.DeleteRepoButton.UseVisualStyleBackColor = true;
            this.DeleteRepoButton.Click += new System.EventHandler(this.DeleteRepoButton_Click);
            // 
            // ReposListBox
            // 
            this.ReposListBox.FormattingEnabled = true;
            this.ReposListBox.Location = new System.Drawing.Point(12, 18);
            this.ReposListBox.Name = "ReposListBox";
            this.ReposListBox.Size = new System.Drawing.Size(452, 200);
            this.ReposListBox.TabIndex = 1;
            this.ReposListBox.SelectedIndexChanged += new System.EventHandler(this.ReposListBox_SelectedIndexChanged);
            // 
            // CacheGroupBox
            // 
            this.CacheGroupBox.Controls.Add(this.SetCKANCacheButton);
            this.CacheGroupBox.Controls.Add(this.CKANCachePathLabel);
            this.CacheGroupBox.Controls.Add(this.ClearCKANCacheButton);
            this.CacheGroupBox.Controls.Add(this.CKANCacheLabel);
            this.CacheGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CacheGroupBox.Location = new System.Drawing.Point(12, 274);
            this.CacheGroupBox.Name = "CacheGroupBox";
            this.CacheGroupBox.Size = new System.Drawing.Size(476, 88);
            this.CacheGroupBox.TabIndex = 6;
            this.CacheGroupBox.TabStop = false;
            this.CacheGroupBox.Text = "CKAN Cache";
            // 
            // SetCKANCacheButton
            // 
            this.SetCKANCacheButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SetCKANCacheButton.Location = new System.Drawing.Point(352, 18);
            this.SetCKANCacheButton.Name = "SetCKANCacheButton";
            this.SetCKANCacheButton.Size = new System.Drawing.Size(112, 26);
            this.SetCKANCacheButton.TabIndex = 8;
            this.SetCKANCacheButton.Text = "Set cache";
            this.SetCKANCacheButton.UseVisualStyleBackColor = true;
            this.SetCKANCacheButton.Click += new System.EventHandler(this.SetCKANCacheButton_Click);
            // 
            // CKANCachePathLabel
            // 
            this.CKANCachePathLabel.AutoSize = true;
            this.CKANCachePathLabel.Location = new System.Drawing.Point(12, 25);
            this.CKANCachePathLabel.Name = "CKANCachePathLabel";
            this.CKANCachePathLabel.MaximumSize = new System.Drawing.Size(328, 14);
            this.CKANCachePathLabel.TabIndex = 7;
            this.CKANCachePathLabel.Text = "Cache path: P";
            // 
            // ClearCKANCacheButton
            // 
            this.ClearCKANCacheButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearCKANCacheButton.Location = new System.Drawing.Point(352, 50);
            this.ClearCKANCacheButton.Name = "ClearCKANCacheButton";
            this.ClearCKANCacheButton.Size = new System.Drawing.Size(112, 26);
            this.ClearCKANCacheButton.TabIndex = 10;
            this.ClearCKANCacheButton.Text = "Clear cache";
            this.ClearCKANCacheButton.UseVisualStyleBackColor = true;
            this.ClearCKANCacheButton.Click += new System.EventHandler(this.ClearCKANCacheButton_Click);
            // 
            // CKANCacheLabel
            // 
            this.CKANCacheLabel.AutoSize = true;
            this.CKANCacheLabel.Location = new System.Drawing.Point(12, 57);
            this.CKANCacheLabel.Name = "CKANCacheLabel";
            this.CKANCacheLabel.Size = new System.Drawing.Size(328, 14);
            this.CKANCacheLabel.TabIndex = 9;
            this.CKANCacheLabel.Text = "There are currently N files in the cache, taking up M MB";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.HideEpochsCheckbox);
            this.groupBox2.Controls.Add(this.RefreshOnStartupCheckbox);
            this.groupBox2.Controls.Add(this.CheckUpdateOnLaunchCheckbox);
            this.groupBox2.Controls.Add(this.InstallUpdateButton);
            this.groupBox2.Controls.Add(this.LatestVersionLabel);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.LocalVersionLabel);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.CheckForUpdatesButton);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(12, 368);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(476, 108);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CKAN Auto-update";
            // 
            // HideEpochsCheckbox
            // 
            this.HideEpochsCheckbox.AutoSize = true;
            this.HideEpochsCheckbox.Location = new System.Drawing.Point(275, 66);
            this.HideEpochsCheckbox.Name = "HideEpochsCheckbox";
            this.HideEpochsCheckbox.Size = new System.Drawing.Size(195, 18);
            this.HideEpochsCheckbox.TabIndex = 20;
            this.HideEpochsCheckbox.Text = "Hide epoch numbers in mod list\r\n(Requires Restart)";
            this.HideEpochsCheckbox.UseVisualStyleBackColor = true;
            this.HideEpochsCheckbox.CheckedChanged += new System.EventHandler(this.HideEpochsCheckbox_CheckedChanged);
            // 
            // RefreshOnStartupCheckbox
            // 
            this.RefreshOnStartupCheckbox.AutoSize = true;
            this.RefreshOnStartupCheckbox.Location = new System.Drawing.Point(275, 42);
            this.RefreshOnStartupCheckbox.Name = "RefreshOnStartupCheckbox";
            this.RefreshOnStartupCheckbox.Size = new System.Drawing.Size(167, 18);
            this.RefreshOnStartupCheckbox.TabIndex = 19;
            this.RefreshOnStartupCheckbox.Text = "Update repositories on launch";
            this.RefreshOnStartupCheckbox.UseVisualStyleBackColor = true;
            this.RefreshOnStartupCheckbox.CheckedChanged += new System.EventHandler(this.RefreshOnStartupCheckbox_CheckedChanged);
            // 
            // CheckUpdateOnLaunchCheckbox
            // 
            this.CheckUpdateOnLaunchCheckbox.AutoSize = true;
            this.CheckUpdateOnLaunchCheckbox.Location = new System.Drawing.Point(275, 18);
            this.CheckUpdateOnLaunchCheckbox.Name = "CheckUpdateOnLaunchCheckbox";
            this.CheckUpdateOnLaunchCheckbox.Size = new System.Drawing.Size(195, 18);
            this.CheckUpdateOnLaunchCheckbox.TabIndex = 18;
            this.CheckUpdateOnLaunchCheckbox.Text = "Check for CKAN updates on launch";
            this.CheckUpdateOnLaunchCheckbox.UseVisualStyleBackColor = true;
            this.CheckUpdateOnLaunchCheckbox.CheckedChanged += new System.EventHandler(this.CheckUpdateOnLaunchCheckbox_CheckedChanged);
            // 
            // InstallUpdateButton
            // 
            this.InstallUpdateButton.Enabled = false;
            this.InstallUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InstallUpdateButton.Location = new System.Drawing.Point(130, 70);
            this.InstallUpdateButton.Name = "InstallUpdateButton";
            this.InstallUpdateButton.Size = new System.Drawing.Size(112, 26);
            this.InstallUpdateButton.TabIndex = 17;
            this.InstallUpdateButton.Text = "Install update";
            this.InstallUpdateButton.UseVisualStyleBackColor = true;
            this.InstallUpdateButton.Click += new System.EventHandler(this.InstallUpdateButton_Click);
            // 
            // LatestVersionLabel
            // 
            this.LatestVersionLabel.AutoSize = true;
            this.LatestVersionLabel.Location = new System.Drawing.Point(84, 38);
            this.LatestVersionLabel.Name = "LatestVersionLabel";
            this.LatestVersionLabel.Size = new System.Drawing.Size(50, 14);
            this.LatestVersionLabel.TabIndex = 15;
            this.LatestVersionLabel.Text = "???";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 14);
            this.label4.TabIndex = 14;
            this.label4.Text = "Latest version:";
            // 
            // LocalVersionLabel
            // 
            this.LocalVersionLabel.AutoSize = true;
            this.LocalVersionLabel.Location = new System.Drawing.Point(84, 18);
            this.LocalVersionLabel.Name = "LocalVersionLabel";
            this.LocalVersionLabel.Size = new System.Drawing.Size(50, 14);
            this.LocalVersionLabel.TabIndex = 13;
            this.LocalVersionLabel.Text = "v0.0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 14);
            this.label3.TabIndex = 12;
            this.label3.Text = "Local version:";
            // 
            // CheckForUpdatesButton
            // 
            this.CheckForUpdatesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckForUpdatesButton.Location = new System.Drawing.Point(12, 70);
            this.CheckForUpdatesButton.Name = "CheckForUpdatesButton";
            this.CheckForUpdatesButton.Size = new System.Drawing.Size(112, 26);
            this.CheckForUpdatesButton.TabIndex = 16;
            this.CheckForUpdatesButton.Text = "Check for updates";
            this.CheckForUpdatesButton.UseVisualStyleBackColor = true;
            this.CheckForUpdatesButton.Click += new System.EventHandler(this.CheckForUpdatesButton_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 488);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.CacheGroupBox);
            this.Controls.Add(this.RepositoryGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsDialog_Load);
            this.RepositoryGroupBox.ResumeLayout(false);
            this.CacheGroupBox.ResumeLayout(false);
            this.CacheGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button NewRepoButton;
        private System.Windows.Forms.GroupBox RepositoryGroupBox;
        private System.Windows.Forms.GroupBox CacheGroupBox;
        private System.Windows.Forms.Label CKANCachePathLabel;
        private System.Windows.Forms.Button SetCKANCacheButton;
        private System.Windows.Forms.Label CKANCacheLabel;
        private System.Windows.Forms.Button ClearCKANCacheButton;
        private System.Windows.Forms.Button DeleteRepoButton;
        private System.Windows.Forms.ListBox ReposListBox;
        private System.Windows.Forms.Button UpRepoButton;
        private System.Windows.Forms.Button DownRepoButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label LatestVersionLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LocalVersionLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CheckForUpdatesButton;
        private System.Windows.Forms.Button InstallUpdateButton;
        private System.Windows.Forms.CheckBox CheckUpdateOnLaunchCheckbox;
        private System.Windows.Forms.CheckBox RefreshOnStartupCheckbox;
        private System.Windows.Forms.CheckBox HideEpochsCheckbox;
    }
}
