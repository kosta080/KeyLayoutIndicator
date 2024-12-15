using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Drawing;
using System.Windows.Automation;

namespace LayoutIndicatorIcon
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")] public static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll", SetLastError = true)] public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)] public static extern bool GetCaretPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)] public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")] public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
        
        [DllImport("kernel32.dll")] private static extern uint GetCurrentThreadId();

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private static IntPtr keyboardLayoutCached;
        
        public static string GetCurrentInputLanguage()
        {
            try
            {
                IntPtr foregroundWindow = GetForegroundWindow();
                if (foregroundWindow == IntPtr.Zero)
                {
                    Console.WriteLine("No foreground window detected.");
                    return null;
                }

                uint processId = 0;
                uint threadId = GetWindowThreadProcessId(foregroundWindow, ref processId);

                // Check if the foreground window is Notepad
                string windowTitle = GetWindowTitle(foregroundWindow);
                Console.WriteLine($"Foreground Window: {foregroundWindow}, Title: {windowTitle}, Thread ID: {threadId}");

                IntPtr keyboardLayout = GetKeyboardLayout(threadId);

                if (keyboardLayoutCached == keyboardLayout) return null;
                keyboardLayoutCached = keyboardLayout;

                int layout = (int)keyboardLayout & 0xFFFF;
                string language = new CultureInfo(layout).DisplayName;
                Console.WriteLine($"Detected Input Language: {language}");
                return language;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error detecting input language: {ex.Message}");
                return null;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);

        private static string GetWindowTitle(IntPtr hwnd)
        {
            const int nChars = 256;
            System.Text.StringBuilder Buff = new System.Text.StringBuilder(nChars);
            if (GetWindowText(hwnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return string.Empty;
        }


        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);


        public static System.Drawing.Point GetCursorPosition()
        {
            GetCursorPos(out POINT point);
            return new System.Drawing.Point(point.X, point.Y);
        }

        public static System.Drawing.Point GetCaretPosition()
        {
            try
            {
                AutomationElement focusedElement = AutomationElement.FocusedElement;
                if (focusedElement == null)
                {
                    throw new Exception("No element is currently focused.");
                }

                //Console.WriteLine($"Focused Element: {focusedElement.Current.Name}, ControlType: {focusedElement.Current.ControlType.ProgrammaticName}");

                if (focusedElement.Current.ControlType != ControlType.Edit &&
                    focusedElement.Current.ControlType != ControlType.Document)
                {
                    throw new Exception("Focused element is not a text control.");
                }

                if (!focusedElement.TryGetCurrentPattern(TextPattern.Pattern, out object pattern))
                {
                    if (focusedElement.TryGetCurrentPattern(ValuePattern.Pattern, out object valuePattern))
                    {
                        string text = ((ValuePattern)valuePattern).Current.Value;
                        //Console.WriteLine($"Text content: {text}");
                        throw new Exception("TextPattern not supported, used ValuePattern instead.");
                    }
                    throw new Exception("Focused element does not support TextPattern or ValuePattern.");
                }

                var textPattern = (TextPattern)pattern;
                System.Windows.Automation.Text.TextPatternRange[] selectionRanges = textPattern.GetSelection();
                if (selectionRanges.Length == 0)
                {
                    throw new Exception("No text selection found.");
                }

                Rect[] boundingRectangles = selectionRanges[0].GetBoundingRectangles();
                if (boundingRectangles.Length == 0)
                {
                    throw new Exception("Bounding rectangle data is empty.");
                }

                return new System.Drawing.Point((int)boundingRectangles[0].X, (int)boundingRectangles[0].Y);
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error retrieving caret position: {ex.Message}");
                return new System.Drawing.Point(0, 0);
            }
        }

    }
}
