using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace USBWatcher
{
    public partial class MainForm : Form
    {
        private readonly UsbScanner _scanner = new UsbScanner();

        public MainForm()
        {
            InitializeComponent();
            LoadDrives();
        }

        private void LoadDrives()
        {
            cmbDrives.Items.Clear();
            var drives = _scanner.GetRemovableDrives();

            if (drives.Count == 0)
            {
                cmbDrives.Items.Add("未检测到U盘");
                cmbDrives.SelectedIndex = 0;
                cmbDrives.Enabled = false;
                btnRefreshDrives.Enabled = true;
            }
            else
            {
                foreach (var drive in drives)
                {
                    string label = string.IsNullOrEmpty(drive.VolumeLabel)
                        ? drive.Name.TrimEnd('\\')
                        : $"{drive.Name.TrimEnd('\\')} ({drive.VolumeLabel})";
                    cmbDrives.Items.Add(label);
                }
                cmbDrives.SelectedIndex = 0;
                cmbDrives.Enabled = true;
            }
        }

        private void BtnRefreshDrives_Click(object sender, EventArgs e)
        {
            LoadDrives();
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            ScanCurrentDrive();
        }

        private void ScanCurrentDrive()
        {
            if (cmbDrives.Items.Count == 0 || cmbDrives.SelectedIndex < 0)
                return;

            var selectedItem = cmbDrives.Items[cmbDrives.SelectedIndex]?.ToString();
            if (string.IsNullOrEmpty(selectedItem) || selectedItem == "未检测到U盘")
                return;

            char driveLetter = selectedItem[0];
            lblStatus.Text = $"正在扫描 {driveLetter}:\\ 驱动器...";
            lblStatus.Update();

            dgvHandles.Rows.Clear();
            btnScan.Enabled = false;
            btnKillProcess.Enabled = false;
            Application.DoEvents();

            var handles = _scanner.GetProcessesUsingDrive(driveLetter);

            foreach (var handle in handles)
            {
                dgvHandles.Rows.Add(handle.ProcessName, handle.ProcessId, handle.FilePath);
            }

            btnScan.Enabled = true;
            btnKillProcess.Enabled = dgvHandles.Rows.Count > 0;

            if (handles.Count == 0)
            {
                lblStatus.Text = "未检测到占用进程，可以安全弹出U盘";
            }
            else
            {
                lblStatus.Text = $"检测到 {handles.Count} 个占用进程";
            }
        }

        private void BtnKillProcess_Click(object sender, EventArgs e)
        {
            if (dgvHandles.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要结束的进程。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvHandles.SelectedRows[0];
            string processName = selectedRow.Cells[0].Value?.ToString() ?? "";
            if (int.TryParse(selectedRow.Cells[1].Value?.ToString(), out int pid))
            {
                var result = MessageBox.Show(
                    $"确定要结束进程 \"{processName}\" (PID: {pid}) 吗？",
                    "确认结束进程",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (_scanner.KillProcess(pid))
                    {
                        MessageBox.Show("进程已结束。", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ScanCurrentDrive();
                    }
                    else
                    {
                        MessageBox.Show("无法结束该进程（可能需要管理员权限）。", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnEject_Click(object sender, EventArgs e)
        {
            if (cmbDrives.Items.Count == 0 || cmbDrives.SelectedIndex < 0)
                return;

            var selectedItem = cmbDrives.Items[cmbDrives.SelectedIndex]?.ToString();
            if (string.IsNullOrEmpty(selectedItem) || selectedItem == "未检测到U盘")
                return;

            char driveLetter = selectedItem[0];
            string drivePath = $"{driveLetter}:\\";

            // Check for handles one more time
            var handles = _scanner.GetProcessesUsingDrive(driveLetter);
            if (handles.Count > 0)
            {
                var result = MessageBox.Show(
                    $"仍有 {handles.Count} 个进程占用该U盘。\n\n先结束这些进程然后弹出？",
                    "检测到占用",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dgvHandles.Rows)
                    {
                        if (int.TryParse(row.Cells[1].Value?.ToString(), out int pid))
                        {
                            _scanner.KillProcess(pid);
                        }
                    }
                    MessageBox.Show("进程已结束，现在可以在资源管理器中安全弹出U盘。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            MessageBox.Show("该U盘未被占用，可以直接拔出。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CmbDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnKillProcess.Enabled = false;
        }
    }
}
