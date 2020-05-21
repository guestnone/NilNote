using NilNote.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NilNote.UI
{
    /// <summary>
    /// Interaction logic for PageSettingsWindow.xaml
    /// </summary>
    public partial class PageSettingsWindow : Window
    {
        private NoteBookPage mPage;
        public NoteBookPage EditedPage { get => mPage;  }

        public bool IsSave { get; set; } = false;

        public PageSettingsWindow(NoteBookPage page)
        {
            InitializeComponent();
            mPage = page;
            NameTextBox.Text = mPage.Name;
            SyntaxComboBox.ItemsSource = Enum.GetValues(typeof(NoteBookPageMarkupType)).Cast<NoteBookPageMarkupType>();
            SyntaxComboBox.SelectedItem = mPage.MarkupType;
            LanguageComboBox.ItemsSource = Enum.GetValues(typeof(Language)).Cast<Language>();
            LanguageComboBox.SelectedItem = mPage.Language;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Name of a page can't be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mPage.Name = NameTextBox.Text;
            mPage.Language = (Language)LanguageComboBox.SelectedItem;
            mPage.MarkupType = (NoteBookPageMarkupType)SyntaxComboBox.SelectedItem;
            
            IsSave = true;
            this.Close();
        }
    }
}
