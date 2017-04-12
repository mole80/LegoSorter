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
    /// Logique d'interaction pour ConfigWebcam.xaml
    /// </summary>
    public partial class ConfigWebcam : UserControl
    {
        
        bool _changedWinInprogress = false;
        HistogramBox _box = new HistogramBox();

        public ConfigWebcam()
        {
            InitializeComponent();
            WinHostHistoControl.Child = _box;
        }


        public void PlotHisto( Image<Gray,byte> img )
        {
            DenseHistogram Histo = new DenseHistogram( 256, new RangeF( 0.0f, 255.0f ) );
            Histo.Calculate<Byte>( new Image<Gray, Byte>[] { img }, true, null );

            float[] t = Histo.GetBinValues();
            float max = 0;
            for ( int k = 0; k < t.Length && k < MainWindow.Model.ConfigParameter.ThresholdIntensityHigh.Value; k++ )
            {
                if ( t[k] > max )
                    max = t[k];
            }

            _box.ClearHistogram();
            _box.GenerateHistograms( img, 255 );

            _box.ZedGraphControl.GraphPane.XAxis.Scale.Max = MainWindow.Model.ConfigParameter.ThresholdIntensityHigh.Value;
            _box.ZedGraphControl.GraphPane.YAxis.Scale.Max = max;
            _box.Refresh();            
        }


        private void CamParameters_ValueChanged( object sender, RoutedPropertyChangedEventArgs<double> e )
        {
            if ( DataContext != null && DataContext is ViewmodelConfigControl )
            {
                var dc = DataContext as ViewmodelConfigControl;
                //dc.UpdateCamProperties();
            }
        }

        private void Image_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( DataContext != null && DataContext is ViewmodelConfigControl )
            {
                var dc = DataContext as ViewmodelConfigControl;

                if ( sender is System.Windows.Controls.Image )
                {
                    _changedWinInprogress = true;

                    var cont = sender as System.Windows.Controls.Image;

                    var height = MainWindow.Model.Capture.Height;
                    var width = MainWindow.Model.Capture.Width;

                    double posX = ( e.GetPosition( cont ).X / cont.ActualWidth ) * width;
                    double posY = ( e.GetPosition( cont ).Y / cont.ActualHeight ) * height;

                    dc.SelectedWindowAquisition.PosX = (int)posX;
                    dc.SelectedWindowAquisition.PosY = (int)posY;

                    var rect = RectangleWindows;

                    Canvas.SetLeft( rect, e.GetPosition( CanvasRectAqc ).X );
                    Canvas.SetTop( rect, e.GetPosition( CanvasRectAqc ).Y );
                }
            }
        }

        private void Image_MouseMove( object sender, MouseEventArgs e )
        {
            if ( DataContext != null && DataContext is ViewmodelConfigControl )
            {
                var dc = DataContext as ViewmodelConfigControl;

                if ( sender is System.Windows.Controls.Image )
                {
                    var cont = sender as System.Windows.Controls.Image;

                    var height = MainWindow.Model.Capture.Height;
                    var width = MainWindow.Model.Capture.Width;

                    double posX = ( e.GetPosition( cont ).X / cont.ActualWidth ) * width;
                    double posY = ( e.GetPosition( cont ).Y / cont.ActualHeight ) * height;

                    var rect = RectangleWindows;

                    if ( _changedWinInprogress )
                    {
                        rect.Width = Math.Abs( e.GetPosition( CanvasRectAqc ).X - Canvas.GetLeft( rect ) );
                        rect.Height = Math.Abs( e.GetPosition( CanvasRectAqc ).Y - Canvas.GetTop( rect ) );
                    }

                    dc.CursorPositionCanvas.PosX = (int)e.GetPosition( CanvasRectAqc ).X;
                    dc.CursorPositionCanvas.PosY = (int)e.GetPosition( CanvasRectAqc ).Y;
                    dc.CursorPositionImage.PosX = (int)posX;
                    dc.CursorPositionImage.PosY = (int)posY;
                }
            }
        }

        private void Image_MouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {

        }

        private void TabControl_MouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            if ( DataContext != null && DataContext is ViewmodelConfigControl && _changedWinInprogress )
            {
                var dc = DataContext as ViewmodelConfigControl;

                if ( ImageWebCam != null )
                {
                    var cont = ImageWebCam as System.Windows.Controls.Image;

                    var height = MainWindow.Model.Capture.Height;
                    var width = MainWindow.Model.Capture.Width;

                    var rect = RectangleWindows;

                    double widthRect = ( rect.Width / cont.ActualWidth ) * width;
                    double heightRect = ( rect.Height / cont.ActualHeight ) * height;

                    dc.SelectedWindowAquisition.Width = (int)widthRect;
                    dc.SelectedWindowAquisition.Height = (int)heightRect;
                }
            }
            _changedWinInprogress = false;
        }

        private void ContentControl_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( DataContext != null && DataContext is ViewmodelConfigControl )
            {
                var dc = DataContext as ViewmodelConfigControl;
                dc.Cam.AquEndConvWindow.IsSelected = false;
                dc.Cam.AquBrickImage.IsSelected = false;
                dc.Cam.AquEntryZone.IsSelected = false;

                if ( sender is ContentControl )
                {
                    var s = ( sender as ContentControl ).Content;
                    if ( s is WindowsAquisition )
                    {
                        var aq = s as WindowsAquisition;
                        aq.IsSelected = true;
                        dc.SelectedWindowAquisition = aq;
                        UpdateRectangle();
                    }
                }                
            }
        }
        
        void UpdateRectangle()
        {
            if ( DataContext != null && DataContext is ViewmodelConfigControl )
            {
                var dc = DataContext as ViewmodelConfigControl;

                var height = MainWindow.Model.Capture.Height;
                var width = MainWindow.Model.Capture.Width;

                var pxH = ImageWebCam.ActualHeight;
                var pxW = ImageWebCam.ActualWidth;

                var gainX = pxW / width;
                var gainY = pxH / height;

                WindowsAquisition aq = dc.SelectedWindowAquisition;
                RectangleWindows.Width = aq.Width * gainX;
                RectangleWindows.Height = aq.Height * gainY;
                Canvas.SetLeft( RectangleWindows, aq.PosX * gainX );
                Canvas.SetTop( RectangleWindows, aq.PosY * gainY );
            }
        }

        private void UserControl_IsVisibleChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            if ( Visibility == Visibility.Visible )
            {
                if ( DataContext != null && DataContext is ViewmodelConfigControl )
                {
                    var dc = DataContext as ViewmodelConfigControl;
                    dc.ControlVisible();
                }

                UpdateRectangle();
            }
        }

        private void UserControl_Loaded( object sender, RoutedEventArgs e )
        {
            if ( DataContext != null && DataContext is ViewmodelConfigControl )
            {
                var dc = DataContext as ViewmodelConfigControl;
                dc.ControlVisible();
            }

            UpdateRectangle();
        }
    }
}
