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
    public class Position : NotifierComponent
    {
        public const string PosYPropertyName = "PosY";
        public int PosY
        {
            get { return _posY; }
            set
            {
                if ( _posY != value )
                {
                    _posY = value;
                    DoPropertyChanged( PosYPropertyName );
                }
            }
        }
        private int _posY;
        
        public const string PosXPropertyName = "PosX";
        public int PosX
        {
            get { return _posX; }
            set
            {
                if ( _posX != value )
                {
                    _posX = value;
                    DoPropertyChanged( PosXPropertyName );
                }
            }
        }
        private int _posX;
        
    }

    // ViewModel qui contient tout les contrôles pour configurer et tester l'installation
    // ViewModel contient aussi la config de la webCam
    class ViewmodelConfigControl : BaseControl
    {
        #region Properties
        ConfigWebcam _contConfigWebcam = new ConfigWebcam();

        public void ControlVisible()
        {
            if ( Cam.AquEndConvWindow.IsSelected )
                SelectedWindowAquisition = Cam.AquEndConvWindow;
            else
                SelectedWindowAquisition = Cam.AquBrickImage;

            ImageBackground = Fonctions.ToBitmapSource( MainWindow.Model.ImageBackground.Image );
            
        }

        public Cam Cam 
        {
            get { return MainWindow.Model.Cam; }
        }

        public double DeltaIntensity
        {
            get { return MainWindow.Model.ConfigParameter.DeltaIntensityRemoveBack.Value; }
            set { MainWindow.Model.ConfigParameter.DeltaIntensityRemoveBack.Value = value; }
        }

        public Control ConfigWebCamControl 
        {
            get { return _contConfigWebcam; }
        }

        public Control ConfigAcquisitionControl
        {
            get 
            {
                var cont = new AcquisitionConfig();
                cont.DataContext = new ViewModelAcquisitionConfig(this);
                return cont; 
            }
        }

        public Control GestionImageRefControl
        {
            get 
            { 
                var cont = new GestionImageRef(); 
                cont.DataContext = new ViewModelGestionImageRef(this);
                return cont;
            }
        }

        public Control GestionHardwareControl
        {
            get
            {
                var cont = new HardwareConfigView();
                cont.DataContext = new ViewModelHardwareControl( this );
                return cont;
            }
        }

        public Control GestionConfigParametersControl
        {
            get
            {
                var cont = new ConfigParametersView();
                cont.DataContext = new ViewModelConfigParameters( this );
                return cont;
            }
        }

        public const string SelectedWindowAquisitionPropertyName = "SelectedWindowAquisition";
        public WindowsAquisition SelectedWindowAquisition
        {
            get { return _selectedWindowAquisition; }
            set
            {
                if ( _selectedWindowAquisition != value )
                {                    
                    _selectedWindowAquisition = value;
                    DoPropertyChanged( SelectedWindowAquisitionPropertyName );
                }
            }
        }
        private WindowsAquisition _selectedWindowAquisition;

        public const string GrayImagePropertyName = "GrayImage";
        public bool GrayImage
        {
            get { return _grayImage; }
            set
            {
                if ( _grayImage != value )
                {
                    _grayImage = value;
                    DoPropertyChanged( GrayImagePropertyName );
                }
            }
        }
        private bool _grayImage;
        

        public const string CursorPositionImagePropertyName = "CursorPositionImage";
        public Position CursorPositionImage
        {
            get { return _cursorPositionImage; }
            set
            {
                if ( _cursorPositionImage != value )
                {
                    _cursorPositionImage = value;
                    DoPropertyChanged( CursorPositionImagePropertyName );
                }
            }
        }
        private Position _cursorPositionImage;
        
        
        public const string CursorPositionCanvasPropertyName = "CursorPositionCanvas";
        public Position CursorPositionCanvas
        {
            get { return _cursorPositionCanvas; }
            set
            {
                if ( _cursorPositionCanvas != value )
                {
                    _cursorPositionCanvas = value;
                    DoPropertyChanged( CursorPositionCanvasPropertyName );
                }
            }
        }
        private Position _cursorPositionCanvas;


        public const string UseFilterPropertyName = "UseFilter";
        public bool UseFilter
        {
            get { return _useFilter; }
            set
            {
                if ( _useFilter != value )
                {
                    _useFilter = value;
                    DoPropertyChanged( UseFilterPropertyName );
                }
            }
        }
        private bool _useFilter;

        public const string UseBackgroundPropertyName = "UseBackground";
        public bool UseBackground
        {
            get { return _useBackground; }
            set
            {
                if ( _useBackground != value )
                {
                    _useBackground = value;
                    DoPropertyChanged( UseBackgroundPropertyName );
                }
            }
        }
        private bool _useBackground;

        public const string ImageBackgroundPropertyName = "ImageBackground";
        public BitmapSource ImageBackground
        {
            get { return _imageBAckground; }
            set
            {
                if ( _imageBAckground != value )
                {
                    _imageBAckground = value;
                    DoPropertyChanged( ImageBackgroundPropertyName );
                }
            }
        }
        private BitmapSource _imageBAckground;

        public const string ImageWebSourcePropertyName = "ImageWebSource";
        public BitmapSource ImageWebSource
        {
            get { return _imageWebSource; }
            set
            {
                if ( _imageWebSource != value )
                {
                    _imageWebSource = value;
                    DoPropertyChanged( ImageWebSourcePropertyName );
                }
            }
        }
        private BitmapSource _imageWebSource;
        #endregion

        public ViewmodelConfigControl()
        {
            CursorPositionCanvas = new Position();
            CursorPositionImage = new Position();
            //MainWindow.Model.Capture.ImageGrabbed += Capture_ImageGrabbed;
        }

        void Capture_ImageGrabbed( object sender, EventArgs e )
        {
            int a = 0;
        }

        public override void CyclicExecute()
        {
            //            Image<Bgr, Byte> imgCapture = MainWindow.Model.Capture.QueryFrame().ToImage<Bgr, Byte>();
            Image<Bgr, Byte> imgCapture = MainWindow.Model.Capture.QueryFrame().ToImage<Bgr, Byte>();

            Image<Gray, byte> imgHisto = imgCapture.Convert<Gray, byte>();

            if ( GrayImage )
            {
                if( UseBackground )
                {
                    imgHisto = ImageHelper.UseBackGround( imgHisto, MainWindow.Model.ImageBackground );
                }
                ImageWebSource = Fonctions.ToBitmapSource( imgHisto );
            }
            else
            {
                if( UseBackground && MainWindow.Model.BackgroundColor != null )
                {
                    imgCapture = ImageHelper.RemoveBackGroundColor( imgCapture );
                }
                
                if ( UseFilter )
                    imgCapture = imgCapture.SmoothGaussian( 5 );

                ImageWebSource = Fonctions.ToBitmapSource( imgCapture );

            }            
            
            var acq = MainWindow.Model.Cam.AquBrickImage;
            var roi = new System.Drawing.Rectangle( acq.PosX, acq.PosY, acq.Width, acq.Height );
            imgHisto.ROI = roi;

            _contConfigWebcam.PlotHisto( imgHisto );
        }


        public void UpdateCamProperties()
        {
            MainWindow.Model.Cam.SetParameters();
        }



        #region Commands
        public override void InitializeCommands()
        {
            CommandStartWebCam = new RoutedUICommand( "StartWebCam", "StartWebCam", typeof( ViewmodelConfigControl ) );
            CommandBinding commandStartWebCamBinding = new CommandBinding( CommandStartWebCam, StartWebCam, CanStartWebCam );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandStartWebCamBinding );

            CommandStopWebCam = new RoutedUICommand( "StopWebCam", "StopWebCam", typeof( ViewmodelConfigControl ) );
            CommandBinding commandStopWebCamBinding = new CommandBinding( CommandStopWebCam, StopWebCam, CanStopWebCam );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandStopWebCamBinding );

            CommandCaptureBackground = new RoutedUICommand( "CaptureBackground", "CaptureBackground", typeof( ViewmodelConfigControl ) );
            CommandBinding commandCaptureBackgroundBinding = new CommandBinding( CommandCaptureBackground, CaptureBackground, CanCaptureBackground );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandCaptureBackgroundBinding );
        }
        
        public RoutedUICommand CommandCaptureBackground { get; set; }
        void CaptureBackground( object param, ExecutedRoutedEventArgs e )
        {
            Image<Bgr, Byte> imgCapture = MainWindow.Model.Capture.QueryFrame().ToImage<Bgr, Byte>();

            MainWindow.Model.BackgroundColor = imgCapture;

            Image<Gray, byte> imgHisto = imgCapture.Convert<Gray, byte>();
            ImageBackground = Fonctions.ToBitmapSource( imgHisto );
            MainWindow.Model.ImageBackground = new ImageRef( imgHisto );

            imgHisto.Bitmap.Save( Model.GetBackgroundImageFullName() );
        }

        void CanCaptureBackground( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandStopWebCam { get; set; }
        void StopWebCam ( object param, ExecutedRoutedEventArgs e )
        {
            StopThread();
        }

        void CanStopWebCam ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }
        

        public RoutedUICommand CommandStartWebCam { get; set; }
        void StartWebCam ( object param, ExecutedRoutedEventArgs e )
        {
            StartThread();
        }

        void CanStartWebCam ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }
        #endregion

    }
}
