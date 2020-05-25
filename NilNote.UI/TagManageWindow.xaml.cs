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
    /// Interaction logic for TagManageWindow.xaml
    /// </summary>
    public partial class TagManageWindow : Window
    {
        public TagManageWindow()
        {
            InitializeComponent();
            TagsListBox.ItemsSource = NoteBookManager.Instance.GetTags();

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(NewTagTextBox.Text))
            {
                MessageBox.Show("Tag name cannot be empty!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var tag = new Tag();
            tag.Name = NewTagTextBox.Text;

            if (NoteBookManager.Instance.TagExist(tag))
            {
                MessageBox.Show("Tag already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!NoteBookManager.Instance.AddTag(tag))
            {
                MessageBox.Show("Error on adding tag!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TagsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TagsListBox.SelectedIndex == -1)
            {
                return;
            }
            Tag tag = (Tag)TagsListBox.SelectedItem;
            TagsListBox.SelectedIndex = -1;
            //NoteBookManager.Instance.RemoveTag(tag);

        }
    }
}
