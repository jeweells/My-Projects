using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FastNotes
{
    public class XamlIconBase : Viewbox
    {
        #region Public Members

        public static readonly DependencyProperty DataProperty = 
            DependencyProperty.RegisterAttached("Data", typeof(Geometry), typeof(XamlIconBase), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty Data2Property = 
            DependencyProperty.RegisterAttached("Data2", typeof(Geometry), typeof(XamlIconBase), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty DesiredHeightProperty =
            DependencyProperty.RegisterAttached("DesiredHeight", typeof(double), typeof(XamlIconBase), new UIPropertyMetadata(null));

        public static readonly DependencyProperty DesiredWidthProperty =
            DependencyProperty.RegisterAttached("DesiredWidth", typeof(double), typeof(XamlIconBase), new UIPropertyMetadata(null));

        public static readonly DependencyProperty DataHeightProperty =
            DependencyProperty.RegisterAttached("DataHeight", typeof(double), typeof(XamlIconBase), new UIPropertyMetadata(null));

        public static readonly DependencyProperty DataWidthProperty =
            DependencyProperty.RegisterAttached("DataWidth", typeof(double), typeof(XamlIconBase), new UIPropertyMetadata(null));

        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.RegisterAttached("FillColor", typeof(Brush), typeof(XamlIconBase), new UIPropertyMetadata(null));


        public double DesiredHeight { get; set; } = 16f;
        public double DesiredWidth { get; set; } = 16f;
        public double DataWidth { get; set; } = 24f;
        public double DataHeight { get; set; } = 24f;
        public Geometry Data { get; set; }
        public Geometry Data2 { get; set; }
        public Brush FillColor { get; set; }

        #endregion

    }
}
