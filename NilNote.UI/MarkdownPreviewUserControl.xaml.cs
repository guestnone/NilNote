using Markdig;
using Markdig.Wpf;
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
    public partial class MarkdownPreviewUserControl : UserControl
    {
        private string mInput = "";

        public string Input
        {
            get => mInput;
            set
            { 
                mInput = value;
                Viewer.Markdown = mInput;
            }
        }

        public MarkdownPreviewUserControl(string input)
        {
            InitializeComponent();
            mInput = input;
            Viewer.Pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
            Viewer.Markdown = mInput;
        }
    }
}
