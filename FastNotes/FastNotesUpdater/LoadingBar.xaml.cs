using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FastNotesUpdater
{
    /// <summary>
    /// Lógica de interacción para LoadingBar.xaml
    /// </summary>
    public partial class LoadingBar : UserControl
    {
        #region Private Members
        double _Progress = 0.5;
        #endregion

        #region Public Members
        public double Progress { get { return _Progress; }
            set
            {
                _Progress = value;
                // Actual width is used because the width of the progresscontainer isn't set, and we want it to have the size it needs
                ProgressIndicator.Width = ProgressContainer.ActualWidth * _Progress;
            }
        }
        #endregion

        #region Constructor
        public LoadingBar()
        {
            InitializeComponent();
        }

        #endregion



    }
}
