using Timer = System.Windows.Forms.Timer;

namespace LayoutIndicatorIcon
{
    public partial class Form1 : Form
    {
        private Timer timer;

        public Form1()
        {
            InitializeComponent();

            // Load default icon for PictureBox
            pictureBox1.Image = LayoutIndicatorIcon.Resources.enIcon;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            // Timer to periodically update the form position and icon
            timer = new Timer();
            timer.Interval = 16; // Check every 500ms
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static Icon ConvertBitmapToIcon(Bitmap bitmap)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                return new Icon(memoryStream);
            }
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update input language and position
            UpdatePosition();
            
            string inputLanguage = NativeMethods.GetCurrentInputLanguage();
            if (inputLanguage == null) return;
            UpdateIcon(inputLanguage);
        }

        private void UpdateIcon(string inputLanguage)
        {
            if (inputLanguage.Contains("English"))
                pictureBox1.Image = Resources.enIcon; // Replace with your icon
            else if (inputLanguage.Contains("Russian"))
                pictureBox1.Image = Resources.ruIcon;
            else if (inputLanguage.Contains("Hebrew"))
                pictureBox1.Image = Resources.heIcon;
            else
                pictureBox1.Image = Resources.heIcon;
        }
        
        private void UpdatePosition()
        {
            // Get the current cursor position
            NativeMethods.POINT cursorPosition = NativeMethods.GetCursorPosition();

            // Offset the form to appear near the cursor
            this.Location = new Point(cursorPosition.X + 10, cursorPosition.Y + 10);
        }

        private void PositionOverlay(NativeMethods.POINT caretPosition)
        {
            // Move the overlay window near the caret
            this.Location = new Point(caretPosition.X + 10, caretPosition.Y + 10); // Adjust offset as needed
        }
        
        protected override void WndProc(ref Message m)
        {
            const int WM_INPUTLANGCHANGE = 0x51;

            if (m.Msg == WM_INPUTLANGCHANGE)
            {
                // Handle language change
                string inputLanguage = NativeMethods.GetCurrentInputLanguage();
                if (inputLanguage == null) return;
                UpdateIcon(inputLanguage);
            }

            base.WndProc(ref m);
        }
        
    }
}