using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FastNotes
{
    public class ScrollViewerScalableBase : ScrollViewer, INotifyPropertyChanged
    {
        #region Public Members

        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set {
                if (value < 0.25 || value > 9) return;
                SetValue(ZoomFactorProperty, value);
                NotifyPropertyChanged(nameof(ZoomFactor));
                RaiseEvent(new RoutedEventArgs(ScrollViewerScalableBase.ZoomChangedEvent));
            }
        }
        public static readonly RoutedEvent ZoomChangedEvent = EventManager.RegisterRoutedEvent(
"ZoomChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerScalableBase));


        public event RoutedEventHandler ZoomChanged
        {
            add { AddHandler(ZoomChangedEvent, value); }
            remove { RemoveHandler(ZoomChangedEvent, value); }
        }


        // Using a DependencyProperty as the backing store for ZoomFactor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(ScrollViewerScalableBase), new PropertyMetadata(3.0));

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Members
        protected NoteWindowBaseModel DataContainer;


        #endregion

        #region Constructor

        public ScrollViewerScalableBase()
        {
            DataContextChanged += (a, b) =>
            {
                DataContainer = (NoteWindowBaseModel)DataContext;
            };
        }
        #endregion

        #region Protected Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (Keyboard.IsKeyDown(Key.Add))
                {
                    ZoomFactor += 0.25;
                }
                else if (Keyboard.IsKeyDown(Key.Subtract))
                {
                    ZoomFactor -= 0.25;

                }
                else if (Keyboard.IsKeyDown(Key.NumPad0) || Keyboard.IsKeyDown(Key.NumPad1))
                {
                    ZoomFactor = 1;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad1))
                {
                    ZoomFactor = 1;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad2))
                {
                    ZoomFactor = 2;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad3))
                {
                    ZoomFactor = 3;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad4))
                {
                    ZoomFactor = 4;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad5))
                {
                    ZoomFactor = 5;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad6))
                {
                    ZoomFactor = 6;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad7))
                {
                    ZoomFactor = 7;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad8))
                {
                    ZoomFactor = 8;
                }
                else if (Keyboard.IsKeyDown(Key.NumPad9))
                {
                    ZoomFactor = 9;
                }



            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                // Zoom in/out
                ZoomFactor += e.Delta / 120.0 * .25;
            }
            else
            {
                // Scroll through
                ScrollToVerticalOffset(VerticalOffset - e.Delta / 2.0);
            }
        }

        #endregion

        #region Public Methods
        public void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
