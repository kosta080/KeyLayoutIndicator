using System.Windows;
using System.Windows.Automation;

public static class CaretHelper
{
    public static System.Drawing.Point GetCaretPosition()
    {
        try
        {
            // Get the currently focused element
            AutomationElement focusedElement = AutomationElement.FocusedElement;

            if (focusedElement == null)
            {
                throw new Exception("No element is currently focused.");
            }

            // Check if the focused element supports the TextPattern
            if (!focusedElement.TryGetCurrentPattern(TextPattern.Pattern, out object pattern))
            {
                throw new Exception("The focused element does not support the TextPattern.");
            }

            var textPattern = (TextPattern)pattern;

            // Get the text selection (caret position is at the end of the selection)
            System.Windows.Automation.Text.TextPatternRange[] selectionRanges = textPattern.GetSelection();
            if (selectionRanges.Length == 0)
            {
                throw new Exception("No text selection found.");
            }

            var boundingRectangles = selectionRanges[0].GetBoundingRectangles();
            if (boundingRectangles.Length < 4)
            {
                throw new Exception("Invalid bounding rectangle data.");
            }

            return new System.Drawing.Point((int)boundingRectangles[0].X, (int)boundingRectangles[0].X);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving caret position: {ex.Message}");
            return new System.Drawing.Point(0, 0);
        }
    }
}