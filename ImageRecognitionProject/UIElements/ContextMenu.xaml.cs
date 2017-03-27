using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UIElements
{
    /// <summary>
    /// Interaction logic for ContextMenu.xaml
    /// </summary>
    public partial class ContextMenu : UserControl
    {
        private int count;
        public List<Button> OptionList { get; set; }

        public ContextMenu(List<Button> optionsList)
        {
            InitializeComponent();
            OptionList = optionsList;
            count = 0;
            foreach (var option in OptionList)
            {
                host.Children.Add(option);
                option.Margin = new Thickness(0, -25, 0, 0);
            }

            count = 0;
            foreach (var option in OptionList)
            {
                count++;
                DoAnimation(option, count);
            }
        }

        private void DoAnimation(ContentControl option, int pos)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = -25;
            da.To = 25 * pos;

            da.DecelerationRatio = 0.9;
            da.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            TranslateTransform tr = new TranslateTransform();
            option.RenderTransform = tr;
            tr.BeginAnimation(TranslateTransform.YProperty, da);
        }
    }
}