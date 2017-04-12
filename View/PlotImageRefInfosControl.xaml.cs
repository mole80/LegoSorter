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

using System.Runtime.InteropServices;

using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Drawing;

using Emgu.CV.XFeatures2D;
using Emgu.CV.Util;
using Emgu.CV.Features2D;

namespace Appl
{
    /// <summary>
    /// Logique d'interaction pour PlotImageRefInfosControl.xaml
    /// </summary>
    public partial class PlotImageRefInfosControl : UserControl
    {
        public PlotImageRefInfosControl()
        {
            InitializeComponent();
        }

        public void PlotHisto( Image<Gray, Byte> img )
        {
            Image<Gray, Byte> img1 = img[0];

            DenseHistogram Histo = new DenseHistogram( 256, new RangeF( 0.0f, 255.0f ) );
            Histo.Calculate<Byte>( new Image<Gray, Byte>[] { img1 }, true, null );

            float[] t = Histo.GetBinValues();
            float max = 0;
            for ( int k = 0; k < t.Length && k < MainWindow.Model.ConfigParameter.ThresholdIntensityHigh.Value; k++ )
            {
                if ( t[k] > max )
                    max = t[k];
            }

            HistogramBox box = new HistogramBox();
            box.GenerateHistograms( img, 255 );
            box.ZedGraphControl.GraphPane.XAxis.Scale.Max = MainWindow.Model.ConfigParameter.ThresholdIntensityHigh.Value;
            box.ZedGraphControl.GraphPane.YAxis.Scale.Max = max;
            box.Refresh();
            WinHostHistoControl.Child = box;
        }
    }
}
