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
    /// Interaction logic for EditTagWindow.xaml
    /// </summary>
    public partial class EditTagWindow : Window
    {
        public bool Change { get; set; } = false;
        public Tag NbTag { get => mTag; }
        private Tag mTag;

        public EditTagWindow(Tag tag)
        {
            InitializeComponent();
            mTag = tag;
            TextBox.Text = mTag.Name;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            mTag.Name = TextBox.Text;
            Change = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
