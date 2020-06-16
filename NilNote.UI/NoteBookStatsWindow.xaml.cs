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
using Microsoft.Win32;
using NilNote.Core;
using SpoiledCat.NiceIO;

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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Saved Data|*.txt";
            if (sfd.ShowDialog() == true)
            {
                NPath niceIoPath = new NPath(sfd.FileName);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Name              : " + NameValueLabel.Content);
                sb.AppendLine("Date of creation  : " + DateOfCreationValueLabel.Content);
                sb.AppendLine("Date of Last Edit : " + DateOfLastModValueLabel.Content);
                sb.AppendLine("Default Language  : " + DefaultLanguageValueLabel.Content);
                sb.AppendLine("Number of pages   : " + NumOfPagesValueLabel.Content);
                sb.AppendLine("Word count        : " + WordCountValueLabel.Content);
                niceIoPath.WriteAllText(sb.ToString());
            }
        }
    }
}
