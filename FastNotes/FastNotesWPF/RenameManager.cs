using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Controls;

namespace FastNotes
{
    public class RenameManager
    {
        #region Checking desktop thread
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        Task checkerNameTask;
        #endregion

        #region Required Items
        HighlightableMenuItem currentRenamableItem;
        TextBox currentRenamableTextBox;
        Desktop currentRenamableDesktop;
        ContextMenu contextMenu;
        #endregion

        /// <summary>
        /// Manages all the events and procedures when a desktop is being renamed
        /// </summary>
        /// <param name="contextMenu">Context menu that contains the items to be renamed</param>
        public RenameManager(ContextMenu contextMenu)
        {
            contextMenu.Unloaded += OnNotifyContextMenuClosed;
            this.contextMenu = contextMenu;
        }

        private void RenameManager_LostFocus23(object sender, EventArgs e)
        {
            Debug.WriteLine("Unloaded");
            contextMenu.IsOpen = true;
        }

        private void OnNotifyContextMenuClosed(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Unloaded");
            if (currentRenamableTextBox != null && currentRenamableTextBox.IsEnabled)
            {
                RenameCurrent(backIfError: true);
            }
        }

        /// <summary>
        /// Apply the events that are needed to be able of renaming the desktop item
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="desktop"></param>
        /// <param name="desktopItem"></param>
        public void ApplyItemEvents(TextBox textBox, Desktop desktop, HighlightableMenuItem desktopItem)
        {
            desktopItem.PreviewKeyUp += (s,e) => OnDesktopItemPreviewKeyUp(s, e, textBox, desktopItem, desktop);
            textBox.IsVisibleChanged += OnTextBoxVisibleChanged;
            textBox.GotKeyboardFocus += OnTextBoxGotKeyboardFocus;
            textBox.KeyUp += OnTextBoxKeyUp;
            textBox.KeyDown += OnTextBoxKeyDown;
            textBox.PreviewKeyDown += OnTextBoxPreviewKeyDown;
            desktopItem.SubmenuOpened += (a, b) =>
            {
                if (currentRenamableTextBox != null && currentRenamableTextBox.IsEnabled)
                { // Auto closes its submenu when the user can write
                    desktopItem.IsSubmenuOpen = false;
                }
            };
            textBox.PreviewLostKeyboardFocus += (a, b) =>
            {
                if (contextMenu.IsKeyboardFocusWithin) // Avoids the textBox lose focus because of the context menu
                {
                    b.Handled = true;
                }
            };
        }


        private void OnTextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (currentRenamableTextBox != null && currentRenamableTextBox.IsEnabled && e.Key == Key.Escape) // User can cancell editing by pressing Escape
            {
                currentRenamableTextBox.Text = Desktop.GetNameFromEscaped(currentRenamableDesktop.Name);
                RestoreCurrent();
                e.Handled = true;
            }
        }

        private void OnDesktopItemPreviewKeyUp(object s, KeyEventArgs e, TextBox textBox, HighlightableMenuItem desktopItem, Desktop desktop)
        {
            if (e.Key == Key.F2) // Handling renaming key (F2)
            {
                Prepare(textBox, desktopItem, desktop);
                desktopItem.IsSubmenuOpen = false;// Closes the submenu
                e.Handled = true;

            }
        }

