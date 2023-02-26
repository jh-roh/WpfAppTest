using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfAppTest.View
{
    public class BaseModule : UserControl
    {
        public BaseModule()
        {

        }



        public void GetModuleVisibleData(ContentControl targetConentControl)
        {
            var ContentControl_AgingControl = this.FindName("ContentControl_AgingControl") as ContentControl;

            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(ContentControl_AgingControl);

            var dataTemplate = myContentPresenter.ContentTemplate;

            if (dataTemplate != null)
            {
                var viewBox_CheckImage = dataTemplate.FindName("viewBox_CheckImage", myContentPresenter);
                var textBlock_AgingCount = dataTemplate.FindName("textBlock_AgingCount", myContentPresenter);
                var viewBox_ErrorImage = dataTemplate.FindName("viewBox_ErrorImage", myContentPresenter);
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
    where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }


    }
}
