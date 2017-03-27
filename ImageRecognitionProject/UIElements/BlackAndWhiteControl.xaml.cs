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
        public BlackAndWhiteControl(string name, ImageSource titleLogo)
        {
            InitializeComponent();
            Host = host;
            titleLabel.Content = name;
            image.Source = titleLogo;
            Margin = new Thickness(10, 10, 10, 0);
            HorizontalAlignment = HorizontalAlignment.Left;

        }

    }
}
