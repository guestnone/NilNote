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
using NilNote.Core;

namespace NilNote.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedUICommand StatisticsCommand = new RoutedUICommand("Notebook statistics command",
            "StatisticsCommand",
            typeof(MainWindow));

        public static RoutedUICommand TagsCommand = new RoutedUICommand("Tags command",
            "TagsCommand",
            typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            PagesListBox.ItemsSource = NoteBookManager.Instance.GetPages();
            ContentControl.Content = new NoSelectUserControl();
            PageDetailsButton.IsEnabled = false;
            RemovePageButton.IsEnabled = false;
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AboutWindow();
            dialog.Show();
        }

        private void PagesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PagesListBox.SelectedItem == null)
            {
                ContentControl.Content = new NoSelectUserControl();
                PageDetailsButton.IsEnabled = false;
                RemovePageButton.IsEnabled = false;
                return;
            }

            switch (((NoteBookPage) PagesListBox.SelectedItem).MarkupType)
            {
                case NoteBookPageMarkupType.PlainText:
                    ContentControl.Content = new PlainTextPreviewUserControl(((NoteBookPage)PagesListBox.SelectedItem).Text);
                    break;
                case NoteBookPageMarkupType.Markdown:
                    break;
                default:
                    ContentControl.Content = new PlainTextPreviewUserControl(((NoteBookPage)PagesListBox.SelectedItem).Text);
                    break;
            }
            PageDetailsButton.IsEnabled = true;
            RemovePageButton.IsEnabled = true;
        }

        private void AlwaysExecuteDetector(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void StatisticsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new NoteBookStatsWindow(NoteBookManager.Instance.GetDetails(), NoteBookManager.Instance.GetPageNumber(), NoteBookManager.Instance.GetNoteBookWordCount());
            dialog.Show();
        }

        private void PagesListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PagesListBox.SelectedItem == null)
            {
                return;
            }

            var dialog = new EditWindow((NoteBookPage)PagesListBox.SelectedItem);
            dialog.ShowDialog();
            if (dialog.Save)
            {
                if (!NoteBookManager.Instance.UpdatePage(dialog.Page))
                {
                    MessageBox.Show("Couldn't update page", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var selItem = PagesListBox.SelectedIndex;
                    PagesListBox.SelectedIndex = -1;
                    PagesListBox.ItemsSource = NoteBookManager.Instance.GetPages();
                    PagesListBox.SelectedIndex = selItem;
                }
            }
        }

        private void PageDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (PagesListBox.SelectedItem == null)
            {
                return;
            }
            var dialog = new PageStatsWindow((NoteBookPage)PagesListBox.SelectedItem);
            dialog.Show();
        }

        private void AddPageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewPageWindow();
            dialog.ShowDialog();
            if(dialog.Add)
            {
                var selItem = PagesListBox.SelectedIndex;
                PagesListBox.SelectedIndex = -1;
                NoteBookManager.Instance.InsertNewPage(dialog.Page);
                PagesListBox.ItemsSource = NoteBookManager.Instance.GetPages();
                PagesListBox.SelectedIndex = selItem;
            }
        }

        private void TagsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new TagManageWindow();
            dialog.Show();
        }
    }
}
