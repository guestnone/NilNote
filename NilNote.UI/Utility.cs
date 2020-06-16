using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Windows.Documents;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.Xaml.Behaviors;
using NilNote.Core;

namespace NilNote.UI
{
    static class Utility
    {
        public static int CountWords(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return 0;
            }
            int wordCount = 0, index = 0;

            while (index < text.Length && char.IsWhiteSpace(text[index]))
                index++;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;
                
                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
                }

            return wordCount;
        }
    }

    public class ListBoxSelectionBehavior<T> : Behavior<ListBox>
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList),
                typeof(ListBoxSelectionBehavior<T>),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemsChanged));

        public static readonly DependencyProperty SelectedValuesProperty =
            DependencyProperty.Register(nameof(SelectedValues), typeof(IList),
                typeof(ListBoxSelectionBehavior<T>),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedValuesChanged));

        private static void OnSelectedItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (ListBoxSelectionBehavior<T>)sender;
            if (behavior._modelHandled) return;

            if (behavior.AssociatedObject == null)
                return;

            behavior._modelHandled = true;
            behavior.SelectedItemsToValues();
            behavior.SelectItems();
            behavior._modelHandled = false;
        }

        private static void OnSelectedValuesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (ListBoxSelectionBehavior<T>)sender;
            if (behavior._modelHandled) return;

            if (behavior.AssociatedObject == null)
                return;

            behavior._modelHandled = true;
            behavior.SelectedValuesToItems();
            behavior.SelectItems();
            behavior._modelHandled = false;
        }

        private static object GetDeepPropertyValue(object obj, string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return obj;
            while (true)
            {
                if (path.Contains('.'))
                {
                    string[] split = path.Split('.');
                    string remainingProperty = path.Substring(path.IndexOf('.') + 1);
                    obj = obj.GetType().GetProperty(split[0]).GetValue(obj, null);
                    path = remainingProperty;
                    continue;
                }
                return obj.GetType().GetProperty(path).GetValue(obj, null);
            }
        }

        private bool _viewHandled;
        private bool _modelHandled;

        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        public IList SelectedValues
        {
            get => (IList)GetValue(SelectedValuesProperty);
            set => SetValue(SelectedValuesProperty, value);
        }

        // Propagate selected items from model to view
        private void SelectItems()
        {
            _viewHandled = true;
            AssociatedObject.SelectedItems.Clear();
            if (SelectedItems != null)
            {
                foreach (var item in SelectedItems)
                    AssociatedObject.SelectedItems.Add(item);
            }
            _viewHandled = false;
        }

        // Update SelectedItems based on SelectedValues
        private void SelectedValuesToItems()
        {
            if (SelectedValues == null)
            {
                SelectedItems = null;
            }
            else
            {
                SelectedItems =
                    AssociatedObject.Items.Cast<T>()
                        .Where(i => SelectedValues.Contains(GetDeepPropertyValue(i, AssociatedObject.SelectedValuePath)))
                        .ToArray();
            }
        }

        // Update SelectedValues based on SelectedItems
        private void SelectedItemsToValues()
        {
            if (SelectedItems == null)
            {
                SelectedValues = null;
            }
            else
            {
                SelectedValues =
                    SelectedItems.Cast<T>()
                        .Select(i => GetDeepPropertyValue(i, AssociatedObject.SelectedValuePath))
                        .ToArray();
            }
        }

        // Propagate selected items from view to model
        private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (_viewHandled) return;
            if (AssociatedObject.Items.SourceCollection == null) return;

            SelectedItems = AssociatedObject.SelectedItems.Cast<object>().ToArray();
        }

        // Re-select items when the set of items changes
        private void OnListBoxItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (_viewHandled) return;
            if (AssociatedObject.Items.SourceCollection == null) return;

            SelectItems();
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectionChanged += OnListBoxSelectionChanged;
            ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged += OnListBoxItemsChanged;

            _modelHandled = true;
            SelectedValuesToItems();
            SelectItems();
            _modelHandled = false;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged -= OnListBoxSelectionChanged;
                ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged -= OnListBoxItemsChanged;
            }
        }
    }

    public class TagListBoxSelectionBehavior : ListBoxSelectionBehavior<Tag>
    {
    }


    namespace SpellCheckAvalonEdit
    {
        internal class SpellCheckBehavior : Behavior<TextEditor>
        {
            private TextBox textBox;
            private TextEditor textEditor;

            protected override void OnAttached()
            {
                textEditor = AssociatedObject;
                if (textEditor != null)
                {
                    textBox = new TextBox();
                    textEditor.ContextMenuOpening += textEditor_ContextMenuOpening;
                }
                base.OnAttached();
            }

            private void textEditor_ContextMenuOpening(object sender, ContextMenuEventArgs e)
            {
                TextViewPosition? pos = textEditor.TextArea.TextView.GetPosition(new Point(e.CursorLeft, e.CursorTop));

                if (pos != null)
                {
                    //Reset the context menu
                    textEditor.ContextMenu = null;

                    //Get the new caret position
                    int newCaret = textEditor.Document.GetOffset(pos.Value.Line, pos.Value.Column);

                    //Text box properties
                    textBox.AcceptsReturn = true;
                    textBox.AcceptsTab = true;
                    textBox.SpellCheck.IsEnabled = true;
                    textBox.Text = textEditor.Text;

                    //Check for spelling errors
                    SpellingError error = textBox.GetSpellingError(newCaret);

                    //If there is a spelling mistake
                    if (error != null)
                    {
                        textEditor.ContextMenu = new ContextMenu();
                        int wordStartOffset = textBox.GetSpellingErrorStart(newCaret);
                        int wordLength = textBox.GetSpellingErrorLength(wordStartOffset);
                        foreach (string err in error.Suggestions)
                        {
                            var item = new MenuItem { Header = err, FontWeight = FontWeights.Bold };
                            var t = new Tuple<int, int>(wordStartOffset, wordLength);
                            item.Tag = t;
                            item.Click += item_Click;
                            textEditor.ContextMenu.Items.Add(item);
                        }
                    }
                }
            }

            //Click event of the context menu
            private void item_Click(object sender, RoutedEventArgs e)
            {
                var item = sender as MenuItem;
                if (item != null)
                {
                    var seg = item.Tag as Tuple<int, int>;
                    textEditor.Document.Replace(seg.Item1, seg.Item2, item.Header.ToString());
                }
            }
        }
        public class SpellingErrorColorizer : DocumentColorizingTransformer
        {
            private static readonly TextBox staticTextBox = new TextBox();
            private readonly TextDecorationCollection collection;

            public SpellingErrorColorizer()
            {
                // Create the Text decoration collection for the visual effect - you can get creative here
                collection = new TextDecorationCollection();
                var dec = new TextDecoration();
                dec.Pen = new Pen { Thickness = 1, DashStyle = DashStyles.DashDot, Brush = new SolidColorBrush(Colors.Red) };
                dec.PenThicknessUnit = TextDecorationUnit.FontRecommended;
                collection.Add(dec);

                // Set the static text box properties
                staticTextBox.AcceptsReturn = true;
                staticTextBox.AcceptsTab = true;
                staticTextBox.SpellCheck.IsEnabled = true;
            }

            protected override void ColorizeLine(DocumentLine line)
            {
                lock (staticTextBox)
                {
                    staticTextBox.Text = CurrentContext.Document.Text;
                    int start = line.Offset;
                    int end = line.EndOffset;
                    start = staticTextBox.GetNextSpellingErrorCharacterIndex(start, LogicalDirection.Forward);
                    while (start < end)
                    {
                        if (start == -1)
                            break;

                        int wordEnd = start + staticTextBox.GetSpellingErrorLength(start);

                        SpellingError error = staticTextBox.GetSpellingError(start);
                        if (error != null)
                        {
                            base.ChangeLinePart(start, wordEnd, (VisualLineElement element) => element.TextRunProperties.SetTextDecorations(collection));
                        }

                        start = staticTextBox.GetNextSpellingErrorCharacterIndex(wordEnd, LogicalDirection.Forward);
                    }
                }
            }
        }
    }
}