        private void OnTextBoxVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                currentRenamableTextBox.Focus();
                Debug.Write("Focusing..");

            }
        }
        private void OnTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            currentRenamableTextBox.SelectAll();
            Debug.Write("Selecting all..");
        }
        public void RestoreCurrent()
        {
            if (currentRenamableItem == null || currentRenamableTextBox == null) return;
            currentRenamableItem.CaptureHover = false;
            currentRenamableTextBox.IsEnabled = false;
        }
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && currentRenamableTextBox != null && currentRenamableTextBox.IsEnabled)
            {
                // Rename it
                if (!RenameCurrent()) return; // If an error happens (such as existing desktop name) the user needs to handle it

                contextMenu.CaptureMouse(); // Return the focus to the main context menu
                e.Handled = true;
            }
        }
        public bool RenameCurrent(bool backIfError = false)
        {
            RestoreCurrent();
            currentRenamableTextBox.Text = currentRenamableTextBox.Text.TrimEnd(' '); // A folder can't have spaces at the end
            if (currentRenamableTextBox.Text == "") // The user didn't introduce anything and pressed enter -> restore the name
            {
                currentRenamableTextBox.Text = Desktop.GetEscapedName(currentRenamableDesktop.Name);
                return true; // Renaming is done
            }
            // Finds the first desktop that has the same name
            if (FirstDesktopSameName(currentRenamableDesktop, currentRenamableTextBox.Text, (d) =>
            {
                if (backIfError)
                {
                    currentRenamableTextBox.Text = Desktop.GetNameFromEscaped(currentRenamableDesktop.Name);
                }
                else
                {
                    if (contextMenu.IsOpen) contextMenu.IsOpen = false;

                    // Here the context menu will close
                    Controls.MessageBox.Error(Program.MainWindow, null, $"There already exists a desktop named \"{currentRenamableTextBox.Text}\".", Controls.MessageBox.Ok);
                    // Here we open it again
                    //Mouse.Capture(null);
                    contextMenu.IsOpen = true;
                    ((HighlightableMenuItem)currentRenamableItem.Parent).IsSubmenuOpen = true;
                    currentRenamableItem.CaptureHover = true;
                    currentRenamableTextBox.IsEnabled = true;
                }

            })) return backIfError; // Error happened, if there's no backiferror -> return will be false. If there's backiferror -> return will be true

            string prevName = currentRenamableDesktop.Name;
            if (currentRenamableDesktop.Name != currentRenamableTextBox.Text && !currentRenamableDesktop.Rename(Desktop.GetEscapedName(currentRenamableTextBox.Text), silently: true))
            {
                if (backIfError)
                {
                    currentRenamableTextBox.Text = Desktop.GetNameFromEscaped(currentRenamableDesktop.Name);
                    return true;
                }
                else
                {
                    contextMenu.IsOpen = false;
                    // Here the context menu will close
                    Controls.MessageBox.Error(Program.MainWindow, null, $"It wasn't possible to rename the desktop to \"{currentRenamableTextBox.Text}\".", Controls.MessageBox.Ok);
                    // Here we open it again
                    contextMenu.IsOpen = true;
                    ((HighlightableMenuItem)currentRenamableItem.Parent).IsSubmenuOpen = true;
                    currentRenamableItem.CaptureHover = true;
                    currentRenamableTextBox.IsEnabled = true;
                    return false; // Error happened
                }
            }

            return true; // Renamed successfully
        }
        private void OnTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter && currentRenamableTextBox != null && currentRenamableTextBox.IsEnabled)
            {
                if (checkerNameTask != null && !checkerNameTask.IsCompleted)
                {
                    cancellationTokenSource.Cancel();
                }
                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;
                checkerNameTask = null;
                string textboxText = currentRenamableTextBox.Text;
                checkerNameTask = Task.Run(() =>
                {
                    if(FirstDesktopSameName(currentRenamableDesktop, textboxText, (d) =>
                    {
                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            // Red color
                            currentRenamableTextBox.Foreground = new SolidColorBrush(Color.FromRgb(255, 82, 82));
                        }));
                    })) return;
                    // Return its color
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        currentRenamableTextBox.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }));
                }, cancellationToken);
            }
        }
        /// <summary>
        /// Used when trying to rename 'desktop' to 'newName', if there's one desktop that has the name of newName an action is performed.
        /// </summary>
        /// <param name="desktop"></param>
        /// <param name="newName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool FirstDesktopSameName(Desktop desktop, string newName, Action<Desktop> action)
        {
            foreach (var d in MainWindow.CurrentConfig.desktops)
            {
                if (cancellationToken.IsCancellationRequested) return false;
                if (desktop != d && Desktop.GetNameFromEscaped(d.Name) == newName)
                {
                    action(desktop);
                    return true;
                }
            }
            return false;
        }

        public void Prepare(TextBox desktopEditableName, HighlightableMenuItem thisDesktopItem, Desktop desktop)
        {
            if(currentRenamableTextBox == null || !currentRenamableTextBox.IsEnabled)
            {
                currentRenamableTextBox = desktopEditableName;
                currentRenamableItem = thisDesktopItem;
                currentRenamableDesktop = desktop;
                thisDesktopItem.CaptureHover = true;
                desktopEditableName.IsEnabled = true;// Make it focusable
                Debug.WriteLine("Preparing..");
            }
            else
            {
                // This part will never occur, but I'll leave it here, it works if somehow it happens
                // However the new textbox for some reason is never selected
                Debug.WriteLine("Renaming current..");
                if (RenameCurrent()) // Make sure the previous desktop was renamed
                {
                    currentRenamableTextBox = null; // Set it to null so that when this function is called again, the new desktop is prepared to be renamed
                    Prepare(desktopEditableName, thisDesktopItem, desktop); // Call this again
                }
            }

        }



    }
}
