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
using System.Windows.Shapes;

namespace Appl
{
    /// <summary>
    /// Logique d'interaction pour CalculateImageRef.xaml
    /// </summary>
    public partial class CalculateImageRefWindows : Window
    {
        public CalculateImageRefWindows()
        {
            InitializeComponent();
        }

        public void Initialize( int nbrTotal )
        {
            ProgBar.Maximum = nbrTotal;
            ProgBar.Value = 0;
        }

        public void SetValue( int nbr )
        {
            ProgBar.Value = nbr;
        }
    }
}
