using ColorManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FastNotes
{
    /// <summary>
    /// Lógica de interacción para NoteWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window, INotifyPropertyChanged
    {

        #region Private Members

        /// <summary>
        /// Reference to the node in the group of notes declared in Program.availableForms
        /// </summary>
        LinkedListNode<Window> thisNode;

        /// <summary>
        /// Current data
        /// </summary>
        NoteData_1_0_1_4 ndata = null;

        /// <summary>
        ///  Let outside classes know when this note hasn't saved its data
        /// </summary>
        bool changed = false;

        WindowViewModel BaseModel;

        NoteWindowBaseModel DataContainer;

      
        private ScrollViewerScalable _NoteScrollContainer = null;
        #endregion

        #region Protected Members

        #endregion

        #region Public Members
        public bool DataHandled { get; set; } = false;

        public bool HasData { get; private set; } = false;

        /// <summary>
        /// This object is part of the rich text box, this has zoom modifiers to work with
        /// </summary>
        public ScrollViewerScalable NoteScrollContainer
        {
            get
            {
                if (_NoteScrollContainer == null) _NoteScrollContainer = (ScrollViewerScalable)textBox.Template.FindName("PART_ContentHost", textBox);
                return _NoteScrollContainer;
            }
        }

        /// <summary>
        /// Happens when the note is created just after it is painted randomly
        /// </summary>
        public event Action OnAfterRandomlyPainted;

        /// <summary>
        /// Happens when the data is created for the first time (when ndata is null and stops being null)
        /// </summary>
        public event Action OnAfterDataCreated;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Contrast ratio between the text in the background and the background
        /// </summary>
        public float TextContrastRatio { set; get; } = ColorManager.ColorManager.CRLevelAA; // 4.5

        /// <summary>
        /// Contrast ratio between the icons in the top bar and the top bar
        /// </summary>
        public float IconContrastRatio { set; get; } = ColorManager.ColorManager.CRLevelAALargeText; // 3

        /// <summary>
        /// Contrast ratio between the top bar and the background
        /// </summary>
        public float TopBarContrastRatio { set; get; } = 1.2f; // 3

        /// <summary>
        /// Returns whether this note is pinned or not
        /// </summary>
        public bool IsPinned
        {
            get { return Topmost; }
        }

        /// <summary>
        /// Leaves a mark that tells this note data hasn't been saved and also notifies the father there's something to be saved
        /// </summary>
        public bool Changed
        {
            get { return changed; }
            set
            {
                changed = value;
                if (changed)
                    MainWindow.DataSaved = false;
            }
        }

        /// <summary>
        /// The note data including position, text, color, etc.
        /// If not initialized, it intializes itself when calling it
        /// </summary>
		public NoteData_1_0_1_4 Data
        {
            get
            {
                if (!changed && ndata != null) return ndata;
                bool dataCreated = false;
                if (ndata == null)
                {
                    ndata = new NoteData_1_0_1_4();
                    dataCreated = true;
                }
                var tmpMainColor = DataContainer.MainBrush as SolidColorBrush;
                ndata.MainColor = System.Drawing.Color.FromArgb(tmpMainColor.Color.R, tmpMainColor.Color.G, tmpMainColor.Color.B);
                ndata.NoteWidth = Width;
                ndata.NoteHeight = Height;
                ndata.TopPosition = Top;
                ndata.LeftPosition = Left;
                var dataRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
                using (System.IO.MemoryStream innerData = new System.IO.MemoryStream())
                {
                    dataRange.Save(innerData, DataFormats.Rtf);
                    ndata.RtfData = innerData.ToArray();
                }
                ndata.ZoomFactor = DataContainer.ZoomFactor;
                ndata.IsPinned = IsPinned;
                if (dataCreated) OnAfterDataCreated?.Invoke(); // The data was just created
                return ndata;
            }
            set
            {
                ndata = value;
                if (value != null)
                {
                    Top = value.TopPosition;
                    Left = value.LeftPosition;
                    Width = value.NoteWidth;
                    Height = value.NoteHeight;

                    var dataRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
                    using (System.IO.MemoryStream innerData = new System.IO.MemoryStream(value.RtfData))
                    {
                        dataRange.Load(innerData, DataFormats.Rtf);
                    }
                    DataContainer.ZoomFactor = value.ZoomFactor;
                    if (value.IsPinned) PinNote();
                    PaintNote(new sRGB(value.MainColor.R / 255f, value.MainColor.G / 255f, value.MainColor.B/255f));
                    changed = false;
                }
            }
        }

        /// <summary>
        /// When closing all notes if there are no notes and this is true, the application will be requested to exit
        /// </summary>
        public bool CloseWithTheLastNoteOpened { get; set; } = true;

        /// <summary>
        /// When this variable is set to true, the confirmation to delete the note will be ignored and assumed as "yes"
        /// </summary>
        public bool CloseDialog { get; set; } = true;

        /// <summary>
        /// Do not perform any operations about data when this note is closing
        /// </summary>
        public bool ForgetDataWhenClosing { get; set; } = false;

        #endregion

        #region Constructor

        public NoteWindow(LinkedListNode<Window> thisNode)
        {
            this.thisNode = thisNode; // Save a reference of the node in the current notes list
            InitializeComponent();
            this.DataContext = new WindowViewModel(this);
            BaseModel = (WindowViewModel)DataContext;
            DataContainer = Resources["DataContainer"] as NoteWindowBaseModel;
            BaseModel.WindowRadius = 0;
            BaseModel.WindowMinHeight = 150;
            BaseModel.WindowMinWidth = 200;
            SetEvents();
            bool isANewNote = false;
            // Make buttons' borders transparent
            Loaded += (x, y) => {
                if (!DataHandled && ndata == null)
                { // Painting randomly the note (Only when it hasn't loaded the data which means it has already been painted)
                    Random r = new Random((int)DateTime.Now.Ticks);

                    PaintNote(
                        new sRGB(
                            new HSL(
                                r.Next(0, 256),
                                (float)r.NextDouble(), 
                                0.25f + (float)r.NextDouble() % 0.45f)));
                    // We want to generate colors not too dark (some colors such as almost black doesn't look very good)
                    textBox.AppendText("");
                    var textRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
                    textRange.ApplyPropertyValue(Control.FontFamilyProperty, new System.Windows.Media.FontFamily("Arial"));
                    var ms = new System.IO.MemoryStream();
                    textRange.Save(ms, DataFormats.Rtf);
                    textRange.Load(ms, DataFormats.Rtf); // For some reason this fixes the line height of the richtextbox

                    isANewNote = true;
                    OnAfterRandomlyPainted?.Invoke();
                }
            };
            ContentRendered += (a, b) => {
                if(isANewNote) HasData = false;
            }; // Avoids asking for closing this note if the note has nothing

        }

        #endregion

        #region Private Methods

        private void SetEvents()
        {
            Closing += new System.ComponentModel.CancelEventHandler(OnNoteFormClosing);
            LocationChanged += new EventHandler(OnLocationChanged);
            textBox.TextChanged += new TextChangedEventHandler(OnNoteContentChanged);
            DataContainer.ZoomFactorChanged += () =>
            {
                Changed = true;
            };
        }

        /// <summary>
        /// Implements when it's wanted to change the color of a note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColorPickerBtnClick(object sender, RoutedEventArgs e)
        {
            var tmpMainColor = DataContainer.MainBrush as SolidColorBrush;
            var previousColor = DataContainer.MainColor;
            var colorPicker = new Controls.ColorPicker();
            colorPicker.SelectedColorInfo.ColorChanged+= (n) => PaintNote(n);
            colorPicker.SetStartColor(new sRGB(tmpMainColor.Color.R/255f, tmpMainColor.Color.G/255f, tmpMainColor.Color.B/255f));

            if (colorPicker.ShowDialog() ?? false)
            {
                Changed = true;
            }
            else
            {
                PaintNote(new sRGB(previousColor.R/255f, previousColor.G/255f, previousColor.B/255f));
            }
        }

        /// <summary>
        /// Implements when a note is created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateNoteBtnClick(object sender, RoutedEventArgs e)
        {
            var noteform = (NoteWindow)Program.CreateNote();
            noteform.OnAfterRandomlyPainted += () =>
            {
                noteform.Width = this.Width;
                noteform.Height = this.Height;
                noteform.textBox.AppendText("");
                var fp = new Controls.Models.FontSelectedInfo();
                fp.InheritFrom(this.textBox.Document.ContentStart, this.textBox.Document.ContentEnd);
                fp.ApplyOn(noteform.textBox.Document.ContentStart, noteform.textBox.Document.ContentEnd);

                //var newTr = new TextRange(noteform.textBox.Document.ContentStart, noteform.textBox.Document.ContentEnd);
                //var fatherTr = new TextRange(this.textBox.Document.ContentStart, this.textBox.Document.ContentEnd);
                //var fatherFontFamilyProp = fatherTr.GetPropertyValue(FontFamilyProperty);
                //var fatherFontSizeProp = fatherTr.GetPropertyValue(FontSizeProperty);
                //var fatherFontWeightProp = fatherTr.GetPropertyValue(FontWeightProperty);
                //var fatherFontStyleProp = fatherTr.GetPropertyValue(FontStyleProperty);
                //var fatherTextDecorationsProp = fatherTr.GetPropertyValue(Inline.TextDecorationsProperty);


                //newTr.ApplyPropertyValue(FontFamilyProperty, fatherFontFamilyProp == DependencyProperty.UnsetValue ? "Arial" : fatherFontFamilyProp);
                //newTr.ApplyPropertyValue(FontSizeProperty, fatherFontSizeProp == DependencyProperty.UnsetValue ? 14 : fatherFontSizeProp);

                //if (fatherFontWeightProp != DependencyProperty.UnsetValue)
                //    newTr.ApplyPropertyValue(FontWeightProperty, fatherFontWeightProp);

                //if (fatherFontStyleProp != DependencyProperty.UnsetValue)
                //    newTr.ApplyPropertyValue(FontStyleProperty, fatherFontStyleProp);

                //if (fatherTextDecorationsProp != DependencyProperty.UnsetValue)
                //    newTr.ApplyPropertyValue(Inline.TextDecorationsProperty, fatherTextDecorationsProp);

                noteform.DataContainer.ZoomFactor = DataContainer.ZoomFactor;

                if (this.IsPinned) noteform.PinNote();
            };
            noteform.Owner = Program.MainWindow;
            noteform.Show();

            Changed = true;
        }

        /// <summary>
        /// Happens when touching the note topbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLocationChanged(object sender, EventArgs e)
        {
            Changed = true; // Data has changed
        }

        /// <summary>
        /// Clicking the discard note button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void OnDiscardBtnClick(object sender, RoutedEventArgs e)
        {
            Close();

            //if (textBox.Text == "" || DialogResult.Yes == MessageBox.Show("Are you sure you want to delete this note?", "Deleting note", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //{
            //	Program.availableForms.Remove(thisNode);
            //	StartupForm.dataSaved = false; // Let father know there are changes
            //	if (Program.availableForms.Count == 0)
            //	{
            //		Application.Exit();
            //	}
            //	else
            //	{
            //		Close();
            //	}
            //}
        }


        /// <summary>
        /// Happens when the text of the note changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNoteContentChanged(object sender, TextChangedEventArgs e)
        {
            Changed = true;
            HasData = true;
            Debug.WriteLine("Changed");
        }

        /// <summary>
        /// Happens when the pin button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPinBtnClick(object sender, RoutedEventArgs e)
        {
            if (IsPinned)
            {
                UnpinNote();
            }
            else
            {
                PinNote();
            }
            Changed = true;
        }

        private void ApplyFontStylesToText(Controls.FontPicker fontPicker)
        {
            if (textBox.Selection.Start == textBox.Selection.End)
                fontPicker.SelectedInfo.ApplyOn(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            else
                fontPicker.SelectedInfo.ApplyOn(textBox.Selection.Start, textBox.Selection.End);
        }

        /// <summary>
        /// Happens when the font button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFontBtnClick(object sender, RoutedEventArgs e)
        {
            var fontPicker = new Controls.FontPicker();
            if(textBox.Selection.Start == textBox.Selection.End)
                fontPicker.SelectedInfo.InheritFrom(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            else
                fontPicker.SelectedInfo.InheritFrom(textBox.Selection.Start, textBox.Selection.End);
            fontPicker.SelectedInfo.FontFeatureChanged += () => {
                ApplyFontStylesToText(fontPicker);
            };
            var currMs = new System.IO.MemoryStream();
            // Lets copy our previous state before change it
            var currTr = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            currTr.Save(currMs, DataFormats.Rtf);
            if(!(fontPicker.ShowDialog() ?? false)) // If the font picking was cancelled
            {
                currTr = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
                currTr.Load(currMs, DataFormats.Rtf); // Restore the previous state
            }
            else
            { // Font styles applied
                Changed = true;
            }
            //System.Windows.Media.FontFamily selectedFontFamily;
            //if(fontPicker == null)
            //{
            //    fontPicker = new System.Windows.Forms.FontDialog();
            //}
            //try
            //{

            //    FontStyleProperty.
            //    selectedFontFamily = (System.Windows.Media.FontFamily) textBox.Selection.GetPropertyValue(FontFamilyProperty);
            //    var FontSize = (double) textBox.Selection.GetPropertyValue(FontSizeProperty);
            //    var FontWeight = (FontStyles)textBox.Selection.GetPropertyValue(FontWeightProperty);
            //    FontWeight.
            //    fontPicker.Font = new System.Drawing.Font(selectedFontFamily.FamilyNames.Values.ElementAt(0), (float)FontSize);
            //}
            //catch {
            //    selectedFontFamily = null;
            //}


            //if (System.Windows.Forms.DialogResult.OK == fontPicker.ShowDialog(GetIWin32Window(this)))
            //{
            //    TextRange tr = (textBox.Selection.Start == textBox.Selection.End) ?
            //       new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd)  // Modify Everything
            //       :
            //        textBox.Selection; // Just what is selected

            //    tr.ApplyPropertyValue(FontFamilyProperty, new System.Windows.Media.FontFamily(fontPicker.Font.Name));
            //    tr.ApplyPropertyValue(FontSizeProperty, (double)fontPicker.Font.Size);
            //    Changed = true;
            //}
        }


        /// <summary>
        /// Happens when someone wants to close this note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNoteFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Closing += new System.ComponentModel.CancelEventHandler(OnNoteFormClosing);
            if (ForgetDataWhenClosing) return;
            // Ask confirmation
            
            if (!MainWindow.CurrentConfig.askWhenClosingANote || !CloseDialog || !HasData || true == (Controls.MessageBox.Question("Deleting note", "Are you sure you want to delete this note?", Controls.MessageBox.YesNo) ?? false))
            {
                // If the question dialog was opened, entering here means the user pressed "Yes"
                RemoveNoteNoDialog();
            }
            else
            {
                // If the question dialog was opened, entering here means the user pressed "No"
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Removes the note as normally, it wont't ask if you really want to delete it
        /// </summary>
        void RemoveNoteNoDialog()
        {
            Program.AvailableNotes.Remove(thisNode);
            Changed = true;
            if (CloseWithTheLastNoteOpened && Program.AvailableNotes.Count == 0)
            {
                Program.MainWindow.Close();
            }
        }

        #region RightClickFunctions

        private void Copy_MenuItemClick(object sender, RoutedEventArgs e)
        {
            textBox.Copy();
        }

        private void Cut_MenuItemClick(object sender, RoutedEventArgs e)
        {
            textBox.Cut();
        }

        private void Paste_MenuItemClick(object sender, RoutedEventArgs e)
        {
            textBox.Paste();
        }

        private void Delete_MenuItemClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SendKeys.SendWait("{DELETE}");
        }

        private void Undo_MenuItemClick(object sender, RoutedEventArgs e)
        {
            textBox.Undo();
        }

        private void Redo_MenuItemClick(object sender, RoutedEventArgs e)
        {
            textBox.Redo();
        }

        private void ZoomIn_MenuItemClick(object sender, RoutedEventArgs e)
        {
            NoteScrollContainer.ZoomFactor += .25;
        }

        private void ZoomOut_MenuItemClick(object sender, RoutedEventArgs e)
        {
            NoteScrollContainer.ZoomFactor -= .25;
        }
       
        private void ResetZoom_MenuItemClick(object sender, RoutedEventArgs e)
        {
            NoteScrollContainer.ZoomFactor = 1;
        }

        private void SelectAll_MenuItemClick(object sender, RoutedEventArgs e)
        {
            textBox.SelectAll();
        }

        private void AlignLeft_MenuItemClick(object sender, RoutedEventArgs e)
        {
            AlignBlocks(TextAlignment.Left);
        }

        private void AlignCenter_MenuItemClick(object sender, RoutedEventArgs e)
        {
            AlignBlocks(TextAlignment.Center);

        }

        private void AlignRight_MenuItemClick(object sender, RoutedEventArgs e)
        {
            AlignBlocks(TextAlignment.Right);

        }

        private void Justify_MenuItemClick(object sender, RoutedEventArgs e)
        {
            AlignBlocks(TextAlignment.Justify);
        }

        private void AlignBlocks(TextAlignment way)
        {
            if (textBox.Selection.Start == textBox.Selection.End)
            {
                var curCaret = textBox.CaretPosition;
                var curBlock = textBox.Document.Blocks.Where(x => x.ContentStart.CompareTo(curCaret) == -1 && x.ContentEnd.CompareTo(curCaret) == 1).FirstOrDefault();
                if (curBlock != null) curBlock.TextAlignment = way;
            }
            else
            {
                // Find the selected blocks
                var blocks = textBox.Document.Blocks.Where(block =>
                    block.ContentEnd.CompareTo(textBox.Selection.Start) > 0 && block.ContentStart.CompareTo(textBox.Selection.End) < 0);
                foreach (var block in blocks)
                {
                    block.TextAlignment = way;
                }
            }
        }
        

        /// <summary>
        /// Use this to translate this wpf owner to windows forms owner
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        static System.Windows.Forms.IWin32Window GetIWin32Window(System.Windows.Media.Visual visual)
        {
            var source = System.Windows.PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
            System.Windows.Forms.IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        /// <summary>
        /// This class helps to access dialogs from windows forms
        /// </summary>
        private class OldWindow : System.Windows.Forms.IWin32Window
        {
            private readonly System.IntPtr _handle;
            public OldWindow(System.IntPtr handle)
            {
                _handle = handle;
            }

            #region IWin32Window Members
            System.IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get { return _handle; }
            }
            #endregion
        }

        #endregion


        #endregion

        #region Protected Methods

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            try
            {
                var border = ((Border)Template.FindName("DataContainerControl", this));
                if (WindowState == WindowState.Maximized) border.Padding = new Thickness(0);
                else border.Padding = new Thickness(10);
            }
            catch
            {

            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (Program.MainWindow.IsCurrentDesktopClosing) return;
            if(Program.AvailableNotes != null && Program.AvailableNotes.First != thisNode)
            {
                Program.AvailableNotes.Remove(thisNode);
                Program.AvailableNotes.AddFirst(thisNode);
                Changed = true;
            }
        }

        /// <summary>
        /// Notifies when a binding variable has changed
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Paints this note to a specific color
        /// </summary>
        /// <param name="color"></param>
        public void PaintNote(sRGB color)
        {

            sRGB backgroundColor = color;
            sRGB textColor = backgroundColor.ChooseColorByContrastRatio(TextContrastRatio);

            // Dark Colors
            sRGB topBarColor = backgroundColor.ChooseColorByContrastRatio(TopBarContrastRatio);
            sRGB iconColor = textColor;

            // Light brushes
            DataContainer.MainBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)(color.R * 255.0), (byte)(color.G * 255.0), (byte)(color.B * 255.0)));
            DataContainer.TextBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) (textColor.R * 255), (byte) (textColor.G * 255), (byte) (textColor.B * 255)));

            // Dark brushes
            DataContainer.DarkMainBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)(topBarColor.R * 255), (byte)(topBarColor.G * 255), (byte)(topBarColor.B * 255)));
            DataContainer.DarkTextBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)(iconColor.R * 255), (byte)(iconColor.G * 255), (byte)(iconColor.B * 255)));

            var tr = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            tr.ApplyPropertyValue(ForegroundProperty, DataContainer.TextBrush);
        }

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Unpins the note and changes its button image
        /// </summary>
		public void UnpinNote()
        {
            Topmost = false;
            DataContainer.PinVisibility = Visibility.Visible;
            Owner = Program.MainWindow;
            SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
        }

        /// <summary>
        /// Pins the note and changes its button image
        /// </summary>
		public void PinNote()
        {
            Topmost = true;
            DataContainer.UnpinVisibility = Visibility.Visible;
            Owner = null;
        }

        #endregion

        private void Hyperlink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Yei!");
        }
    }
}
