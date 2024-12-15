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

            Console.WriteLine($"Updating Icon for Language: {inputLanguage}");
            UpdateIcon(inputLanguage);
        }

        private void UpdateIcon(string inputLanguage)
        {
            if (inputLanguage.IndexOf("English", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                pictureBox1.Image = Resources.enIcon;
            }
            else if (inputLanguage.IndexOf("Russian", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                pictureBox1.Image = Resources.ruIcon;
            }
            else if (inputLanguage.IndexOf("Hebrew", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                pictureBox1.Image = Resources.heIcon;
            }
            else
            {
                Console.WriteLine($"Unknown language: {inputLanguage}, setting default icon.");
                pictureBox1.Image = Resources.heIcon; // Default icon
            }
        }
        
        private bool useTypingPosition = true;
        private void UpdatePlacementMode(bool typingPosition)
        {
            useTypingPosition = typingPosition;
            
        }
        private void UpdatePosition()
        {
            Point position;
            if (useTypingPosition)
            {
                // Get the typing (caret) position
                position = NativeMethods.GetCaretPosition();
            }
            else
            {
                // Get the cursor position
                position = NativeMethods.GetCursorPosition();
            }

            // Offset the form to appear near the cursor
            this.Location = new Point(position.X + 10, position.Y + 10);
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
                string inputLanguage = NativeMethods.GetCurrentInputLanguage();
                if (inputLanguage == null) return;

                Console.WriteLine($"Language Changed to: {inputLanguage}");
                UpdateIcon(inputLanguage);
            }

            base.WndProc(ref m);
        }
        
    }
}