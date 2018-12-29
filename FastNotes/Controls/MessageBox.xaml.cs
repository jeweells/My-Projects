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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MessageBox : Window
    {

        #region Private Members
        WindowViewModel BaseModel;

        #endregion

        #region Public Members

        public static readonly Dictionary<string, ButtonType> YesNo = new Dictionary<string, ButtonType>() { { "Yes", ButtonType.Accept }, { "No", ButtonType.Cancel } };
        public static readonly Dictionary<string, ButtonType> AcceptCancel = new Dictionary<string, ButtonType>() { { "Accept", ButtonType.Accept }, { "Cancel", ButtonType.Cancel } };
        public static readonly Dictionary<string, ButtonType> Accept = new Dictionary<string, ButtonType>() { { "Accept", ButtonType.Accept } };
        public static readonly Dictionary<string, ButtonType> Ok = new Dictionary<string, ButtonType>() { { "Ok", ButtonType.Accept } };
        public static readonly Dictionary<string, ButtonType> Cancel = new Dictionary<string, ButtonType>() { { "Cancel", ButtonType.Cancel } };
         
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern IntPtr GetActiveWindow();

        public enum MessageType
        {
            None, Warning, Log, Error, Question, Success
        }

        public enum ButtonType
        {
            None, Accept, Cancel
        }
        #endregion

        public MessageBox()
        {
            var wih = new WindowInteropHelper(this);
            wih.Owner = GetActiveWindow();
            InitializeComponent();
            this.DataContext = BaseModel = new WindowViewModel(this);
            //SelectedInfo =  TryFindResource("FontSelectedInfo") as FontDialog.Models.FontSelectedInfo;
            //SelectedInfo.Bold = true;

        }

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #endregion


        #region Public Methods
        /// <summary>
        /// Shows a message box blocking the main window, waiting for user's response
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Log')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="type">According to this type the icon will be different</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Show(Window owner, string title, string message, MessageType type, Dictionary<string, ButtonType> buttons)
        {
            var currentMsb = new Controls.MessageBox();
            if (owner != null) currentMsb.Owner = owner;
            return currentMsb.ShowFromInstance(title, message, type, buttons);
        }



        /// <summary>
        /// Shows a message box blocking the main window, waiting for user's response
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Log')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="type">According to this type the icon will be different</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public bool? ShowFromInstance(string title, string message, MessageType type, Dictionary<string, ButtonType> buttons)
        {
            Build(null, title, message, type, buttons);
            return this.ShowDialog();
        }

        /// <summary>
        /// Shows a message box with the log icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Log')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Log(string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(null, title, message, MessageType.Log, buttons);
        }
        /// <summary>
        /// Shows a message box with the log icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Log')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Log(Window owner, string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(owner, title, message, MessageType.Log, buttons);
        }

        /// <summary>
        /// Shows a message box with the error icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Error')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Error(string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(null, title == null? "Error" : title, message, MessageType.Error, buttons);
        }


        /// <summary>
        /// Shows a message box with the error icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Error')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Error(Window owner, string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(owner, title == null ? "Error" : title, message, MessageType.Error, buttons);
        }


        /// <summary>
        /// Shows a message box with the warning icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Warning')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Warning(string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(null, title == null? "Warning" : title, message, MessageType.Warning, buttons);
        }

        /// <summary>
        /// Shows a message box with the warning icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Warning')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Warning(Window owner, string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(owner, title == null ? "Warning" : title, message, MessageType.Warning, buttons);
        }


        /// <summary>
        /// Shows a message box with the question icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Question')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Question(string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(null, title == null ? "Question" : title, message, MessageType.Question, buttons);
        }


        /// <summary>
        /// Shows a message box with the question icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Question')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Question(Window owner, string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(owner, title == null ? "Question" : title, message, MessageType.Question, buttons);
        }



        /// <summary>
        /// Shows a message box with the success icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Question')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Success(string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(null, title == null ? "Success" : title, message, MessageType.Success, buttons);
        }


        /// <summary>
        /// Shows a message box with the success icon
        /// </summary>
        /// <param name="title">Title of this box (if null it'll be 'Question')</param>
        /// <param name="message">Message contained in this message box</param>
        /// <param name="buttons">The key is the text a button has and its value is the type of that button</param>
        public static bool? Success(Window owner, string title, string message, Dictionary<string, ButtonType> buttons)
        {
            return Show(owner, title == null ? "Success" : title, message, MessageType.Success, buttons);
        }
        
        public void Build(Window owner, string title, string message, MessageType type, Dictionary<string, ButtonType> buttons)
        {
            if(owner != null) Owner = owner;
            if (title != null) this.Title = title;
            this.MessageBlock.Text = message;
            if (type != MessageType.None)
            {
                this.MessageIcon.Visibility = Visibility.Visible;
                switch (type)
                {
                    case MessageType.None:
                        break;
                    case MessageType.Warning:
                        this.MessageIcon.Style = (Style)this.Resources["WarningIcon"];
                        break;
                    case MessageType.Log:
                        this.MessageIcon.Style = (Style)this.Resources["LogIcon"];
                        break;
                    case MessageType.Error:
                        this.MessageIcon.Style = (Style)this.Resources["ErrorIcon"];
                        break;
                    case MessageType.Question:
                        this.MessageIcon.Style = (Style)this.Resources["QuestionIcon"];
                        break;
                    case MessageType.Success:
                        this.MessageIcon.Style = (Style)this.Resources["SuccessIcon"];
                        break;
                    default:
                        break;
                }
            }
            if (buttons != null && buttons.Count > 0)
            {
                foreach (KeyValuePair<string, ButtonType> kvp in buttons)
                {
                    var newBtn = new IconButtonAnimated();
                    newBtn.Style = (Style)this.Resources["CommonButton"];
                    newBtn.Content = kvp.Key;
                    if (kvp.Value == ButtonType.Accept)
                    {
                        newBtn.IsDefault = kvp.Value == ButtonType.Accept;
                        newBtn.Click += this.OnSelectClick;

                    }
                    else if (kvp.Value == ButtonType.Cancel)
                    {
                        newBtn.IsCancel = kvp.Value == ButtonType.Cancel;
                        newBtn.Click += this.OnCancelClick;
                    }
                    this.buttonsPanel.Children.Add(newBtn);

                }
            }
            else
            {
                this.buttonsPanel.Margin = new Thickness(0, 15, 0, 0); // This way it'll look nicer
            }

        }

        public static void ShowUntil(Window owner, string title, string message, MessageType type, Dictionary<string, ButtonType> buttons, Action asyncAction)
        {
            var currentMsb = new Controls.MessageBox();
            if (owner != null) currentMsb.Owner = owner;
            Task.Run(() => {
                asyncAction.Invoke();
                currentMsb.Dispatcher.BeginInvoke((Action)(() => {
                    try
                    {
                        currentMsb.DialogResult = true;
                    }
                    catch { }
            }));  });
            currentMsb.ShowFromInstance(title, message, type, buttons);
        }

        #endregion
    }
}
