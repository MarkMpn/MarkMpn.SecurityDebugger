
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityDebuggerPluginControl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.recordPermissionsPanel = new System.Windows.Forms.Panel();
            this.resolutionsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.executeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.retryLabel = new System.Windows.Forms.Label();
            this.requiredPrivilegeLabel = new System.Windows.Forms.Label();
            this.targetLinkLabel = new System.Windows.Forms.LinkLabel();
            this.missingPrivilegeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.userLinkLabel = new System.Windows.Forms.LinkLabel();
            this.noMatchPanel = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.errorPanel = new System.Windows.Forms.Panel();
            this.errorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.recordPermissionsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.noMatchPanel.SuspendLayout();
            this.errorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
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
            this.splitContainer1.Size = new System.Drawing.Size(857, 689);
            this.splitContainer1.SplitterDistance = 413;
            this.splitContainer1.TabIndex = 0;
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(413, 689);
            this.scintilla1.TabIndex = 0;
            this.scintilla1.Text = "Paste or open the permissions-related error message here";
            this.scintilla1.WrapMode = ScintillaNET.WrapMode.Word;
            this.scintilla1.TextChanged += new System.EventHandler(this.scintilla1_TextChanged);
            this.scintilla1.Enter += new System.EventHandler(this.scintilla1_Enter);
            // 
            // recordPermissionsPanel
            // 
            this.recordPermissionsPanel.Controls.Add(this.resolutionsListView);
            this.recordPermissionsPanel.Controls.Add(this.panel1);
            this.recordPermissionsPanel.Controls.Add(this.label1);
            this.recordPermissionsPanel.Controls.Add(this.retryLabel);
            this.recordPermissionsPanel.Controls.Add(this.requiredPrivilegeLabel);
            this.recordPermissionsPanel.Controls.Add(this.targetLinkLabel);
            this.recordPermissionsPanel.Controls.Add(this.missingPrivilegeLinkLabel);
            this.recordPermissionsPanel.Controls.Add(this.userLinkLabel);
            this.recordPermissionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recordPermissionsPanel.Location = new System.Drawing.Point(0, 149);
            this.recordPermissionsPanel.Name = "recordPermissionsPanel";
            this.recordPermissionsPanel.Padding = new System.Windows.Forms.Padding(4);
            this.recordPermissionsPanel.Size = new System.Drawing.Size(440, 540);
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
            this.resolutionsListView.Location = new System.Drawing.Point(4, 168);
            this.resolutionsListView.Name = "resolutionsListView";
            this.resolutionsListView.Size = new System.Drawing.Size(432, 339);
            this.resolutionsListView.TabIndex = 7;
            this.resolutionsListView.UseCompatibleStateImageBehavior = false;
            this.resolutionsListView.View = System.Windows.Forms.View.Details;
            this.resolutionsListView.SelectedIndexChanged += new System.EventHandler(this.resolutionsListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Description";
            this.columnHeader1.Width = 200;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.executeButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(4, 507);
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
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Possible resolutions:";
            // 
            // retryLabel
            // 
            this.retryLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.retryLabel.Location = new System.Drawing.Point(4, 105);
            this.retryLabel.Name = "retryLabel";
            this.retryLabel.Size = new System.Drawing.Size(432, 40);
            this.retryLabel.TabIndex = 8;
            this.retryLabel.Text = "This user already has the required permissions, ask the user to retry the operati" +
    "on and obtain an updated log file in case of any further errors";
            this.retryLabel.Visible = false;
            // 
            // requiredPrivilegeLabel
            // 
            this.requiredPrivilegeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.requiredPrivilegeLabel.Location = new System.Drawing.Point(4, 65);
            this.requiredPrivilegeLabel.Name = "requiredPrivilegeLabel";
            this.requiredPrivilegeLabel.Size = new System.Drawing.Size(432, 40);
            this.requiredPrivilegeLabel.TabIndex = 3;
            this.requiredPrivilegeLabel.Text = "To resolve this error, the user needs to be granted the prvWriteAccount privilege" +
    " to Global depth";
            // 
            // targetLinkLabel
            // 
            this.targetLinkLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.targetLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(15, 5);
            this.targetLinkLabel.Location = new System.Drawing.Point(4, 38);
            this.targetLinkLabel.Name = "targetLinkLabel";
            this.targetLinkLabel.Size = new System.Drawing.Size(432, 27);
            this.targetLinkLabel.TabIndex = 2;
            this.targetLinkLabel.TabStop = true;
            this.targetLinkLabel.Text = "on the account Data8";
            this.targetLinkLabel.UseCompatibleTextRendering = true;
            // 
            // missingPrivilegeLinkLabel
            // 
            this.missingPrivilegeLinkLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.missingPrivilegeLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.missingPrivilegeLinkLabel.Location = new System.Drawing.Point(4, 21);
            this.missingPrivilegeLinkLabel.Name = "missingPrivilegeLinkLabel";
            this.missingPrivilegeLinkLabel.Size = new System.Drawing.Size(432, 17);
            this.missingPrivilegeLinkLabel.TabIndex = 1;
            this.missingPrivilegeLinkLabel.Text = "does not have write permission";
            // 
            // userLinkLabel
            // 
            this.userLinkLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.userLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(9, 15);
            this.userLinkLabel.Location = new System.Drawing.Point(4, 4);
            this.userLinkLabel.Name = "userLinkLabel";
            this.userLinkLabel.Size = new System.Drawing.Size(432, 17);
            this.userLinkLabel.TabIndex = 0;
            this.userLinkLabel.TabStop = true;
            this.userLinkLabel.Text = "The user Mark Carrington";
            this.userLinkLabel.UseCompatibleTextRendering = true;
            // 
            // noMatchPanel
            // 
            this.noMatchPanel.Controls.Add(this.linkLabel1);
            this.noMatchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.noMatchPanel.Location = new System.Drawing.Point(0, 100);
            this.noMatchPanel.Name = "noMatchPanel";
            this.noMatchPanel.Padding = new System.Windows.Forms.Padding(4);
            this.noMatchPanel.Size = new System.Drawing.Size(440, 49);
            this.noMatchPanel.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(163, 25);
            this.linkLabel1.Location = new System.Drawing.Point(4, 4);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(432, 41);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = resources.GetString("linkLabel1.Text");
            this.linkLabel1.UseCompatibleTextRendering = true;
            // 
            // errorPanel
            // 
            this.errorPanel.Controls.Add(this.errorLabel);
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
            this.errorLabel.Location = new System.Drawing.Point(4, 4);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(432, 92);
            this.errorLabel.TabIndex = 0;
            this.errorLabel.Text = "Error";
            // 
            // SecurityDebuggerPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SecurityDebuggerPluginControl";
            this.Size = new System.Drawing.Size(857, 689);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.recordPermissionsPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.noMatchPanel.ResumeLayout(false);
            this.errorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.Panel recordPermissionsPanel;
        private System.Windows.Forms.Panel noMatchPanel;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel targetLinkLabel;
        private System.Windows.Forms.LinkLabel missingPrivilegeLinkLabel;
        private System.Windows.Forms.LinkLabel userLinkLabel;
        private System.Windows.Forms.Label requiredPrivilegeLabel;
        private System.Windows.Forms.ListView resolutionsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel errorPanel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label retryLabel;
    }
}
