using System.Windows.Forms;
using System.ComponentModel;

namespace LayoutIndicatorIcon
{
    partial class Form1
    {
        private IContainer components = null;
        private PictureBox pictureBox1;
        private NotifyIcon notifyIcon;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = Resources.appIcon; // Replace with your app icon
            this.notifyIcon.Text = "Layout Indicator Icon"; // Tooltip text
            this.notifyIcon.Visible = true; // Make the icon visible
            this.notifyIcon.ContextMenuStrip = CreateContextMenu(); // Add a context menu (optional)
            
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32); // Set the icon size
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent; // Make the PictureBox transparent
            this.Controls.Add(this.pictureBox1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(32, 32); // Match form size to icon size
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; // Remove borders
            this.ShowInTaskbar = false; // Hide the form from the taskbar
            this.TopMost = true; // Ensure the form is always on top
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.BackColor = System.Drawing.Color.Lime; // Temporary background color (will be transparent)
            this.TransparencyKey = System.Drawing.Color.Lime; // Set the transparency color
            this.Name = "Form1";
            this.ResumeLayout(false);
        }
        
        private ContextMenuStrip CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) => Application.Exit();
            contextMenu.Items.Add(exitItem);
            return contextMenu;
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.notifyIcon.Dispose(); // Clean up the tray icon
        }
        
        
    }
}