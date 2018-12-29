using ColorManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Controls
{
    /// <summary>
    /// Lógica de interacción para ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        #region Private Members
        private Controls.Models.SelectedColorInfo _SelectedColorInfo;
        private WindowViewModel BaseModel;

        // These 4 variables help performance, not more
        private bool RGBComponentsChanged;
        private bool HueDotChanged;
        private bool SVDotChanged;
        private bool HexTextChanged;
        /// <summary>
        /// Indicates if the event was fired without the user's help
        /// </summary>
        private bool computedChange; 

        #endregion

        #region Public Members
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern IntPtr GetActiveWindow();

        public Controls.Models.SelectedColorInfo SelectedColorInfo
        {
            get {
                if (_SelectedColorInfo == null)
                    _SelectedColorInfo = (Controls.Models.SelectedColorInfo) Resources["SelectedColorInfo"];
                return _SelectedColorInfo;
            }
        }
        
        #endregion

        #region Constructor

        public ColorPicker()
        {
            computedChange = true;
            var wih = new WindowInteropHelper(this);
            wih.Owner = GetActiveWindow();
            InitializeComponent();
            this.DataContext = BaseModel = new WindowViewModel(this);
            Loaded += (a,b) => {
                PaintHuePreviewer();
                PaintHueDot();
                PaintSVViewer();
                PaintColorPreviewer();
                MoveHueDot();
                MoveSVDot();
                computedChange = false;
                SelectedColorInfo.ColorChanged += (x) => {
                    computedChange = true;
                    if (RGBComponentsChanged)
                    {
                        ModifyHexTextBlock();
                        MoveSVDot();
                        MoveHueDot();
                        PaintSVViewer();
                        PaintHueDot();
                        PaintColorPreviewer();
                    }
                    else if (HexTextChanged)
                    {
                        MoveHueDot();
                        MoveSVDot();
                        PaintSVViewer();
                        PaintHueDot();
                        PaintColorPreviewer();
                        ModifyRGBComponents();
                    }
                    else if (HueDotChanged)
                    {
                        ModifyHexTextBlock();
                        ModifyRGBComponents();
                        PaintSVViewer();
                        PaintColorPreviewer();
                        PaintHueDot();

                    }
                    else if (SVDotChanged)
                    {
                        ModifyHexTextBlock();
                        ModifyRGBComponents();
                        PaintColorPreviewer();
                    }
                    else
                    { // Directly changed from source
                        ModifyHexTextBlock();
                        ModifyRGBComponents();
                        MoveSVDot();
                        MoveHueDot();
                        PaintSVViewer();
                        PaintHueDot();
                        PaintColorPreviewer();
                    }
                    computedChange = false;
                };

            }; // Paint the hue previewer

        }

        #endregion



        #region Private Methods

        private void OnSelectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = true;
            }
            catch { }
        }
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = false;
            }
            catch { }
        }

        /// <summary>
        /// Changes the hex mode to rgb mode and vice versa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwapInputMode(object sender, RoutedEventArgs e)
        {
            HexColorPanel.Visibility ^= Visibility.Collapsed;
            RGBColorPanel.Visibility ^= Visibility.Collapsed;
        }

        /// <summary>
        /// Updates the current color when the hex value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HexTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (computedChange) return;
            if (RGBComponentsChanged) return;

            // We receive a string
            string val = HexTextBlock.Text;
            val = val.TrimStart('#'); // Take out the #
            try
            {
                int r = System.Convert.ToInt32(val.Substring(0, 2), 16); // Get the number from hex to decimal
                int g = System.Convert.ToInt32(val.Substring(2, 2), 16); // Get the number from hex to decimal
                int b = System.Convert.ToInt32(val.Substring(4, 2), 16); // Get the number from hex to decimal

                var rgb = new sRGB(r / 255f, g / 255f, b / 255f);
                HexTextChanged = true;
                SelectedColorInfo.SelectedColorRGB = rgb;
                HexTextChanged = false;
            }
            catch
            {

            }
        }


        /// <summary>
        /// Updates the current color when the rgb components change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RGBComponents_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (computedChange) return;
            if (HexTextChanged) return;
            try
            {
                byte r = byte.Parse(RComponent.Text);
                byte g = byte.Parse(GComponent.Text);
                byte b = byte.Parse(BComponent.Text);
                RGBComponentsChanged = true;
                SelectedColorInfo.SelectedColorRGB = new sRGB(r / 255f, g / 255f, b / 255f);
                RGBComponentsChanged = false;
            }
            catch
            {

            }
        }




        /// <summary>
        /// Updates the current color when the hue dot is moved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (computedChange) return;
            HueDotChanged = true;
            SelectedColorInfo.SelectedColorHSV = new HSV((int)HueSlider.Value, SelectedColorInfo.SelectedColorHSV.S, SelectedColorInfo.SelectedColorHSV.V);
            HueDotChanged = false;
        }

        /// <summary>
        /// Moves the SV Dot following the mouse position and changing the color value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SVDot_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Mouse.OverrideCursor = Cursors.Hand;
                var pos = Mouse.GetPosition(SVViewer);
                var viewerRelpos = e.GetPosition(SVViewer);
                double top = 0, left = 0;
                if (viewerRelpos.Y > 0)
                {
                    if (viewerRelpos.Y < SVViewer.ActualHeight)
                    {
                        top = viewerRelpos.Y;
                    }
                    else top = SVViewer.ActualHeight;
                }


                if (viewerRelpos.X > 0)
                {
                    if (viewerRelpos.X < SVViewer.ActualWidth)
                    {
                        left = viewerRelpos.X;
                    }
                    else left = SVViewer.ActualWidth;
                }
                HSV newColor = new HSV(SelectedColorInfo.SelectedColorHSV.H,
                    (float)(left / SVViewer.ActualWidth),  // X is the saturation -> 0 left 1 right
                    (float)(1 - top / SVViewer.ActualHeight)); // Y is the value -> 0 for bottom and 1 is top
                SVDotChanged = true;
                SelectedColorInfo.SelectedColorHSV = newColor;
                SVDotChanged = false;
                top -= SVDot.Height / 2;
                left -= SVDot.Width / 2;
                Canvas.SetTop(SVDot, top);
                Canvas.SetLeft(SVDot, left);

            }
            else if (e.LeftButton == MouseButtonState.Released)
            {
                Mouse.OverrideCursor = null;

            }
        }

        /// <summary>
        /// Restores the cursor to default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SVViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        /// <summary>
        /// Helps to capture the sv dot by the mouse so that it can be moved even if the mouse is outside the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SVViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SVDot.CaptureMouse();
        }

        /// <summary>
        /// Helps to release the capture of the sv dot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SVViewer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        /// <summary>
        /// Increments/Reduce the value of the RGB components when the mouse wheel value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RGBComponent_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int increment = (int)(e.Delta / 120.0);
            var rgbComponent = (TextBox)sender;
            byte currentVal;
            try
            {
                currentVal = byte.Parse(rgbComponent.Text);
                if ((currentVal == 0 && increment < 0) || (currentVal == 255 && increment > 0)) return;
            }
            catch
            {
                currentVal = (byte)(increment > 0 ? 0 : 255);
            }
            if (currentVal + increment < 0) currentVal = 0;
            else if (currentVal + increment > 255) currentVal = 255;
            else currentVal = (byte)(currentVal + increment);
            rgbComponent.Text = currentVal + "";

        }


        /// <summary>
        /// Selects the hex part of the hex text box when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HexTextBlock_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (HexTextBlock.Text[0] == '#')
            {
                HexTextBlock.Select(1, HexTextBlock.Text.Length - 1);
            }
            else
            {
                HexTextBlock.Select(0, HexTextBlock.Text.Length);
            }
        }

        /// <summary>
        /// Selects the RGB components when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RGBComponent_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var obj = (TextBox)sender;
            obj.SelectAll();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Paints the linear gradient square
        /// </summary>
        protected void PaintSVViewer()
        {
            float widthf = (float)(SVViewer.ActualWidth == 0? SVViewer.Width : SVViewer.ActualWidth);
            float heightf = (float)(SVViewer.ActualHeight == 0? SVViewer.Height : SVViewer.ActualHeight);
            int width = (int)widthf;
            int height = (int)heightf;
            int bytesperpixel = 4;
            int stride = width * bytesperpixel;
            byte[] imgdata = new byte[width * height * bytesperpixel];
            var loopColor = new ColorManager.HSV(SelectedColorInfo.SelectedColorHSV.H, 0, 0);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    loopColor.V = 1 - row / heightf; // This might be seen as the y axis
                    loopColor.S = col / widthf; // This might be seen as the x axis
                    var currRgb = new ColorManager.RGB(loopColor);
                    int pos = row * stride + col * 4; // Position we're right now on the array
                                                      // BGRA
                    imgdata[pos + 0] = (currRgb.B);
                    imgdata[pos + 1] = (currRgb.G);
                    imgdata[pos + 2] = (currRgb.R);
                    imgdata[pos + 3] = (255);
                }
            }
            SVViewer.Background = new ImageBrush(BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, imgdata, stride));
        }

        /// <summary>
        /// Paints the circle that shows the current color selected
        /// </summary>
        protected void PaintColorPreviewer()
        {
            byte r = (byte)(SelectedColorInfo.SelectedColorRGB.R * 255f);
            byte g = (byte)(SelectedColorInfo.SelectedColorRGB.G * 255f);
            byte b = (byte)(SelectedColorInfo.SelectedColorRGB.B * 255f);
            CurrentColorEllipse.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        /// <summary>
        /// Moves the hue dot according to the selected color's hue
        /// </summary>
        protected void MoveHueDot()
        {
            //Controls.MessageBox.Log(null, currentColorHue + " "+maxLeftMargin, Controls.MessageBox.Ok);
            HueSlider.Value = SelectedColorInfo.SelectedColorHSV.H;
        }


        /// <summary>
        /// Paints the hue dot using the selected hue, also with saturation and value = 1
        /// </summary>
        protected void PaintHueDot()
        {
            var rgb = new sRGB(new HSV(SelectedColorInfo.SelectedColorHSV.H, 1, 1));
            byte r = (byte)(rgb.R * 255.0);
            byte g = (byte)(rgb.G * 255.0);
            byte b = (byte)(rgb.B * 255.0);
            HueSlider.Foreground = new SolidColorBrush(Color.FromRgb(r,g,b));
        }

        /// <summary>
        /// Modifies the R G and B components to values between 0 and 255
        /// </summary>
        protected void ModifyRGBComponents()
        {
            byte r = (byte)(SelectedColorInfo.SelectedColorRGB.R * 255.0);
            byte g = (byte)(SelectedColorInfo.SelectedColorRGB.G * 255.0);
            byte b = (byte)(SelectedColorInfo.SelectedColorRGB.B * 255.0);
            RComponent.Text = r.ToString();
            GComponent.Text = g.ToString();
            BComponent.Text = b.ToString();
        }

        /// <summary>
        /// Modifies the hex text block to the #FFFFFF form
        /// </summary>
        protected void ModifyHexTextBlock()
        {
            var rgb = SelectedColorInfo.SelectedColorRGB;
            HexTextBlock.Text = "#" + (((int)(rgb.R * 255.0) << 16) | ((int)(rgb.G * 255.0) << 8) | ((int)(rgb.B * 255.0))).ToString("X6");
        }

        /// <summary>
        /// Moves the saturation-value dot according to the selected saturation-value already stored in the selected color
        /// </summary>
        protected void MoveSVDot()
        {
            double left = SelectedColorInfo.SelectedColorHSV.S * SVViewer.ActualWidth;
            double top = (1 - SelectedColorInfo.SelectedColorHSV.V) * SVViewer.ActualHeight;
            Canvas.SetTop(SVDot, top - SVDot.Height/2);
            Canvas.SetLeft(SVDot, left - SVDot.Width/2);
        }

        /// <summary>
        /// Paints the hue rectagle with saturation = 1 and value = 1 (This should be called once)
        /// </summary>
        protected void PaintHuePreviewer()
        {
            float widthf = (float)(HuePreviewer.ActualWidth == 0 ? HuePreviewer.Width : HuePreviewer.ActualWidth);
            float heightf = (float)(HuePreviewer.ActualHeight == 0 ? HuePreviewer.Height : HuePreviewer.ActualHeight);
            int width = (int)widthf;
            int height = (int)heightf;
            int bytesperpixel = 4;
            int stride = width * bytesperpixel;
            byte[] imgdata = new byte[width * height * bytesperpixel];
            var loopColor = new ColorManager.HSV(0, 1, 1);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    loopColor.H = (int)((col / (double)width) * 360.0);
                    var currRgb = new ColorManager.RGB(loopColor);
                    int pos = row * stride + col * 4; // Position we're right now on the array
                                                      // BGRA
                    imgdata[pos + 0] = (currRgb.B);
                    imgdata[pos + 1] = (currRgb.G);
                    imgdata[pos + 2] = (currRgb.R);
                    imgdata[pos + 3] = (255);
                }
            }
            HuePreviewer.Background = new ImageBrush(BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, imgdata, stride));
        }

        #endregion


        #region Public Methods
        /// <summary>
        /// Sets the color when the window has loaded
        /// </summary>
        /// <param name="sRGB"></param>
        public void SetStartColor(sRGB sRGB)
        {
            Loaded += (a, b) => SelectedColorInfo.SelectedColorRGB = sRGB;
        }
        #endregion

    }
}
