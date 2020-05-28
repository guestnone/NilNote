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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExportAllCheckBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
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
    }
}
