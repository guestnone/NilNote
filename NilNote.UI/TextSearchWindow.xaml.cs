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
    /// Interaction logic for TextSearchWindow.xaml
    /// </summary>
    public partial class TextSearchWindow : Window
    {
        public bool FindByTag { get; set; } = false;

        public bool DoSearch { get; set; } = false;

        public string TextToFind { get; set; } = "";

        public TextSearchWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (FindByTagCheckBox.IsChecked != null) FindByTag = (bool) FindByTagCheckBox.IsChecked;
            TextToFind = ContentToSearchTextBox.Text;
            DoSearch = true;
            this.Close();
        }
    }
}
