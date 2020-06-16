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
using Microsoft.Win32;
using SpoiledCat.NiceIO;

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

            if (page.Tags != null)
            {
                var tagList = NoteBookManager.Instance.GetTags().ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (var tag in page.Tags)
                {
                    foreach (var origTag in tagList)
                    {
                        if (tag.Id == origTag.Id)
                         sb.AppendFormat("{0},", origTag.Name);
                    }
                }

                TagsValue.Content = sb.ToString();
            }
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
                sb.AppendLine("Name              : " + NameValue.Content);
                sb.AppendLine("Date of creation  : " + DateOfCreationValue.Content);
                sb.AppendLine("Date of Last Edit : " + DateOfLastModValue.Content);
                sb.AppendLine("Language          : " + LanguageValue.Content);
                sb.AppendLine("Syntax            : " + SyntaxValue.Content);
                sb.AppendLine("Word count        : " + WordCountValue.Content);
                sb.AppendLine("Tags              : " + TagsValue.Content);
                niceIoPath.WriteAllText(sb.ToString());
            }
        }
    }
}
