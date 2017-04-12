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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Viewmodel _vm = new Viewmodel();
        public static Model Model { get; private set; }

        public MainWindow()
        {
            Model = new Appl.Model();

            InitializeComponent();
            DataContext = _vm;
        }

        private void Window_Closed( object sender, EventArgs e )
        {
            _vm.Close();
        }

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            Model.Initialize();
        }
    }
}
