﻿
namespace MarkMpn.SecurityDebugger
{
    partial class SecurityDebuggerPluginControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityDebuggerPluginControl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.recordPermissionsPanel = new System.Windows.Forms.Panel();
            this.resolutionsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.retryPanel = new System.Windows.Forms.Panel();
            this.retryLabel = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.requiredPermissionPanel = new System.Windows.Forms.Panel();
            this.requiredPrivilegeLabel = new System.Windows.Forms.Label();
            this.requiredDepthImage = new System.Windows.Forms.PictureBox();
            this.decodedPanel = new System.Windows.Forms.Panel();
            this.targetLinkLabel = new System.Windows.Forms.LinkLabel();
            this.missingPrivilegeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.userLinkLabel = new System.Windows.Forms.LinkLabel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.executeButton = new System.Windows.Forms.Button();
            this.noMatchPanel = new System.Windows.Forms.Panel();
            this.createIssueLinkLabel = new System.Windows.Forms.LinkLabel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.errorPanel = new System.Windows.Forms.Panel();
            this.errorLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.aboutToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.recordPermissionsPanel.SuspendLayout();
            this.retryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.requiredPermissionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.requiredDepthImage)).BeginInit();
            this.decodedPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel1.SuspendLayout();
            this.noMatchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.errorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scintilla1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.recordPermissionsPanel);
            this.splitContainer1.Panel2.Controls.Add(this.noMatchPanel);
            this.splitContainer1.Panel2.Controls.Add(this.errorPanel);
            this.splitContainer1.Size = new System.Drawing.Size(857, 664);
            this.splitContainer1.SplitterDistance = 413;
            this.splitContainer1.TabIndex = 0;
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(413, 664);
            this.scintilla1.TabIndex = 0;
            this.scintilla1.Text = "Paste or open the permissions-related error message here";
            this.scintilla1.WrapMode = ScintillaNET.WrapMode.Word;
            this.scintilla1.TextChanged += new System.EventHandler(this.scintilla1_TextChanged);
            this.scintilla1.Enter += new System.EventHandler(this.scintilla1_Enter);
            // 
            // recordPermissionsPanel
            // 
            this.recordPermissionsPanel.Controls.Add(this.resolutionsListView);
            this.recordPermissionsPanel.Controls.Add(this.label1);
            this.recordPermissionsPanel.Controls.Add(this.retryPanel);
            this.recordPermissionsPanel.Controls.Add(this.requiredPermissionPanel);
            this.recordPermissionsPanel.Controls.Add(this.decodedPanel);
            this.recordPermissionsPanel.Controls.Add(this.panel1);
            this.recordPermissionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recordPermissionsPanel.Location = new System.Drawing.Point(0, 149);
            this.recordPermissionsPanel.Name = "recordPermissionsPanel";
            this.recordPermissionsPanel.Padding = new System.Windows.Forms.Padding(4);
            this.recordPermissionsPanel.Size = new System.Drawing.Size(440, 515);
            this.recordPermissionsPanel.TabIndex = 1;
            this.recordPermissionsPanel.Visible = false;
            // 
            // resolutionsListView
            // 
            this.resolutionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.resolutionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resolutionsListView.FullRowSelect = true;
            this.resolutionsListView.HideSelection = false;
            this.resolutionsListView.Location = new System.Drawing.Point(4, 147);
            this.resolutionsListView.Name = "resolutionsListView";
            this.resolutionsListView.Size = new System.Drawing.Size(432, 335);
            this.resolutionsListView.SmallImageList = this.imageList;
            this.resolutionsListView.TabIndex = 12;
            this.resolutionsListView.UseCompatibleStateImageBehavior = false;
            this.resolutionsListView.View = System.Windows.Forms.View.Details;
            this.resolutionsListView.SelectedIndexChanged += new System.EventHandler(this.resolutionsListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Description";
            this.columnHeader1.Width = 200;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "SecurityRole.png");
            this.imageList.Images.SetKeyName(1, "SystemUser.png");
            this.imageList.Images.SetKeyName(2, "ShareRecord.png");
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "Possible resolutions:";
            // 
            // retryPanel
            // 
            this.retryPanel.Controls.Add(this.retryLabel);
            this.retryPanel.Controls.Add(this.pictureBox4);
            this.retryPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.retryPanel.Location = new System.Drawing.Point(4, 92);
            this.retryPanel.Name = "retryPanel";
            this.retryPanel.Size = new System.Drawing.Size(432, 32);
            this.retryPanel.TabIndex = 10;
            this.retryPanel.Visible = false;
            // 
            // retryLabel
            // 
            this.retryLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.retryLabel.Location = new System.Drawing.Point(17, 0);
            this.retryLabel.Name = "retryLabel";
            this.retryLabel.Size = new System.Drawing.Size(415, 32);
            this.retryLabel.TabIndex = 9;
            this.retryLabel.Text = "This user already has the required permissions, ask the user to retry the operati" +
    "on and obtain an updated log file in case of any further errors";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox4.Image = global::MarkMpn.SecurityDebugger.Properties.Resources.StatusSecurityOK_16x;
            this.pictureBox4.Location = new System.Drawing.Point(0, 0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(17, 32);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // requiredPermissionPanel
            // 
            this.requiredPermissionPanel.Controls.Add(this.requiredPrivilegeLabel);
            this.requiredPermissionPanel.Controls.Add(this.requiredDepthImage);
            this.requiredPermissionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.requiredPermissionPanel.Location = new System.Drawing.Point(4, 58);
            this.requiredPermissionPanel.Name = "requiredPermissionPanel";
            this.requiredPermissionPanel.Size = new System.Drawing.Size(432, 34);
            this.requiredPermissionPanel.TabIndex = 7;
            // 
            // requiredPrivilegeLabel
            // 
            this.requiredPrivilegeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.requiredPrivilegeLabel.Location = new System.Drawing.Point(17, 0);
            this.requiredPrivilegeLabel.Name = "requiredPrivilegeLabel";
            this.requiredPrivilegeLabel.Size = new System.Drawing.Size(415, 34);
            this.requiredPrivilegeLabel.TabIndex = 6;
            this.requiredPrivilegeLabel.Text = "To resolve this error, the user needs to be granted the prvWriteAccount privilege" +
    " to Global depth";
            // 
            // requiredDepthImage
            // 
            this.requiredDepthImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.requiredDepthImage.Location = new System.Drawing.Point(0, 0);
            this.requiredDepthImage.Name = "requiredDepthImage";
            this.requiredDepthImage.Size = new System.Drawing.Size(17, 34);
            this.requiredDepthImage.TabIndex = 0;
            this.requiredDepthImage.TabStop = false;
            // 
            // decodedPanel
            // 
            this.decodedPanel.Controls.Add(this.targetLinkLabel);
            this.decodedPanel.Controls.Add(this.missingPrivilegeLinkLabel);
            this.decodedPanel.Controls.Add(this.userLinkLabel);
            this.decodedPanel.Controls.Add(this.pictureBox3);
            this.decodedPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.decodedPanel.Location = new System.Drawing.Point(4, 4);
            this.decodedPanel.Name = "decodedPanel";
            this.decodedPanel.Size = new System.Drawing.Size(432, 54);
            this.decodedPanel.TabIndex = 9;
            // 
            // targetLinkLabel
            // 
            this.targetLinkLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.targetLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(15, 5);
            this.targetLinkLabel.Location = new System.Drawing.Point(17, 34);
            this.targetLinkLabel.Name = "targetLinkLabel";
            this.targetLinkLabel.Size = new System.Drawing.Size(415, 27);
            this.targetLinkLabel.TabIndex = 5;
            this.targetLinkLabel.TabStop = true;
            this.targetLinkLabel.Text = "on the account Data8";
            this.targetLinkLabel.UseCompatibleTextRendering = true;
            this.targetLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.targetLinkLabel_LinkClicked);
            // 
            // missingPrivilegeLinkLabel
            // 
            this.missingPrivilegeLinkLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.missingPrivilegeLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.missingPrivilegeLinkLabel.Location = new System.Drawing.Point(17, 17);
            this.missingPrivilegeLinkLabel.Name = "missingPrivilegeLinkLabel";
            this.missingPrivilegeLinkLabel.Size = new System.Drawing.Size(415, 17);
            this.missingPrivilegeLinkLabel.TabIndex = 4;
            this.missingPrivilegeLinkLabel.Text = "does not have write permission";
            // 
            // userLinkLabel
            // 
            this.userLinkLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.userLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(9, 15);
            this.userLinkLabel.Location = new System.Drawing.Point(17, 0);
            this.userLinkLabel.Name = "userLinkLabel";
            this.userLinkLabel.Size = new System.Drawing.Size(415, 17);
            this.userLinkLabel.TabIndex = 3;
            this.userLinkLabel.TabStop = true;
            this.userLinkLabel.Text = "The user Mark Carrington";
            this.userLinkLabel.UseCompatibleTextRendering = true;
            this.userLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.userLinkLabel_LinkClicked);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox3.Image = global::MarkMpn.SecurityDebugger.Properties.Resources.StatusSecurityWarning_grey_16x;
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(17, 54);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.executeButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(4, 482);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 29);
            this.panel1.TabIndex = 6;
            // 
            // executeButton
            // 
            this.executeButton.Enabled = false;
            this.executeButton.Location = new System.Drawing.Point(3, 3);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(75, 23);
            this.executeButton.TabIndex = 6;
            this.executeButton.Text = "Execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // noMatchPanel
            // 
            this.noMatchPanel.Controls.Add(this.createIssueLinkLabel);
            this.noMatchPanel.Controls.Add(this.pictureBox2);
            this.noMatchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.noMatchPanel.Location = new System.Drawing.Point(0, 100);
            this.noMatchPanel.Name = "noMatchPanel";
            this.noMatchPanel.Padding = new System.Windows.Forms.Padding(4);
            this.noMatchPanel.Size = new System.Drawing.Size(440, 49);
            this.noMatchPanel.TabIndex = 0;
            // 
            // createIssueLinkLabel
            // 
            this.createIssueLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createIssueLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(163, 25);
            this.createIssueLinkLabel.Location = new System.Drawing.Point(21, 4);
            this.createIssueLinkLabel.Name = "createIssueLinkLabel";
            this.createIssueLinkLabel.Size = new System.Drawing.Size(415, 41);
            this.createIssueLinkLabel.TabIndex = 2;
            this.createIssueLinkLabel.TabStop = true;
            this.createIssueLinkLabel.Text = resources.GetString("createIssueLinkLabel.Text");
            this.createIssueLinkLabel.UseCompatibleTextRendering = true;
            this.createIssueLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.createIssueLinkLabel_LinkClicked);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.Image = global::MarkMpn.SecurityDebugger.Properties.Resources.StatusInformation_16x;
            this.pictureBox2.Location = new System.Drawing.Point(4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(17, 41);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // errorPanel
            // 
            this.errorPanel.Controls.Add(this.errorLabel);
            this.errorPanel.Controls.Add(this.pictureBox1);
            this.errorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.errorPanel.Location = new System.Drawing.Point(0, 0);
            this.errorPanel.Name = "errorPanel";
            this.errorPanel.Padding = new System.Windows.Forms.Padding(4);
            this.errorPanel.Size = new System.Drawing.Size(440, 100);
            this.errorPanel.TabIndex = 2;
            this.errorPanel.Visible = false;
            // 
            // errorLabel
            // 
            this.errorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorLabel.Location = new System.Drawing.Point(21, 4);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(415, 92);
            this.errorLabel.TabIndex = 2;
            this.errorLabel.Text = "Error";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::MarkMpn.SecurityDebugger.Properties.Resources.StatusCriticalError_16x;
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(17, 92);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.aboutToolStripLabel});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(857, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(56, 22);
            this.toolStripButton1.Text = "Open";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "txt";
            this.openFileDialog.Filter = "Error Log (ErrorDetails*.txt)|ErrorDetails*.txt";
            // 
            // aboutToolStripLabel
            // 
            this.aboutToolStripLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.aboutToolStripLabel.Image = global::MarkMpn.SecurityDebugger.Properties.Resources.StatusSecurityWarning_grey_16x;
            this.aboutToolStripLabel.IsLink = true;
            this.aboutToolStripLabel.Name = "aboutToolStripLabel";
            this.aboutToolStripLabel.Size = new System.Drawing.Size(226, 22);
            this.aboutToolStripLabel.Text = "Security Debugger by Mark Carrington";
            this.aboutToolStripLabel.Click += new System.EventHandler(this.aboutToolStripLabel_Click);
            // 
            // SecurityDebuggerPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Name = "SecurityDebuggerPluginControl";
            this.Size = new System.Drawing.Size(857, 689);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.recordPermissionsPanel.ResumeLayout(false);
            this.retryPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.requiredPermissionPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.requiredDepthImage)).EndInit();
            this.decodedPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel1.ResumeLayout(false);
            this.noMatchPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.errorPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.Panel recordPermissionsPanel;
        private System.Windows.Forms.Panel noMatchPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.Panel errorPanel;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel decodedPanel;
        private System.Windows.Forms.LinkLabel targetLinkLabel;
        private System.Windows.Forms.LinkLabel missingPrivilegeLinkLabel;
        private System.Windows.Forms.LinkLabel userLinkLabel;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.LinkLabel createIssueLinkLabel;
        private System.Windows.Forms.Panel retryPanel;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label requiredPrivilegeLabel;
        private System.Windows.Forms.ListView resolutionsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label retryLabel;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel requiredPermissionPanel;
        private System.Windows.Forms.PictureBox requiredDepthImage;
        private System.Windows.Forms.ToolStripLabel aboutToolStripLabel;
    }
}
