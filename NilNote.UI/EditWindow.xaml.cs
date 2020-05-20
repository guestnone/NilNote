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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private NoteBookPage mPage;

        public NoteBookPage Page
        {
            get => mPage;
        }
        public bool Save { get; set; } = false;

        public EditWindow(NoteBookPage page)
        {
            InitializeComponent();
            mPage = page;
            switch (page.MarkupType)
            {
                case NoteBookPageMarkupType.PlainText:
                    ContentControl.Content = new PlainTextPreviewUserControl(page.Text);
                    break;
                case NoteBookPageMarkupType.Markdown:
                    break;
                default:
                    ContentControl.Content = new PlainTextPreviewUserControl(page.Text);
                    break;
            }
            Editor.Text = page.Text;
        }

        private void Editor_OnTextChanged(object? sender, EventArgs e)
        {
            if (ContentControl == null) return;

            switch (mPage.MarkupType)
            {

                case NoteBookPageMarkupType.Markdown:
                    break;
                case NoteBookPageMarkupType.PlainText:
                default:
                    ((PlainTextPreviewUserControl)ContentControl.Content).Input = Editor.Text;
                    break;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            mPage.Text = Editor.Text;
            mPage.DateOfLastModification = DateTime.Now;
            Save = true;
            this.Close();
        }

        private void PageSettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
