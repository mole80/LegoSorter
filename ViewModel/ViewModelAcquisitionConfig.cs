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
using System.Collections.ObjectModel;

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

using System.Diagnostics;

namespace Appl
{
    class ViewModelAcquisitionConfig : NotifierComponent
    {
        ViewmodelConfigControl _vmConfig;

        public ViewModelAcquisitionConfig( ViewmodelConfigControl vm )
        {
            _vmConfig = vm;

            InitializeCommands();

            ResultsCompareList = new ObservableCollection<ResultCompare>();

            PlotImageRefControl = new PlotImageRefInfosControl();
            PlotImageRefControl.DataContext = ImageRef;

            PlotImageTestControl = new PlotImageRefInfosControl();
            PlotImageTestControl.DataContext = ImageTested;
            BackSustraction = false;
        }

        #region Commands
        void InitializeCommands()
        {
            CommandGetImageCheck = new RoutedUICommand( "GetImageCheck", "GetImageCheck", typeof( ViewmodelConfigControl ) );
            CommandBinding commandGetImageCheckBinding = new CommandBinding( CommandGetImageCheck, GetImageCheck, CanGetImageCheck );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandGetImageCheckBinding );

            CommandCheckImage = new RoutedUICommand( "CheckImage", "CheckImage", typeof( ViewmodelConfigControl ) );
            CommandBinding commandCheckImageBinding = new CommandBinding( CommandCheckImage, CheckImage, CanCheckImage );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandCheckImageBinding );

