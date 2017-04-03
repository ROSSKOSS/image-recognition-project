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

namespace UIElements.Controls
{
    /// <summary>
    /// Interaction logic for SubMenu.xaml
    /// </summary>
    public partial class SubMenu : UserControl
    {
        public WrapPanel Container { get; set; }
        public SubMenu(string title, ImageSource titleLogo)
        {
            InitializeComponent();
            image.Source = titleLogo;
            Container = buttonsContainer;
            buttonsContainer.Height = Double.NaN;
            Height = Double.NaN;
            titleLabel.Content = title;
        }
    }
}
