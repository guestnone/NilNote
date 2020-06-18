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
            TagsListBox.ItemsSource = NoteBookManager.Instance.GetTags();
            NewTagTextBox.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagsListBox.SelectedIndex == -1)
            {
                return;
            }
            if (MessageBox.Show("Are you sure?", "Delete tag", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Tag tag = (Tag)TagsListBox.SelectedItem;
                TagsListBox.SelectedIndex = -1;
                NoteBookManager.Instance.RemoveTag(tag);
                TagsListBox.ItemsSource = NoteBookManager.Instance.GetTags();
            }
        }

        private void TagsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TagsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TagsListBox.SelectedIndex == -1)
            {
                return;
            }
            var dialog = new EditTagWindow((Tag)TagsListBox.SelectedItem);
            dialog.ShowDialog();
            if (dialog.Change)
            {
                var tag = dialog.NbTag;
                // Check if the name already exists
                if (NoteBookManager.Instance.TagExist(tag))
                {
                    MessageBox.Show("Tag already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                NoteBookManager.Instance.UpdateTag(tag);
                TagsListBox.SelectedIndex = -1;
                TagsListBox.ItemsSource = NoteBookManager.Instance.GetTags();
            }
        }
    }
}
