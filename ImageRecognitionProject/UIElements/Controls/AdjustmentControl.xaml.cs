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
using Utilities;

namespace UIElements
{
    /// <summary>
    /// Interaction logic for AdjustmentControl.xaml
    /// </summary>
    public partial class AdjustmentControl : UserControl
    {
        public Slider Slider { get; set; }
        public AdjustmentControl(string name, ImageSource titleLogo)
        {
            InitializeComponent();
            Slider = slider;
            titleLabel.Content = name;
            image.Source = titleLogo;
            Margin = new Thickness(10,10,10,0);
        }
    }
}
