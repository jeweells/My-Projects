using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FastNotes
{
    public class FNRichTextBoxBase : RichTextBox
    {
        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            base.OnSelectionChanged(e);
            //TextPointer start = CaretPosition;
            //FrameworkContentElement fce = (start.Parent as FrameworkContentElement);
            //if (fce != null)
            //{
            //    Debug.WriteLine("Bringing");
            //    fce.BringIntoView();
            //}
            // Rectangle corresponding to the coordinates of the selected text.
            Rect screenPos = Selection.Start.GetCharacterRect(LogicalDirection.Forward);
            double offset = screenPos.Top + VerticalOffset;
            // The offset - half the size of the RichtextBox to keep the selection centered.
            if(screenPos.Top < 0)
            {
                ScrollToVerticalOffset(screenPos.Top + VerticalOffset);
            }
            else if(screenPos.Bottom > ActualHeight)
            {
                ScrollToVerticalOffset(screenPos.Bottom - ActualHeight + VerticalOffset);
            }
        //    ScrollToVerticalOffset(offset);
        }
    }
}
