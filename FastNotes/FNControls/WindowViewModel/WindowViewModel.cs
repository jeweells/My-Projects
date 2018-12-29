using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FastNotes
{
    class WindowViewModel : BaseViewModel
    {
        #region PrivateMembers
        /// <summary>
        /// Padding of the content region
        /// </summary>
        int _ContentPadding = 15;

        Window window;

        /// <summary>
        /// Border around the content (Top border isn't included)
        /// </summary>
        int _ContentBorderThickness = 3;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        int outerMarginSize = 10;

        /// <summary>
        /// The radius of the window's borders
        /// </summary>
        int windowRadius = 10;
        #endregion

        #region PublicMembers

        public Thickness ContentPadding { get{ return new Thickness(_ContentPadding);  } }
        public double WindowMinWidth { get; set; } = 300;
        public double WindowMinHeight { get; set; } = 100;

        public ICommand MenuCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The size of the border around the window
        /// </summary>
        public int ResizeBorderSize { get; set; } = 6;

        public Thickness ContentBorderThickness { get { return new Thickness(_ContentBorderThickness, 0, _ContentBorderThickness, _ContentBorderThickness);  } }

        /// <summary>
        /// The size of the border around the window, taking into account the outermargin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorderSize + OuterMarginSize); } }
      
        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return window.WindowState == WindowState.Maximized ? 0 : outerMarginSize;
            }
            set
            {
                outerMarginSize = value;
            }
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        /// <summary>
        /// The radius of the window's borders
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return window.WindowState == WindowState.Maximized ? 0 : windowRadius;
            }
            set
            {
                windowRadius = value;
            }
        }

        #region WindowRadius Combinations (No WindowRadius in some corners)
        /// <summary>
        /// The radius of the window's borders
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusTL { get { return new CornerRadius(WindowRadius, 0, 0, 0); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusTR { get { return new CornerRadius(0, WindowRadius, 0, 0); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusBR { get { return new CornerRadius(0, 0, WindowRadius, 0); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusBL { get { return new CornerRadius(0, 0, 0, WindowRadius); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left and top-right)
        /// </summary>
        public CornerRadius WindowCornerRadiusTLTR { get { return new CornerRadius(WindowRadius, WindowRadius, 0, 0); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left and bottom-right)
        /// </summary>
        public CornerRadius WindowCornerRadiusTLBR { get { return new CornerRadius(WindowRadius, 0, WindowRadius, 0); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left and bottom-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusTLBL { get { return new CornerRadius(WindowRadius, 0, 0, WindowRadius); } }

        /// <summary>
        /// The radius of the window's borders (Only top-right and bottom-right)
        /// </summary>
        public CornerRadius WindowCornerRadiusTRBR { get { return new CornerRadius(0, WindowRadius, WindowRadius, 0); } }
       
        /// <summary>
        /// The radius of the window's borders (Only top-right and bottom-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusTRBL { get { return new CornerRadius(0, WindowRadius, 0, WindowRadius); } }

        /// <summary>
        /// The radius of the window's borders (Only bottom-right and bottom-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusBRBL { get { return new CornerRadius(0, 0, WindowRadius, WindowRadius); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left, top-right and bottom-right)
        /// </summary>
        public CornerRadius WindowCornerRadiusTLTRBR { get { return new CornerRadius(WindowRadius, WindowRadius, WindowRadius, 0); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left, top-right and bottom-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusTLTRBL { get { return new CornerRadius(WindowRadius, WindowRadius, 0, WindowRadius); } }

        /// <summary>
        /// The radius of the window's borders (Only top-left, bottom-right and bottom-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusTLBRBL { get { return new CornerRadius(WindowRadius, 0, WindowRadius, WindowRadius); } }

        /// <summary>
        /// The radius of the window's borders (Only top-right, bottom-right and bottom-left)
        /// </summary>
        public CornerRadius WindowCornerRadiusTRBRBL { get { return new CornerRadius(0, WindowRadius, WindowRadius, WindowRadius); } }
        #endregion


        /// <summary>
        /// Height of the window title bar
        /// </summary>
        public int TitleHeight { get; set; } = 30;

        /// <summary>
        /// Height of the window title bar
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorderSize); } }

        #endregion

        #region Constructor
        public WindowViewModel(Window window)
        {
            this.window = window;
            this.window.StateChanged += (sender, e) =>
            {
                WindowResized();
            };
            MinimizeCommand = new RelayCommand(() => window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => window.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(window, window.PointToScreen(Mouse.GetPosition(window))));

            // Fix when maximizing the window
            var resizer = new WindowResizer(window);
        }
        #endregion

        #region Private Helpers
        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            //OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }
        #endregion

    }
}
