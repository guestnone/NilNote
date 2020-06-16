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
    /// Interaction logic for DateSearchWindow.xaml
    /// </summary>
    public partial class DateSearchWindow : Window
    {

        public bool FindByDateOfCreation { get; set; } = false;

        public bool DoSearch { get; set; } = false;

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

        public DateSearchWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ComboBox.ItemsSource = Enum.GetValues(typeof(NoteBookDateSearchMode)).Cast<NoteBookDateSearchMode>();
            ComboBox.SelectedIndex = 0;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            StartDate = DatePickerForStart.DisplayDate;
            EndDate = DatePickerForEnd.DisplayDate;
            if ((NoteBookDateSearchMode)ComboBox.SelectedItem == NoteBookDateSearchMode.DateOfCreation)
            {
                FindByDateOfCreation = true;
            }

            DoSearch = true;
            this.Close();
        }
    }
}
