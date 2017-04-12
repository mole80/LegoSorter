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

namespace Appl
{
    /// <summary>
    /// Logique d'interaction pour HardwareConfigView.xaml
    /// </summary>
    public partial class HardwareConfigView : UserControl
    {
        public HardwareConfigView()
        {
            InitializeComponent();
        }


        private void Plateau_Click( object sender, RoutedEventArgs e )
        {
            MainWindow.Model.Hard.IsPlateauEnable = !MainWindow.Model.Hard.IsPlateauEnable;
        }

    }
}
