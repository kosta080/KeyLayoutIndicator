using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace LayoutIndicatorIcon
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        
        [DllImport("user32.dll")]
        public static extern bool GetCaretPos(out POINT lpPoint);
        
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public static POINT GetCursorPosition()
        {
            GetCursorPos(out POINT point);
            return point;
        }

        private static IntPtr keyboardLayoutCashed;
        public static string GetCurrentInputLanguage()
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            uint processId = 0; // Initialize a variable to store the process ID
            uint threadId = NativeMethods.GetWindowThreadProcessId(foregroundWindow, ref processId);
            IntPtr keyboardLayout = GetKeyboardLayout(threadId);
            if (keyboardLayoutCashed == keyboardLayout) return null;
            keyboardLayoutCashed = keyboardLayout;
            int layout = (int)keyboardLayout & 0xFFFF;
            return new CultureInfo(layout).DisplayName;
        }

        public static POINT GetCaretPosition()
        {
            if (GetCaretPos(out POINT point))
                return point;

            // Default to (0, 0) if caret position retrieval fails
            return new POINT { X = 0, Y = 0 };
        }
    }
}