            CommandSaveImage = new RoutedUICommand( "SaveImage", "SaveImage", typeof( ViewmodelConfigControl ) );
            CommandBinding commandSaveImageBinding = new CommandBinding( CommandSaveImage, SaveImage, CanSaveImage );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandSaveImageBinding );

            CommandSaveImageRef = new RoutedUICommand( "SaveImageRef", "SaveImageRef", typeof( ViewmodelConfigControl ) );
            CommandBinding commandSaveImageRefBinding = new CommandBinding( CommandSaveImageRef, SaveImageRef, CanSaveImageRef );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandSaveImageRefBinding );

            CommandSetBackgroundImage = new RoutedUICommand( "SetBackgroundImage", "SetBackgroundImage", typeof( ViewmodelConfigControl ) );
            CommandBinding commandSetBackgroundImageBinding = new CommandBinding( CommandSetBackgroundImage, SetBackgroundImage, CanSetBackgroundImage );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandSetBackgroundImageBinding );

            CommandLoadImage = new RoutedUICommand( "LoadImage", "LoadImage", typeof( ViewModelAcquisitionConfig ) );
            CommandBinding commandLoadImageBinding = new CommandBinding( CommandLoadImage, LoadImage, CanLoadImage );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandLoadImageBinding );

            CommandSortResult = new RoutedUICommand( "SortResult", "SortResult", typeof( ViewModelAcquisitionConfig ) );
            CommandBinding commandSortResultBinding = new CommandBinding( CommandSortResult, SortResult, CanSortResult );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandSortResultBinding );
        }
       

        public RoutedUICommand CommandSortResult { get; set; }
        void SortResult ( object param, ExecutedRoutedEventArgs e )
        {
            var p = e.Parameter.ToString();

            List<ResultCompare> l = null;

            switch ( p )
            {
                case "Id":
                    l = ResultsCompareList.OrderBy( o => o.IdImageModel ).ToList();
                    break;

                case "Hist":
                    l = ResultsCompareList.OrderBy( o => o.ResultCompHist[0] ).ToList();
                    break;

                case "Key":
                    l = ResultsCompareList.OrderBy( o => o.NumberMatchOrientationAndDistance ).ToList();
                    break;

                case "Size":
                    l = ResultsCompareList.OrderBy( o => o.ErrorSize ).ToList();
                    break;

                case "Total":
                    l = ResultsCompareList.OrderBy( o => o.ErrorTotal ).ToList();
                    break;

                default:
                    l = ResultsCompareList.OrderBy( o => o.IdImageModel ).ToList();
                    break;
            }
            
            ResultsCompareList.Clear();
            foreach ( ResultCompare c in l )
                ResultsCompareList.Add( c );

        }

        void CanSortResult ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandLoadImage { get; set; }
        void LoadImage ( object param, ExecutedRoutedEventArgs e )
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Image File | *.jpg";
            dialog.InitialDirectory = Directory.GetCurrentDirectory();

            System.Windows.Forms.DialogResult res = dialog.ShowDialog();

            if ( res == System.Windows.Forms.DialogResult.OK )
            {
                FileInfo f = new FileInfo( dialog.FileName );
                if ( f.Exists )
                {
                    ImageRef img = new ImageRef( new Image<Gray, byte>( f.FullName ) );
                    img.Name = "not ref";
                    SetNewTestImage( img, false );
                }
            }
        }

        void CanLoadImage ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandSetBackgroundImage { get; set; }
        void SetBackgroundImage ( object param, ExecutedRoutedEventArgs e )
        {
            var acq = MainWindow.Model.Cam.AquBrickImage;
            Image<Gray, Byte> img = MainWindow.Model.Capture.QueryFrame().ToImage<Gray, Byte>();
            var roi = new System.Drawing.Rectangle( acq.PosX, acq.PosY, acq.Width, acq.Height );
            img.ROI = roi;
            MainWindow.Model.ImageBackground = new ImageRef( img );
        }

        void CanSetBackgroundImage ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        // Ajout l'image a tester dans la liste des images de references
        public RoutedUICommand CommandSaveImageRef { get; set; }
        void SaveImageRef( object param, ExecutedRoutedEventArgs e )
        {
            if ( ImageTested != null )
            {
                MainWindow.Model.AddImageRef( ImageTested );
            }
        }

        void CanSaveImageRef( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        // Save image a tester dans folder unknow en fichier image
        public RoutedUICommand CommandSaveImage { get; set; }
        void SaveImage( object param, ExecutedRoutedEventArgs e )
        {
            MainWindow.Model.SaveImageInFile( ImageTested.Image );
        }

        void CanSaveImage( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandCheckImage { get; set; }
        void CheckImage( object param, ExecutedRoutedEventArgs e )
        {
            bool found = false;

            ResultsCompareList.Clear();

            ResultCompare resultMatch = new ResultCompare();
            ResultCompare result = new ResultCompare();

            ObservableCollection<ImageRef> imageList = MainWindow.Model.BricksRef;

            List<ErrorSort> err = new List<ErrorSort>();

            double min = 1;
            int indMax = 0;
            for ( int k = 0; k < imageList.Count; k++ )
            {
                ImageRef i = imageList[k];
                if ( i.Name != ImageTested.Name ) // Ne pas tester la même image
                {
                    int errLine = Math.Abs( i.NumberOfLines - ImageTested.NumberOfLines );
                    int errCircle = 0;// Math.Abs( i.NumberOfCircles - ImageTested.NumberOfCircles );
                    int errSize = Math.Abs( i.Size - ImageTested.Size );

                    if ( errLine + errCircle <= 1 &&
                        errSize <= 10000 )
                    {
                        result = MainWindow.Model.CheckImageMatching( i, ImageTested );
                        result.IndexOfImageInRefList = k;
                        ResultsCompareList.Add( result );

                        if (    result.ResultCompHist[0] > 0 && 
                                min > result.ErrorTotal &&
                                result.NumberMatchOrientationAndDistance > 8)
                            {
                                found = true;
                                min = result.ErrorTotal;
                                indMax = k;
                                resultMatch = result;
                            }
                    }
                }
            }

            if ( found )
            {
                SelectedResult = resultMatch;
            }

            //resultMatch = MainWindow.Model.CheckImageMatching( imageList[indMax], ImageTested, true );
            //ImageRef = imageList[indMax];
            //ImageCompareLeft = Fonctions.ToBitmapSource( resultMatch.ImageCouleur );
            //Image<Gray, float> ii = ImageTested.Image.MatchTemplate( imageList[indMax].Image.Resize(0.8,Inter.Linear), TemplateMatchingType.Sqdiff );
            //ImageCompareLeft = Fonctions.ToBitmapSource( ii );
        }

        void CompareSelectedResultImage()
        {
            if ( SelectedResult != null )
            {
                ImageRef imRef = MainWindow.Model.BricksRef[SelectedResult.IndexOfImageInRefList];
                ResultCompare resultMatch = MainWindow.Model.CheckImageMatching( imRef, ImageTested, true );
                ImageRef = imRef;                
                if( resultMatch.ImageCouleur != null )
                    ImageCompareLeft = Fonctions.ToBitmapSource( resultMatch.ImageCouleur );
                else
                    ImageCompareLeft = Fonctions.ToBitmapSource( resultMatch.ImageResult.Image );
            }
        }

        void CanCheckImage( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        // Capture une image de la camera
        public RoutedUICommand CommandGetImageCheck { get; set; }
        void GetImageCheck( object param, ExecutedRoutedEventArgs e )
        {
            Stopwatch sw1 = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            Stopwatch sw3 = new Stopwatch();

            //sw3.Restart();
            //sw1.Restart();            
            //double ex = MainWindow.Model.Cam.Exposure;
            ImageRef img = new Appl.ImageRef( MainWindow.Model.Capture.QueryFrame().ToImage<Gray, Byte>() );
            //sw1.Stop();
            //sw2.Restart();
            //MainWindow.Model.Cam.Exposure = ex + 2;
            //sw2.Stop();
            //ImageRef img1 = new Appl.ImageRef( MainWindow.Model.Capture.QueryFrame().ToImage<Gray, Byte>() );
            //MainWindow.Model.Cam.Exposure = ex;
            //sw3.Stop();

            //img.Image = ImageHelper.RemoveNoise( img.Image );
            Image<Gray, byte> back = MainWindow.Model.ImageBackground.Image;

            if ( BackSustraction )
            {
                int width = img.Image.Width;
                int height = img.Image.Height;
                byte[, ,] data = img.Image.Data;
                for ( int indW = 0; indW < width; indW++ )
                {
                    for ( int indH = 0; indH < height; indH++ )
                    {
                        var c = img.Image[indH, indW];
                        var a = img.Image[indH, indW];

                        if( indH < back.Height && indW < back.Width )
                            img.Image.Data[indH, indW, 0] = (byte)(img.Image.Data[indH, indW, 0] - 100);// (byte)back[indH, indW].Intensity);
                    }
                }
            }

            SetNewTestImage( img );
        }

        void CanGetImageCheck( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }
        #endregion


        void SetNewTestImage( ImageRef img, bool userROI = true )
        {
            //Image<Gray, byte> gray_img = img.Image.Convert<Gray, byte>();

            if ( userROI )
            {
                var acq = MainWindow.Model.Cam.AquBrickImage;
                var roi = new System.Drawing.Rectangle( acq.PosX, acq.PosY, acq.Width, acq.Height );
                img.Image.ROI = roi;
            }

            ImageHelper.ResizeImageRef( img );

            PlotImageTestControl.PlotHisto( img.Image );
            ImageTested = MainWindow.Model.TestImage( img );
        }

        public const string BackSustractionPropertyName = "BackSustraction";
        public bool BackSustraction
        {
            get { return _backSubstraction; }
            set
            {
                if ( _backSubstraction != value )
                {
                    _backSubstraction = value;
                    DoPropertyChanged( BackSustractionPropertyName );
                }
            }
        }
        private bool _backSubstraction;
        

        public const string ImageCompareRightPropertyName = "ImageCompareRight";
        public BitmapSource ImageCompareRight
        {
            get { return _imageCompareRight; }
            set
            {
                if ( _imageCompareRight != value )
                {
                    _imageCompareRight = value;
                    DoPropertyChanged( ImageCompareRightPropertyName );
                }
            }
        }
        private BitmapSource _imageCompareRight;
        

        public const string ImageCompareLeftPropertyName = "ImageCompareLeft";
        public BitmapSource ImageCompareLeft
        {
            get { return _imageCompareLeft; }
            set
            {
                if ( _imageCompareLeft != value )
                {
                    _imageCompareLeft = value;
                    DoPropertyChanged( ImageCompareLeftPropertyName );
                }
            }
        }
        private BitmapSource _imageCompareLeft;
        

        public const string ImageTestCheckPropertyName = "ImageTestCheck";
        public BitmapSource ImageTestCheck
        {
            get { return _imageTestCheck; }
            set
            {
                if ( _imageTestCheck != value )
                {
                    _imageTestCheck = value;
                    DoPropertyChanged( ImageTestCheckPropertyName );
                }
            }
        }
        private BitmapSource _imageTestCheck;
        

        public const string ImageTestedPropertyName = "ImageTested";
        public ImageRef ImageTested
        {
            get { return _imageTested; }
            set
            {
                if ( _imageTested != value )
                {
                    _imageTested = value;
                    if ( PlotImageTestControl != null )
                        PlotImageTestControl.DataContext = value;
                    DoPropertyChanged( ImageTestedPropertyName );                    
                }
            }
        }
        private ImageRef _imageTested;


        public const string ImageRefPropertyName = "ImageRef";
        public ImageRef ImageRef
        {
            get { return _imageRef; }
            set
            {
                if ( _imageRef != value )
                {
                    _imageRef = value;
                    if ( PlotImageRefControl != null )
                    {
                        PlotImageRefControl.DataContext = value;
                        PlotImageRefControl.PlotHisto( value.Image );
                    }
                    DoPropertyChanged( ImageRefPropertyName );
                }
            }
        }
        private ImageRef _imageRef;

        public const string SelectedResultPropertyName = "SelectedResult";
        public ResultCompare SelectedResult
        {
            get { return _selectedResult; }
            set
            {
                if ( _selectedResult != value )
                {
                    _selectedResult = value;
                    CompareSelectedResultImage();
                    DoPropertyChanged( SelectedResultPropertyName );
                }
            }
        }
        private ResultCompare _selectedResult;
        
        public ObservableCollection<ResultCompare> ResultsCompareList { get; set; }

        public PlotImageRefInfosControl PlotImageTestControl { get; set; }
        public PlotImageRefInfosControl PlotImageRefControl { get; set; }

    }
}
