using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for NoteBookStatsWindow.xaml
    /// </summary>
    public partial class NoteBookStatsWindow : Window
    {
        public NoteBookStatsWindow(NoteBookDetails details, int numOfPages, int wordCount)
        {
            InitializeComponent();
            NameValueLabel.Content = details.Name;
            DateOfCreationValueLabel.Content = details.DateOfCreation.ToString(CultureInfo.InvariantCulture);
            DateOfLastModValueLabel.Content = details.DateOfLastOpen.ToString(CultureInfo.InvariantCulture);
            DefaultLanguageValueLabel.Content = details.DefaultLanguage;
            NumOfPagesValueLabel.Content = numOfPages.ToString();
            WordCountValueLabel.Content = wordCount.ToString();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StatsToFileButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
