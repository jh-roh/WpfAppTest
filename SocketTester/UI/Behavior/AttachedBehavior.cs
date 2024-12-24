using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SocketTester.UI.Behavior
{
    public static class TextBoxScrollBehavior
    {
        public static readonly DependencyProperty AutoScrollProperty = 
            DependencyProperty.RegisterAttached(
                "AutoScroll",
                typeof(bool),
                typeof(TextBoxScrollBehavior),
                new PropertyMetadata(false, OnAutoScrollChanged));

        public static bool GetAutoScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollProperty, value);
        }

        private static void OnAutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.TextChanged += TextBox_TextChanged;
                }
                else
                {
                    textBox.TextChanged -= TextBox_TextChanged;
                }
            }
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.ScrollToEnd();
            }
        }
    }

    public static class ScrollViewerBehavior
    {
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached(
                "AutoScroll",
                typeof(bool),
                typeof(ScrollViewerBehavior),
                new PropertyMetadata(false, OnAutoScrollChanged));

        public static bool GetAutoScroll(DependencyObject obj) => (bool)obj.GetValue(AutoScrollProperty);
        public static void SetAutoScroll(DependencyObject obj, bool value) => obj.SetValue(AutoScrollProperty, value);

        private static void OnAutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                if ((bool)e.NewValue)
                {
                    if (scrollViewer.DataContext is INotifyCollectionChanged collection)
                    {
                        collection.CollectionChanged += (s, args) => scrollViewer.ScrollToEnd();
                    }
                }
            }
        }
    }
}
