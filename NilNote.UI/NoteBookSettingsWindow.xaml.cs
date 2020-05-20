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
    /// <summary>
    /// Interaction logic for NoteBookSettings.xaml
    /// </summary>
    public partial class NoteBookSettingsWindow : Window
    {
        private bool mSave = false;

        public bool Save { get => mSave; }

        private NoteBookDetails mDetails = new NoteBookDetails();

        public NoteBookDetails Settings { get => mDetails; }

        public NoteBookSettingsWindow()
        {
            InitializeComponent();
            SyntaxComboBox.ItemsSource = Enum.GetValues(typeof(NoteBookPageMarkupType)).Cast<NoteBookPageMarkupType>();
            SyntaxComboBox.SelectedIndex = 0;
            LanguageComboBox.ItemsSource = Enum.GetValues(typeof(Language)).Cast<Language>();
            LanguageComboBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TextBox.Text))
            {
                MessageBox.Show("Name of the Notebook can't be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mDetails.Name = TextBox.Text;
            mDetails.Creator = Environment.UserName;
            mDetails.DefaultMarkupType = (NoteBookPageMarkupType) SyntaxComboBox.SelectedItem;
            mDetails.DefaultLanguage = (Language) LanguageComboBox.SelectedItem;
            mDetails.DateOfCreation = DateTime.Now;
            mDetails.DateOfLastOpen = DateTime.Now;
            mSave = true;
            this.Close();
        }
    }
}
