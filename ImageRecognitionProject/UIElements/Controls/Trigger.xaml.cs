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

        public bool State
        {
            get { return _on; }
            set { _on = value; }
        }

        public Trigger()
        {
            InitializeComponent();
            State = false;
            Margin = new Thickness(10, 35, 10, 10);
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (State)
            {
                button.HorizontalAlignment = HorizontalAlignment.Left;
                body.Fill = (Brush)new BrushConverter().ConvertFrom("#FF0277BD");
                State = false;
            }
            else if (!State)
            {
                button.HorizontalAlignment = HorizontalAlignment.Right;
                body.Fill = (Brush)new BrushConverter().ConvertFrom("#FFB3E5FC");
                State = true;
            }
        }
    }
}
