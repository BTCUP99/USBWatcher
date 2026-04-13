namespace USBWatcher
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cmbDrives = new System.Windows.Forms.ComboBox();
            this.btnRefreshDrives = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.dgvHandles = new System.Windows.Forms.DataGridView();
            this.colProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnKillProcess = new System.Windows.Forms.Button();
            this.btnEject = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpDrives = new System.Windows.Forms.GroupBox();
            this.grpHandles = new System.Windows.Forms.GroupBox();
            this.grpActions = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHandles)).BeginInit();
            this.grpDrives.SuspendLayout();
            this.grpHandles.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.SuspendLayout();
            //
            // cmbDrives
            //
            this.cmbDrives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDrives.FormattingEnabled = true;
            this.cmbDrives.Location = new System.Drawing.Point(15, 30);
            this.cmbDrives.Name = "cmbDrives";
            this.cmbDrives.Size = new System.Drawing.Size(350, 25);
            this.cmbDrives.TabIndex = 0;
            this.cmbDrives.SelectedIndexChanged += new System.EventHandler(this.CmbDrives_SelectedIndexChanged);
            //
            // btnRefreshDrives
            //
            this.btnRefreshDrives.Location = new System.Drawing.Point(380, 28);
            this.btnRefreshDrives.Name = "btnRefreshDrives";
            this.btnRefreshDrives.Size = new System.Drawing.Size(90, 30);
            this.btnRefreshDrives.TabIndex = 1;
            this.btnRefreshDrives.Text = "刷新";
            this.btnRefreshDrives.UseVisualStyleBackColor = true;
            this.btnRefreshDrives.Click += new System.EventHandler(this.BtnRefreshDrives_Click);
            //
            // btnScan
            //
            this.btnScan.Location = new System.Drawing.Point(15, 30);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(100, 35);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "扫描占用";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.BtnScan_Click);
            //
            // dgvHandles
            //
            this.dgvHandles.AllowUserToAddRows = false;
            this.dgvHandles.AllowUserToDeleteRows = false;
            this.dgvHandles.AllowUserToResizeRows = false;
            this.dgvHandles.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvHandles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHandles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProcessName,
            this.colPid,
            this.colFilePath});
            this.dgvHandles.Location = new System.Drawing.Point(15, 30);
            this.dgvHandles.MultiSelect = false;
            this.dgvHandles.Name = "dgvHandles";
            this.dgvHandles.ReadOnly = true;
            this.dgvHandles.RowHeadersWidth = 5;
            this.dgvHandles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHandles.Size = new System.Drawing.Size(610, 280);
            this.dgvHandles.TabIndex = 3;
            //
            // colProcessName
            //
            this.colProcessName.HeaderText = "进程名";
            this.colProcessName.Name = "colProcessName";
            this.colProcessName.ReadOnly = true;
            this.colProcessName.Width = 120;
            //
            // colPid
            //
            this.colPid.HeaderText = "PID";
            this.colPid.Name = "colPid";
            this.colPid.ReadOnly = true;
            this.colPid.Width = 70;
            //
            // colFilePath
            //
            this.colFilePath.HeaderText = "占用文件路径";
            this.colFilePath.Name = "colFilePath";
            this.colFilePath.ReadOnly = true;
            this.colFilePath.Width = 400;
            //
            // btnKillProcess
            //
            this.btnKillProcess.Enabled = false;
            this.btnKillProcess.Location = new System.Drawing.Point(130, 30);
            this.btnKillProcess.Name = "btnKillProcess";
            this.btnKillProcess.Size = new System.Drawing.Size(100, 35);
            this.btnKillProcess.TabIndex = 4;
            this.btnKillProcess.Text = "结束进程";
            this.btnKillProcess.UseVisualStyleBackColor = true;
            this.btnKillProcess.Click += new System.EventHandler(this.BtnKillProcess_Click);
            //
            // btnEject
            //
            this.btnEject.Location = new System.Drawing.Point(500, 30);
            this.btnEject.Name = "btnEject";
            this.btnEject.Size = new System.Drawing.Size(125, 35);
            this.btnEject.TabIndex = 5;
            this.btnEject.Text = "安全弹出U盘";
            this.btnEject.UseVisualStyleBackColor = true;
            this.btnEject.Click += new System.EventHandler(this.BtnEject_Click);
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(15, 325);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            this.lblStatus.TabIndex = 6;
            //
            // grpDrives
            //
            this.grpDrives.Controls.Add(this.cmbDrives);
            this.grpDrives.Controls.Add(this.btnRefreshDrives);
            this.grpDrives.Location = new System.Drawing.Point(15, 15);
            this.grpDrives.Name = "grpDrives";
            this.grpDrives.Size = new System.Drawing.Size(490, 70);
            this.grpDrives.TabIndex = 7;
            this.grpDrives.TabStop = false;
            this.grpDrives.Text = "选择U盘";
            //
            // grpHandles
            //
            this.grpHandles.Controls.Add(this.dgvHandles);
            this.grpHandles.Location = new System.Drawing.Point(15, 95);
            this.grpHandles.Name = "grpHandles";
            this.grpHandles.Size = new System.Drawing.Size(640, 330);
            this.grpHandles.TabIndex = 8;
            this.grpHandles.TabStop = false;
            this.grpHandles.Text = "占用进程列表";
            //
            // grpActions
            //
            this.grpActions.Controls.Add(this.btnScan);
            this.grpActions.Controls.Add(this.btnKillProcess);
            this.grpActions.Controls.Add(this.btnEject);
            this.grpActions.Location = new System.Drawing.Point(520, 15);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(135, 70);
            this.grpActions.TabIndex = 9;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "操作";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 450);
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.grpHandles);
            this.Controls.Add(this.grpDrives);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "U盘占用检查工具";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHandles)).EndInit();
            this.grpDrives.ResumeLayout(false);
            this.grpHandles.ResumeLayout(false);
            this.grpActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDrives;
        private System.Windows.Forms.Button btnRefreshDrives;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridView dgvHandles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilePath;
        private System.Windows.Forms.Button btnKillProcess;
        private System.Windows.Forms.Button btnEject;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpDrives;
        private System.Windows.Forms.GroupBox grpHandles;
        private System.Windows.Forms.GroupBox grpActions;
    }
}
