using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Controls
{
    public class HighlightableMenuItemBase : MenuItem
    {

        public static readonly RoutedEvent HoverEnterEvent = EventManager.RegisterRoutedEvent(
"HoverEnter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HighlightableMenuItemBase));


        public static readonly RoutedEvent HoverLeaveEvent = EventManager.RegisterRoutedEvent(
"HoverLeave", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HighlightableMenuItemBase));


        public bool CaptureHover
        {
            get { return (bool)GetValue(CaptureHoverProperty); }
            set { SetValue(CaptureHoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaptureHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptureHoverProperty =
            DependencyProperty.Register("CaptureHover", typeof(bool), typeof(HighlightableMenuItemBase), new PropertyMetadata(false));



        public event RoutedEventHandler HoverEnter
        {
            add { AddHandler(HoverEnterEvent, value); }
            remove { RemoveHandler(HoverEnterEvent, value); }
        }

        public event RoutedEventHandler HoverLeave
        {
            add { AddHandler(HoverLeaveEvent, value); }
            remove { RemoveHandler(HoverLeaveEvent, value); }
        }


        public bool IsHover
        {
            get { return (bool)GetValue(IsHoverProperty); }
            set { SetValue(IsHoverProperty, value); }
        }

        /// <summary>
        /// This function helps calling an event to listen to IsHover changes
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnHoverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = ((HighlightableMenuItemBase)d);
            if ((bool)e.NewValue) item.RaiseEvent(new RoutedEventArgs(HoverEnterEvent));
             else if(!item.CaptureHover) item.RaiseEvent(new RoutedEventArgs(HoverLeaveEvent));
            if (item.CaptureHover) item.IsHighlighted = true;

        }
        // Using a DependencyProperty as the backing store for IsHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHoverProperty =
            DependencyProperty.Register("IsHover", typeof(bool), typeof(HighlightableMenuItemBase), new PropertyMetadata(false, new PropertyChangedCallback(OnHoverChanged)));


        public Color NormalColor
        {
            get { return (Color)GetValue(NormalColorProperty); }
            set { SetValue(NormalColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalColorProperty =
            DependencyProperty.Register("NormalColor", typeof(Color), typeof(HighlightableMenuItemBase), new PropertyMetadata(null));




        public Color HoverColor
        {
            get { return (Color)GetValue(HoverColorProperty); }
            set { SetValue(HoverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverColorProperty =
            DependencyProperty.Register("HoverColor", typeof(Color), typeof(HighlightableMenuItemBase), new PropertyMetadata(null));




    }
}
