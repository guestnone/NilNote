using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NilNote.Core;

namespace NilNote.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool PreBoot()
        {
            if (!NoteBookManager.Instance.HasExistingDB)
            {
                if (MessageBox.Show(
                    "The Notebook doesn't exists on your computer, it is required to start the program.\nDo you want to create one?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var dialog = new NoteBookSettingsWindow();
                    dialog.ShowDialog();
                    if (dialog.Save)
                    {
                        NoteBookManager.Instance.CreateNewUserDatabase(dialog.Settings);
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }


        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            if (PreBoot())
            {
                var mainWindow = new MainWindow();
                //Re-enable normal shutdown mode.
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Can't run without database, bailing out", "Error", MessageBoxButton.OK);
                Current.Shutdown(-1);
            }
        }
    }
}
