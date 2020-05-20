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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NilNote.UI
{
    /// <summary>
    /// Interaction logic for PlainTextPreviewUserControl.xaml
    /// </summary>
    public partial class PlainTextPreviewUserControl : UserControl
    {
        private string mInput = "";

        public string Input
        {
            get => mInput;
            set
            { 
                mInput = value;
                TextBlock.Text = mInput;
            }
        }

        public PlainTextPreviewUserControl(string input)
        {
            InitializeComponent();
            mInput = input;
            TextBlock.Text = mInput;
        }
    }
}
