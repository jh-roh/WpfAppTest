using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SocketTester.UI.Helper
{
    public static class CheckBoxHelper
    {
        public static readonly DependencyProperty CheckedCommandProperty =
        DependencyProperty.RegisterAttached(
            "CheckedCommand",
            typeof(ICommand),
            typeof(CheckBoxHelper),
            new PropertyMetadata(null, OnCheckedCommandChanged));

        public static readonly DependencyProperty UncheckedCommandProperty =
            DependencyProperty.RegisterAttached(
                "UncheckedCommand",
                typeof(ICommand),
                typeof(CheckBoxHelper),
                new PropertyMetadata(null, OnUncheckedCommandChanged));

        public static ICommand GetCheckedCommand(DependencyObject obj) =>
            (ICommand)obj.GetValue(CheckedCommandProperty);

        public static void SetCheckedCommand(DependencyObject obj, ICommand value) =>
            obj.SetValue(CheckedCommandProperty, value);

        public static ICommand GetUncheckedCommand(DependencyObject obj) =>
            (ICommand)obj.GetValue(UncheckedCommandProperty);

        public static void SetUncheckedCommand(DependencyObject obj, ICommand value) =>
            obj.SetValue(UncheckedCommandProperty, value);

        private static void OnCheckedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBox checkBox)
            {
                checkBox.Checked -= CheckBox_Checked;
                if (e.NewValue is ICommand)
                {
                    checkBox.Checked += CheckBox_Checked;
                }
            }
        }

        private static void OnUncheckedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBox checkBox)
            {
                checkBox.Unchecked -= CheckBox_Unchecked;
                if (e.NewValue is ICommand)
                {
                    checkBox.Unchecked += CheckBox_Unchecked;
                }
            }
        }

        private static void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                var command = GetCheckedCommand(checkBox);
                if (command?.CanExecute(null) == true)
                {
                    command.Execute(null);
                }
            }
        }

        private static void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                var command = GetUncheckedCommand(checkBox);
                if (command?.CanExecute(null) == true)
                {
                    command.Execute(null);
                }
            }
        }
    }
}
