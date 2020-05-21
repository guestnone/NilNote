using NilNote.Core;
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

namespace NilNote.UI
{
    /// <summary>
    /// Interaction logic for PageStatsWindow.xaml
    /// </summary>
    public partial class PageStatsWindow : Window
    {
        public PageStatsWindow(NoteBookPage page)
        {
            InitializeComponent();
            NameValue.Content = page.Name;
            DateOfCreationValue.Content = page.DateOfCreation;
            DateOfLastModValue.Content = page.DateOfLastModification;
            LanguageValue.Content = page.Language;
            SyntaxValue.Content = page.MarkupType;
            WordCountValue.Content = Convert.ToString(Utility.CountWords(page.Text));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
