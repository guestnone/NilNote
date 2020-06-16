using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NilNote.Core;
using NilNote.UI.SpellCheckAvalonEdit;

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
                
                case NoteBookPageMarkupType.Markdown:
                    ContentControl.Content = new MarkdownPreviewUserControl(page.Text);
                    break;
                case NoteBookPageMarkupType.PlainText:
                default:
                    ContentControl.Content = new PlainTextPreviewUserControl(page.Text);
                    break;
            }
            Editor.Text = page.Text;
            Editor.TextArea.TextView.LineTransformers.Add(new SpellingErrorColorizer());
            Editor.Language = XmlLanguage.GetLanguage(StaticStuff.ToIETFCode(mPage.Language));
        }

        private void Editor_OnTextChanged(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (ContentControl == null) return;

            switch (mPage.MarkupType)
            {

                case NoteBookPageMarkupType.Markdown:
                    ((MarkdownPreviewUserControl)ContentControl.Content).Input = Editor.Text;
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
            var dialog = new PageSettingsWindow(mPage);
            dialog.ShowDialog();
            if(dialog.IsSave)
            {
                mPage.Name = dialog.EditedPage.Name;
                mPage.MarkupType = dialog.EditedPage.MarkupType;
                mPage.Language = dialog.EditedPage.Language;
            }
        }

        private void TagsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectTagWindow(NoteBookManager.Instance.GetTags().ToList(), mPage.Tags);
            dialog.ShowDialog();
            if (dialog.IsChanged)
            {
                mPage.Tags = dialog.Selected;
            }
        }
    }
}
