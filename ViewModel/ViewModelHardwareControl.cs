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

using System.IO;


namespace Appl
{
    class ViewModelHardwareControl : BaseControl
    {
        ViewmodelConfigControl _vmConfig;

        public ViewModelHardwareControl( ViewmodelConfigControl vm )
        {
            _vmConfig = vm;
            Sleep = 100;
        }


        #region Cams
        public override void CyclicExecute()
        {
            Image<Bgr, Byte> imgCapture = MainWindow.Model.Capture.QueryFrame().ToImage<Bgr, Byte>();

            Image<Gray, byte> imgHisto;
            var acq = MainWindow.Model.Cam.AquEndConvWindow;
            System.Drawing.Rectangle roi;

            switch ( SelectedTabIndex )
            {
                // Entrée
                case 0:
                    acq = MainWindow.Model.Cam.AquEntryZone;
                    roi = new System.Drawing.Rectangle( acq.PosX, acq.PosY, acq.Width, acq.Height );
                    imgCapture = ImageHelper.RemoveBackGroundColorForAcq( imgCapture, acq );
                    imgHisto = imgCapture.Convert<Gray, byte>();
                    imgHisto.ROI = roi;
                    ImageEntryZone = Fonctions.ToBitmapSource( imgHisto );
                    SizeEntryZone = (int)ImageHelper.GetSize( imgHisto );
                    break;

                // Piece
                case 1:
                    acq = MainWindow.Model.Cam.AquBrickImage;
                    roi = new System.Drawing.Rectangle( acq.PosX, acq.PosY, acq.Width, acq.Height );
                    imgCapture = ImageHelper.RemoveBackGroundColorForAcq( imgCapture, acq );
                    imgHisto = imgCapture.Convert<Gray, byte>();
                    imgHisto.ROI = roi;
                    ImagePiece = Fonctions.ToBitmapSource( imgHisto );
                    SizeImagePiece = (int)ImageHelper.GetSize( imgHisto );
                    break;

                // Sortie
                case 2:
                    acq = MainWindow.Model.Cam.AquEndConvWindow;
                    roi = new System.Drawing.Rectangle( acq.PosX, acq.PosY, acq.Width, acq.Height );
                    imgCapture = ImageHelper.RemoveBackGroundColorForAcq( imgCapture, acq );
                    imgHisto = imgCapture.Convert<Gray, byte>();
                    imgHisto.ROI = roi;
                    ImageEndConv = Fonctions.ToBitmapSource( imgHisto );
                    SizeImageEndConv = (int)ImageHelper.GetSize( imgHisto );
                    break;

                default:
                    break;
            }

        }
        #endregion

        #region Commands
        public override void InitializeCommands()
        {
            CommandStartConvoyeur = new RoutedUICommand( "StartConvoyeur", "StartConvoyeur", typeof( ViewModelHardwareControl ) );
            CommandBinding commandStartConvoyeurBinding = new CommandBinding( CommandStartConvoyeur, StartConvoyeur, CanStartConvoyeur );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandStartConvoyeurBinding );

            CommandConvoyeurStop = new RoutedUICommand( "ConvoyeurStop", "ConvoyeurStop", typeof( ViewModelHardwareControl ) );
            CommandBinding commandConvoyeurStopBinding = new CommandBinding( CommandConvoyeurStop, ConvoyeurStop, CanConvoyeurStop );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandConvoyeurStopBinding );

