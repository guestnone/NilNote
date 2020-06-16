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
    /// Interaction logic for SelectTagWindow.xaml
    /// </summary>
    public partial class SelectTagWindow : Window
    {
        public bool IsChanged { get; set; } = false;

        public List<Tag> Selected { get; set; } = new List<Tag>();

        public SelectTagWindow(IList<Tag> tagList, List<Tag> selectedTags)
        {
            InitializeComponent();
            TagsListBox.ItemsSource = tagList;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var ShittyList = TagsListBox.SelectedItems;
            foreach (var tag in ShittyList)
            {
                Selected.Add((Tag)tag);
            }
            IsChanged = true;
            this.Close();
        }
    }
}
