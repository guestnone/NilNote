using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
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
    /// Interaction logic for PageExportWindow.xaml
    /// </summary>
    public partial class PageExportWindow : Window
    {
        private List<NoteBookPage> mPages;

        public PageExportWindow(List<NoteBookPage> pages)
        {
            InitializeComponent();
            mPages = pages;
            FormatTypeComboBox.ItemsSource = Enum.GetValues(typeof(NoteBookExportFormat)).Cast<NoteBookExportFormat>();
            PagesListBox.ItemsSource = mPages;
            FormatTypeComboBox.SelectedIndex = 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExportAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)ExportAllCheckBox.IsChecked)
            {
                PagesListBox.IsEnabled = false;
            }
            else
            {
                PagesListBox.IsEnabled = true;
            }
        }

        private Exporter GetExporter()
        {

            if (FormatTypeComboBox.SelectedItem == null)
            {
                return new HTMLExporter();
            }
            var selected = (NoteBookExportFormat)FormatTypeComboBox.SelectedItem;
            switch (selected)
            {
                case NoteBookExportFormat.PlainText:
                    return new PlainTextExporter();
                case NoteBookExportFormat.HTML:
                default:
                    return new HTMLExporter();
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var openDirDialog = new CommonOpenFileDialog();
            openDirDialog.IsFolderPicker = true;
            if (openDirDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var exporter = GetExporter();
                exporter.Start(openDirDialog.FileName);
                if ((bool)ExportAllCheckBox.IsChecked)
                {
                    exporter.ExportPages(NoteBookManager.Instance.GetPages().ToList());
                }
                else
                {
                    exporter.ExportPages(PagesListBox.SelectedItems);
                }
                exporter.End();
            }
            this.Close();
        }
    }
}
