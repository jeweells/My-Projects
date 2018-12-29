using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FastNotes
{
    public class FNPopupBase : Popup
    {


        public bool KeepOpen
        {
            get { return (bool)GetValue(KeepOpenProperty); }
            set { SetValue(KeepOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeepOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeepOpenProperty =
            DependencyProperty.Register("KeepOpen", typeof(bool), typeof(FNPopupBase), new PropertyMetadata(false));

        public FNPopupBase()
        {
            IsVisibleChanged += (a, b) =>
            {
                Mouse.Capture(this, CaptureMode.SubTree);
            };
            Loaded += (a, b) =>
            {

                Program.MainWindow.Deactivated += (c, d) =>
                {
                    if (!KeepOpen)
                    {
                        IsOpen = false;
                    }
                };
                
            };
            
        }
    }
}
