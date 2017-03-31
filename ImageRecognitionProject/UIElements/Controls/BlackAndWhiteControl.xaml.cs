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
    /// Interaction logic for BlackAndWhiteControl.xaml
    /// </summary>
    public partial class BlackAndWhiteControl : UserControl
    {
        public Grid Host { get; set; }
        public bool State { get; set; }
        public BlackAndWhiteControl(string name, ImageSource titleLogo)
        {
            InitializeComponent();
            State = false;
            Margin = new Thickness(10, 10, 10, 10);
            HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = name;
        }



        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (State == false)
            {
                body.Fill = (Brush)new BrushConverter().ConvertFrom("#FF4FC3F7");
                State = true;
            }
            else if (State == true)
            {
                body.Fill = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");
                State = false;
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
