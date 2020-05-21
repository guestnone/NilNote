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
using NilNote.Core;

namespace NilNote.UI
{
    enum NewPageValidationResult
    {
        Ok,
        ErrNoName
    }

    /// <summary>
    /// Interaction logic for NewPageWindow.xaml
    /// </summary>
    public partial class NewPageWindow : Window
    {
        private bool mIsAdd = false;
        private NoteBookPage mPage = new NoteBookPage();

        public NoteBookPage Page => mPage;
        public bool Add => mIsAdd;

        public NewPageWindow()
        {
            InitializeComponent();
            SyntaxComboBox.ItemsSource = Enum.GetValues(typeof(NoteBookPageMarkupType)).Cast<NoteBookPageMarkupType>();
            SyntaxComboBox.SelectedIndex = 0;
            LanguageComboBox.ItemsSource = Enum.GetValues(typeof(Language)).Cast<Language>();
            LanguageComboBox.SelectedIndex = 0;
        }

        private NewPageValidationResult CreatePageSetting()
        {
            if (String.IsNullOrEmpty(NameTextBox.Text))
                return NewPageValidationResult.ErrNoName;
            mPage.Name = NameTextBox.Text;
            mPage.DateOfCreation = DateTime.Now;
            mPage.DateOfLastModification = DateTime.Now;
            mPage.MarkupType = (NoteBookPageMarkupType)SyntaxComboBox.SelectedItem;
            mPage.Language = (Language)LanguageComboBox.SelectedItem;
            return NewPageValidationResult.Ok;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if(CreatePageSetting() == NewPageValidationResult.ErrNoName)
            {
                MessageBox.Show("Name of the Notebook can't be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mIsAdd = true;
            this.Close();

        }
    }
}
