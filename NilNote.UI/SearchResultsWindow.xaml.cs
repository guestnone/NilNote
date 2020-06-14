using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NilNote.Core;

namespace NilNote.UI
{
    /// <summary>
    /// Interaction logic for SearchResultsWindow.xaml
    /// </summary>
    public partial class SearchResultsWindow : Window
    {
        public SearchResultsWindow(IList<NoteBookPage> pages)
        {
            InitializeComponent();
            PagesListBox.ItemsSource = pages;
            ContentControl.Content = new NoSelectUserControl();
        }

        private void PagesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PagesListBox.SelectedItem == null)
            {
                ContentControl.Content = new NoSelectUserControl();
                return;
            }

            switch (((NoteBookPage)PagesListBox.SelectedItem).MarkupType)
            {
                case NoteBookPageMarkupType.PlainText:
                    ContentControl.Content = new PlainTextPreviewUserControl(((NoteBookPage)PagesListBox.SelectedItem).Text);
                    break;
                case NoteBookPageMarkupType.Markdown:
                    ContentControl.Content = new MarkdownPreviewUserControl(((NoteBookPage)PagesListBox.SelectedItem).Text);
                    break;
                default:
                    ContentControl.Content = new PlainTextPreviewUserControl(((NoteBookPage)PagesListBox.SelectedItem).Text);
                    break;
            }
        }
    }
}
