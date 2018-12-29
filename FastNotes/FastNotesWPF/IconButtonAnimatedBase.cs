using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace FastNotes
{
    public class IconButtonAnimatedBase : Button
    {
        #region Private Members
        #endregion
        #region Public Members

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.RegisterAttached("NormalColor",
            typeof(Color), typeof(IconButtonAnimatedBase), new FrameworkPropertyMetadata(Color.FromRgb(255, 255, 255)));


        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.RegisterAttached("HoverColor",
          typeof(Color), typeof(IconButtonAnimatedBase), new FrameworkPropertyMetadata(Color.FromRgb(255, 255, 255)));


        public static readonly DependencyProperty PressedColorProperty = DependencyProperty.RegisterAttached("PressedColor",
          typeof(Color), typeof(IconButtonAnimatedBase), new FrameworkPropertyMetadata(Color.FromRgb(255, 255, 255)));


        public static readonly DependencyProperty NormalForeColorProperty = DependencyProperty.RegisterAttached("NormalForeColor",
typeof(Color), typeof(IconButtonAnimatedBase), new FrameworkPropertyMetadata(Color.FromRgb(0, 0, 0)));


        public static readonly DependencyProperty PressedForeColorProperty = DependencyProperty.RegisterAttached("PressedForeColor",
   typeof(Color), typeof(IconButtonAnimatedBase), new FrameworkPropertyMetadata(Color.FromRgb(0, 0, 0)));

        public static readonly DependencyProperty HoverForeColorProperty = DependencyProperty.RegisterAttached("HoverForeColor",
typeof(Color), typeof(IconButtonAnimatedBase), new FrameworkPropertyMetadata(Color.FromRgb(0, 0, 0)));



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

        #endregion



        #region Protected Methods
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Mouse.OverrideCursor = Cursors.Hand;
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Mouse.OverrideCursor = null;
        }
        #endregion

        #region Public Methods

        #endregion


    }
}
