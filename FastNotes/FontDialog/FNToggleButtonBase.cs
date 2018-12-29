using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FontDialog
{
    public class FNToggleButtonBase : Button
    {
        #region Private Members
        #endregion
        #region Public Members
        public static readonly RoutedEvent MouseEnterAfterCheckedEvent = EventManager.RegisterRoutedEvent(
        "MouseEnterAfterChecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FNToggleButtonBase));
        public static readonly RoutedEvent MouseLeaveAfterCheckedEvent = EventManager.RegisterRoutedEvent(
        "MouseLeaveAfterChecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FNToggleButtonBase));
        public static readonly RoutedEvent ToggleLeaveEvent = EventManager.RegisterRoutedEvent(
        "ToggleLeave", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FNToggleButtonBase));
        public static readonly RoutedEvent ToggleEnterEvent = EventManager.RegisterRoutedEvent(
        "ToggleEnter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FNToggleButtonBase));


        public event RoutedEventHandler ToggleLeave
        {
            add { AddHandler(ToggleLeaveEvent, value); }
            remove { RemoveHandler(ToggleLeaveEvent, value); }
        }


        public event RoutedEventHandler ToggleEnter
        {
            add { AddHandler(ToggleEnterEvent, value); }
            remove { RemoveHandler(ToggleEnterEvent, value); }
        }

        public event RoutedEventHandler MouseEnterAfterChecked
        {
            add { AddHandler(MouseEnterAfterCheckedEvent, value); }
            remove { RemoveHandler(MouseEnterAfterCheckedEvent, value); }
        }
        public event RoutedEventHandler MouseLeaveAfterChecked
        {
            add { AddHandler(MouseLeaveAfterCheckedEvent, value); }
            remove { RemoveHandler(MouseLeaveAfterCheckedEvent, value); }
        }

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.RegisterAttached("NormalColor",
            typeof(Color), typeof(FNToggleButtonBase), new FrameworkPropertyMetadata(null));


        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.RegisterAttached("HoverColor",
          typeof(Color), typeof(FNToggleButtonBase), new FrameworkPropertyMetadata(null));


        public static readonly DependencyProperty PressedColorProperty = DependencyProperty.RegisterAttached("PressedColor",
          typeof(Color), typeof(FNToggleButtonBase), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty NormalForeColorProperty = DependencyProperty.RegisterAttached("NormalForeColor",
  typeof(Color), typeof(FNToggleButtonBase), new FrameworkPropertyMetadata(null));


        public static readonly DependencyProperty PressedForeColorProperty = DependencyProperty.RegisterAttached("PressedForeColor",
   typeof(Color), typeof(FNToggleButtonBase), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty HoverForeColorProperty = DependencyProperty.RegisterAttached("HoverForeColor",
typeof(Color), typeof(FNToggleButtonBase), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ToggleValueProperty = DependencyProperty.RegisterAttached("ToggleValue",
          typeof(bool), typeof(FNToggleButtonBase), new FrameworkPropertyMetadata(null));



        public Color NormalForeColor
        {
            get
            {
                return (Color)GetValue(NormalForeColorProperty);
            }
            set
            {
                SetValue(NormalForeColorProperty, value);
            }
        }


        public Color HoverForeColor
        {
            get
            {
                return (Color)GetValue(HoverForeColorProperty);
            }
            set
            {
                SetValue(HoverForeColorProperty, value);
            }
        }


        public Color PressedForeColor
        {
            get
            {
                return (Color)GetValue(PressedForeColorProperty);
            }
            set
            {
                SetValue(PressedForeColorProperty, value);
            }
        }


        public Color NormalColor
        {
            get
            {
                return (Color)GetValue(NormalColorProperty);
            }
            set
            {
                SetValue(NormalColorProperty, value);
            }
        }
        public Color HoverColor
        {
            get
            {
                return (Color)GetValue(HoverColorProperty);
            }
            set
            {
                SetValue(HoverColorProperty, value);
            }
        }
        public Color PressedColor
        {
            get
            {
                return (Color)GetValue(PressedColorProperty);
            }
            set
            {
                SetValue(PressedColorProperty, value);
            }
        }
        public bool ToggleValue
        {
            get
            {
                return (bool)GetValue(ToggleValueProperty);
            }
            set
            {
                SetValue(ToggleValueProperty, value);
            }
        }
        #endregion

        #region Public Method



        public virtual void OnMouseEnterAfterChecked()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(FNToggleButtonBase.MouseEnterAfterCheckedEvent);
            RaiseEvent(newEventArgs);
        }

        public virtual void OnMouseLeaveAfterChecked()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(FNToggleButtonBase.MouseLeaveAfterCheckedEvent);
            RaiseEvent(newEventArgs);
        }


        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (!ToggleValue)
            {
                OnMouseEnterAfterChecked();
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!ToggleValue)
            {
                OnMouseLeaveAfterChecked();
            }
        }

        protected override void OnClick()
        {
            base.OnClick();
            RoutedEventArgs newEventArgs;
            if (ToggleValue) // Is marked and is clicked
            {
                // Take out the background 
         //       Background = new SolidColorBrush(HoverColor);
                newEventArgs = new RoutedEventArgs(FNToggleButtonBase.ToggleLeaveEvent);
            }
            else  // Is not marked, and is clicked
            {
                // Set a background
          //      Background = new SolidColorBrush(PressedColor);
                newEventArgs = new RoutedEventArgs(FNToggleButtonBase.ToggleEnterEvent);
            }
            RaiseEvent(newEventArgs);

            ToggleValue = !ToggleValue; // Set the new value
        }

        #endregion


    }
}
