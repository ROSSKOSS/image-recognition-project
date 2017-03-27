using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIElements
{
    /// <summary>
    /// Interaction logic for Trigger.xaml
    /// </summary>
    public partial class Trigger : UserControl
    {
        private bool _on;
        public Trigger()
        {
            InitializeComponent();
            _on = false;
            Margin = new Thickness(10, 35, 10, 10);
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_on)
            {
                button.HorizontalAlignment = HorizontalAlignment.Left;
                body.Fill = (Brush)new BrushConverter().ConvertFrom("#FF0277BD");
                _on = false;
            }
            else if (!_on)
            {
                button.HorizontalAlignment = HorizontalAlignment.Right;
                body.Fill = (Brush)new BrushConverter().ConvertFrom("#FFB3E5FC");
                _on = true;
            }
        }
    }
}