            CommandPlateauGoTo = new RoutedUICommand( "PlateauGoTo", "PlateauGoTo", typeof( ViewModelHardwareControl ) );
            CommandBinding commandPlateauGoToBinding = new CommandBinding( CommandPlateauGoTo, PlateauGoTo, CanPlateauGoTo );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandPlateauGoToBinding );

            CommandEjectPiece = new RoutedUICommand( "EjectPiece", "EjectPiece", typeof( ViewModelHardwareControl ) );
            CommandBinding commandEjectPieceBinding = new CommandBinding( CommandEjectPiece, EjectPiece, CanEjectPiece );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandEjectPieceBinding );

            CommandStartCam = new RoutedUICommand( "StartCam", "StartCam", typeof( ViewModelHardwareControl ) );
            CommandBinding commandStartCamBinding = new CommandBinding( CommandStartCam, StartCam, CanStartCam );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandStartCamBinding );

            CommandStopCam = new RoutedUICommand( "StopCam", "StopCam", typeof( ViewModelHardwareControl ) );
            CommandBinding commandStopCamBinding = new CommandBinding( CommandStopCam, StopCam, CanStopCam );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandStopCamBinding );
        }
        
        public RoutedUICommand CommandStopCam { get; set; }
        void StopCam( object param, ExecutedRoutedEventArgs e )
        {
            StopThread();
        }

        void CanStopCam( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandStartCam { get; set; }
        void StartCam( object param, ExecutedRoutedEventArgs e )
        {
            StartThread();
        }

        void CanStartCam( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandEjectPiece { get; set; }
        void EjectPiece ( object param, ExecutedRoutedEventArgs e )
        {
            MainWindow.Model.Hard.EjectPiece();
        }

        void CanEjectPiece ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandPlateauGoTo { get; set; }
        void PlateauGoTo ( object param, ExecutedRoutedEventArgs e )
        {
            MainWindow.Model.Hard.MovePlateau( NextPosition );
        }

        void CanPlateauGoTo ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandConvoyeurStop { get; set; }
        void ConvoyeurStop ( object param, ExecutedRoutedEventArgs e )
        {
            MainWindow.Model.Hard.EnableConvoyeur( false );
        }

        void CanConvoyeurStop ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }
        public RoutedUICommand CommandStartConvoyeur { get; set; }
        void StartConvoyeur ( object param, ExecutedRoutedEventArgs e )
        {
            MainWindow.Model.Hard.EnableConvoyeur( true );
        }

        void CanStartConvoyeur ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }
        #endregion


        public const string SizeEntryZonePropertyName = "SizeEntryZone";
        public int SizeEntryZone
        {
            get { return _sizeEntryZone; }
            set
            {
                if ( _sizeEntryZone != value )
                {
                    _sizeEntryZone = value;
                    DoPropertyChanged( SizeEntryZonePropertyName );
                }
            }
        }
        private int _sizeEntryZone;

        public const string SizeImagePiecePropertyName = "SizeImagePiece";
        public int SizeImagePiece
        {
            get { return _sizeImagePiece; }
            set
            {
                if ( _sizeImagePiece != value )
                {
                    _sizeImagePiece = value;
                    DoPropertyChanged( SizeImagePiecePropertyName );
                }
            }
        }
        private int _sizeImagePiece;

        public const string ImagePiecePropertyName = "ImagePiece";
        public BitmapSource ImagePiece
        {
            get { return _imagePiece; }
            set
            {
                if ( _imagePiece != value )
                {
                    _imagePiece = value;
                    DoPropertyChanged( ImagePiecePropertyName );
                }
            }
        }
        private BitmapSource _imagePiece;

        public const string ImageEntryZonePropertyName = "ImageEntryZone";
        public BitmapSource ImageEntryZone
        {
            get { return _imageEntryZone; }
            set
            {
                if ( _imageEntryZone != value )
                {
                    _imageEntryZone = value;
                    DoPropertyChanged( ImageEntryZonePropertyName );
                }
            }
        }
        private BitmapSource _imageEntryZone;

        public const string SizeImageEndConvPropertyName = "SizeImageEndConv";
        public int SizeImageEndConv
        {
            get { return _sizeImageEndConv; }
            set
            {
                if ( _sizeImageEndConv != value )
                {
                    _sizeImageEndConv = value;
                    DoPropertyChanged( SizeImageEndConvPropertyName );
                }
            }
        }
        private int _sizeImageEndConv;

        public const string ImageEndConvPropertyName = "ImageEndConv";
        public BitmapSource ImageEndConv
        {
            get { return _imageEndConv; }
            set
            {
                if ( _imageEndConv != value )
                {
                    _imageEndConv = value;
                    DoPropertyChanged( ImageEndConvPropertyName );
                }
            }
        }
        private BitmapSource _imageEndConv;

        public const string NextPositionPropertyName = "NextPosition";
        public int NextPosition
        {
            get { return _nextPosition; }
            set
            {
                if ( _nextPosition != value )
                {
                    _nextPosition = value;
                    DoPropertyChanged( NextPositionPropertyName );
                }
            }
        }
        private int _nextPosition;


        public const string SelectedTabIndexPropertyName = "SelectedTabIndex";
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if ( _selectedTabIndex != value )
                {
                    _selectedTabIndex = value;
                    DoPropertyChanged( SelectedTabIndexPropertyName );
                }
            }
        }
        private int _selectedTabIndex;
        public Hardware Hard { get { return MainWindow.Model.Hard; } }

    }
}
