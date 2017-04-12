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

using System.Threading;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Drawing;
using Emgu.CV.XFeatures2D;
using Emgu.CV.Util;
using Emgu.CV.Features2D;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

using System.Diagnostics;

namespace Appl
{
    public class ErrorSort
    {
        //static List<ErrorSort> _list = new List<ErrorSort>();

        public ErrorSort( int id, double error )
        {
            Id = id;
            Error = error;
           // _list.Add( this );
        }

        public int Id { get; set; }
        public double Error { get; set; }
    }

    public enum eTypeOfBrick { Unknown=0, Technic=1, TechnicBrick=2, Wheel=3, Window=4, SmallBrick=5, Brick=6, Men=7, VerySmallSpecial=8, SmallSpecial=9, Special=10 }

    public class ImageRef : NotifierComponent
    {
        public ImageRef()
        { }

        public ImageRef( Image<Gray, Byte> img )
        {
            Image = img;
        }                

        public const string NumberOfCirclesPropertyName = "NumberOfCircles";
        public int NumberOfCircles
        {
            get { return _numberOfCircles; }
            set
            {
                if ( _numberOfCircles != value )
                {
                    _numberOfCircles = value;
                    DoPropertyChanged( NumberOfCirclesPropertyName );
                }
            }
        }
        private int _numberOfCircles;
        
        
        public const string NumberOfLinesPropertyName = "NumberOfLines";
        public int NumberOfLines
        {
            get { return _numberOfLines; }
            set
            {
                if ( _numberOfLines != value )
                {
                    _numberOfLines = value;
                    DoPropertyChanged( NumberOfLinesPropertyName );
                }
            }
        }
        private int _numberOfLines;
        

        public const string BrickTypePropertyName = "BrickType";
        public eTypeOfBrick BrickType
        {
            get { return _brickType; }
            set
            {
                if ( _brickType != value )
                {
                    _brickType = value;
                    DoPropertyChanged( BrickTypePropertyName );
                }
            }
        }
        private eTypeOfBrick _brickType;
        

        public const string CaseNumberPropertyName = "CaseNumber";
        public int CaseNumber
        {
            get { return _caseNumber; }
            set
            {
                if ( _caseNumber != value )
                {
                    _caseNumber = value;
                    DoPropertyChanged( CaseNumberPropertyName );
                }
            }
        }
        private int _caseNumber;

        [XmlIgnore]
        public const string VectKeyPointsPropertyName = "VectKeyPoints";
        [XmlIgnore]
        public VectorOfKeyPoint VectKeyPoints
        {
            get { return _vectoKeyPoints; }
            set
            {
                if ( _vectoKeyPoints != value )
                {
                    _vectoKeyPoints = value;
                    DoPropertyChanged( VectKeyPointsPropertyName );
                }
            }
        }
        [XmlIgnore]
        private VectorOfKeyPoint _vectoKeyPoints;

        [XmlIgnore]
        public const string DescriptorPropertyName = "Descriptor";
        [XmlIgnore]
        public UMat Descriptor
        {
            get { return _descriptor; }
            set
            {
                if ( _descriptor != value )
                {
                    _descriptor = value;
                    DoPropertyChanged( DescriptorPropertyName );
                }
            }
        }
        [XmlIgnore]
        private UMat _descriptor;
        

        [XmlIgnore]
        public const string ImagePropertyName = "Image";
        [XmlIgnore]
        public Image<Gray, Byte> Image
        {
            get { return _image; }
            set
            {
                if ( _image != value )
                {
                    _image = value;
                    DoPropertyChanged( ImagePropertyName );
                }
            }
        }
        [XmlIgnore]
        private Image<Gray, Byte> _image;

        [XmlIgnore]
        public const string ResultComparePropertyName = "ResultCompare";
        [XmlIgnore]
        public ResultCompare ResultCompare
        {
            get { return _resultCompare; }
            set
            {
                if ( _resultCompare != value )
                {
                    _resultCompare = value;
                    DoPropertyChanged( ResultComparePropertyName );
                }
            }
        }
        [XmlIgnore]
        private ResultCompare _resultCompare;
        

        public BitmapSource ImageSource
        {
            get
            {
                if ( Image != null )
                    return Fonctions.ToBitmapSource( Image );
                else
                    return Fonctions.ToBitmapSource( new Mat( Model.GetImageFullFileName(this), LoadImageType.Color ) );
            }
        }

        
        public const string SizePropertyName = "Size";
        public int Size
        {
            get { return _size; }
            set
            {
                if ( _size != value )
                {
                    _size = value;
                    DoPropertyChanged( SizePropertyName );
                }
            }
        }
        private int _size;
        
        
        public const string SizesPropertyName = "Sizes";
        public int[] Sizes
        {
            get { return _sizes; }
            set
            {
                if ( _sizes != value )
                {
                    _sizes = value;
                    DoPropertyChanged( SizesPropertyName );
                }
            }
        }
        private int[] _sizes;
        

        public const string IdPropertyName = "Id";
        public int Id
        {
            get { return _id; }
            set
            {
                if ( _id != value )
                {
                    _id = value;
                    DoPropertyChanged( IdPropertyName );
                }
            }
        }
        private int _id;        
        
        public const string ImageFileNamePropertyName = "ImageFileName";
        public string ImageFileName
        {
            get { return _imageFilnename; }
            set
            {
                if ( _imageFilnename != value )
                {
                    _imageFilnename = value;
                    DoPropertyChanged( ImageFileNamePropertyName );
                }
            }
        }
        private string _imageFilnename;        
        
        public const string NamePropertyName = "Name";
        public string Name
        {
            get { return _name; }
            set
            {
                if ( _name != value )
                {
                    _name = value;
                    DoPropertyChanged( NamePropertyName );
                }
            }
        }
        private string _name;        
    }

    public class Parameter : NotifierComponent
    {
        public const string IdPropertyName = "Id";
        public int Id
        {
            get { return _id; }
            set
            {
                if ( _id != value )
                {
                    _id = value;
                    DoPropertyChanged( IdPropertyName );
                }
            }
        }
        private int _id;

        public const string NamePropertyName = "Name";
        public string Name
        {
            get { return _name; }
            set
            {
                if ( _name != value )
                {
                    _name = value;
                    DoPropertyChanged( NamePropertyName );
                }
            }
        }
        private string _name;

        public const string ValuePropertyName = "Value";
        public double Value
        {
            get { return _value; }
            set
            {
                if ( _value != value )
                {
                    _value = value;
                    DoPropertyChanged( ValuePropertyName );
                }
            }
        }
        private double _value;

        public Parameter()
        { }

        public Parameter(int id, string name, double value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

    }

    public class ConfigParametersClass
    {
        [XmlIgnore]
        public ObservableCollection<Parameter> ConfigParametersList { get; set; }

        public Parameter ThresholdIntensityHigh = new Parameter(1, "Seuil intensité max", 240 );
        public Parameter ThresholdIntensityLow = new Parameter(2, "Seuil intensité min", 0 );
        public Parameter SizePieceDetection = new Parameter( 3, "Taille min detection pièce", 1000 );
        public Parameter SizeEntryZoneEmpty = new Parameter( 4, "Taille max pour zone entrée vide", 1000 );
        public Parameter SizeEndConvMinPiecePresent = new Parameter( 5, "Taille min piece presente fin de convoyeur", 5000 );
        public Parameter SizeEndConvMaxEmpty = new Parameter( 6, "Taille max fin de convoyeur vide", 2000 );
        public Parameter DeltaIntensityRemoveBack = new Parameter( 7, "Difference d'intensité pour détection avec le fond", 90 );
        public Parameter IndexCameraNumber = new Parameter(8, "Index de la camera", 1);

        public ConfigParametersClass()
        {
        }
        
        public void Initialize()
        {
            ConfigParametersList = new ObservableCollection<Parameter>();
            ConfigParametersList.Add( ThresholdIntensityHigh );
            ConfigParametersList.Add( ThresholdIntensityLow );
            ConfigParametersList.Add( SizePieceDetection );
            ConfigParametersList.Add( SizeEntryZoneEmpty );
            ConfigParametersList.Add( SizeEndConvMinPiecePresent );
            ConfigParametersList.Add( SizeEndConvMaxEmpty );
            ConfigParametersList.Add( DeltaIntensityRemoveBack );
            ConfigParametersList.Add( IndexCameraNumber );
        }

    }


    public class WindowsAquisition : NotifierComponent
    {
        public const string IsSelectedPropertyName = "IsSelected";
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if ( _isSelected != value )
                {
                    _isSelected = value;
                    DoPropertyChanged( IsSelectedPropertyName );
                }
            }
        }
        private bool _isSelected;

        [XmlIgnore]
        public const string TitlePropertyName = "Title";
        [XmlIgnore]
        public string Title
        {
            get { return _title; }
            set
            {
                if ( _title != value )
                {
                    _title = value;
                    DoPropertyChanged( TitlePropertyName );
                }
            }
        }
        [XmlIgnore]
        private string _title;

        public const string HeightPropertyName = "Height";
        public int Height
        {
            get { return _height; }
            set
            {
                if ( _height != value )
                {
                    _height = value;
                    DoPropertyChanged( HeightPropertyName );
                }
            }
        }
        private int _height;
        
        
        public const string WidthPropertyName = "Width";
        public int Width
        {
            get { return _width; }
            set
            {
                if ( _width != value )
                {
                    _width = value;
                    DoPropertyChanged( WidthPropertyName );
                }
            }
        }
        private int _width;
        
        
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

    public class Cam : NotifierComponent
    {
        Capture _capt;

        public WindowsAquisition AquBrickImage { get; set; }
        public WindowsAquisition AquEndConvWindow { get; set; }
        public WindowsAquisition AquEntryZone { get; set; }

        public Cam()
        {
            AquBrickImage = new WindowsAquisition();            
            AquEndConvWindow = new WindowsAquisition();
            AquEntryZone = new WindowsAquisition();
        }

        public void Initialize(Capture capt)
        {
            AquEndConvWindow.Title = "Zone fin convoyeur";
            AquBrickImage.Title = "Zone piece";
            AquEntryZone.Title = "Zone entrée";

            _capt = capt;
        }

        public const string GammaPropertyName = "Gamma";
        public double Gamma
        {
            get { return _gamma; }
            set
            {
                if ( _gamma != value )
                {
                    _gamma = value;
                    SetParameters();
                    DoPropertyChanged( GammaPropertyName );
                }
            }
        }
        private double _gamma;

        public const string GainPropertyName = "Gain";
        public double Gain
        {
            get { return _gain; }
            set
            {
                if ( _gain != value )
                {
                    _gain = value;
                    SetParameters();
                    DoPropertyChanged( GainPropertyName );
                }
            }
        }
        private double _gain;

        public const string BritghnessPropertyName = "Britghness";
        public double Britghness
        {
            get { return _brightness; }
            set
            {
                if ( _brightness != value )
                {
                    _brightness = value;
                    SetParameters();
                    DoPropertyChanged( BritghnessPropertyName );
                }
            }
        }
        private double _brightness;


        public const string SaturationPropertyName = "Saturation";
        public double Saturation
        {
            get { return _saturation; }
            set
            {
                if ( _saturation != value )
                {
                    _saturation = value;
                    SetParameters();
                    DoPropertyChanged( SaturationPropertyName );
                }
            }
        }
        private double _saturation;

        public const string ContrastPropertyName = "Contrast";
        public double Contrast
        {
            get { return _contrast; }
            set
            {
                if ( _contrast != value )
                {
                    _contrast = value;
                    SetParameters();
                    DoPropertyChanged( ContrastPropertyName );
                }
            }
        }
        private double _contrast;
        

        public const string ExposurePropertyName = "Exposure";
        public double Exposure
        {
            get { return _exposure; }
            set
            {
                if ( _exposure != value )
                {
                    _exposure = value;
                    SetParameters();
                    DoPropertyChanged( ExposurePropertyName );
                }
            }
        }
        private double _exposure;

        public const string FocusPropertyName = "Focus";
        public double Focus
        {
            get { return _focus; }
            set
            {
                if ( _focus != value )
                {
                    _focus = value;
                    SetParameters();
                    DoPropertyChanged( FocusPropertyName );
                }
            }
        }
        private double _focus;
        

        public void ReadParameters()
        {
            Gamma = _capt.GetCaptureProperty( CapProp.Gamma );
            Britghness = _capt.GetCaptureProperty( CapProp.Brightness );
            Gain = _capt.GetCaptureProperty( CapProp.Gain );
            Saturation = _capt.GetCaptureProperty( CapProp.Staturation );
            Exposure = _capt.GetCaptureProperty( CapProp.Exposure );
            Contrast = _capt.GetCaptureProperty( CapProp.Contrast );
            Focus = _capt.GetCaptureProperty( CapProp.Focus );
        }

        public void SetParameters()
        {
            if ( _capt != null )
            {
                _capt.SetCaptureProperty( CapProp.AutoExposure, -1 );
                _capt.SetCaptureProperty( CapProp.Gain, Gain );
                _capt.SetCaptureProperty( CapProp.Gamma, Gamma );
                _capt.SetCaptureProperty( CapProp.Brightness, Britghness );
                _capt.SetCaptureProperty( CapProp.Staturation, Saturation );
                _capt.SetCaptureProperty( CapProp.Exposure, Exposure );
                _capt.SetCaptureProperty( CapProp.Contrast, Contrast );
                _capt.SetCaptureProperty( CapProp.Focus, Focus );
            }
        }
    }


    /** @description Class pour afficher les résultats de comparaison entre deux imaes
     */
    public class ResultCompare : NotifierComponent
    {
        public ResultCompare()
        {
            ResultCompHist = new ObservableCollection<double>();
        }

        public ObservableCollection<double> ResultCompHist { get; set; }

        public const string IdImageModelPropertyName = "IdImageModel";
        public int IdImageModel
        {
            get { return _idImageModel; }
            set
            {
                if ( _idImageModel != value )
                {
                    _idImageModel = value;
                    DoPropertyChanged( IdImageModelPropertyName );
                }
            }
        }
        private int _idImageModel;

        public const string ErrorTotalPropertyName = "ErrorTotal";
        public double ErrorTotal
        {
            get { return _errorTotal; }
            set
            {
                if ( _errorTotal != value )
                {
                    _errorTotal = value;
                    DoPropertyChanged( ErrorTotalPropertyName );
                }
            }
        }
        private double _errorTotal;
        

        public const string ErrorKeyPercentPropertyName = "ErrorKeyPercent";
        public double ErrorKeyPercent
        {
            get { return _errorKeyPercent; }
            set
            {
                if ( _errorKeyPercent != value )
                {
                    _errorKeyPercent = value;
                    DoPropertyChanged( ErrorKeyPercentPropertyName );
                }
            }
        }
        private double _errorKeyPercent;
        

        public const string ErrorSizePropertyName = "ErrorSize";
        public double ErrorSize
        {
            get { return _errorSize; }
            set
            {
                if ( _errorSize != value )
                {
                    _errorSize = value;
                    DoPropertyChanged( ErrorSizePropertyName );
                }
            }
        }
        private double _errorSize;
        

        public const string NumberOfCirclesPropertyName = "NumberOfCircles";
        public int NumberOfCircles
        {
            get { return _numberOfCircles; }
            set
            {
                if ( _numberOfCircles != value )
                {
                    _numberOfCircles = value;
                    DoPropertyChanged( NumberOfCirclesPropertyName );
                }
            }
        }
        private int _numberOfCircles;
        

        public const string NumberOfLinePropertyName = "NumberOfLine";
        public int NumberOfLine
        {
            get { return _numberOfLine; }
            set
            {
                if ( _numberOfLine != value )
                {
                    _numberOfLine = value;
                    DoPropertyChanged( NumberOfLinePropertyName );
                }
            }
        }
        private int _numberOfLine;
        

        public const string IndexOfImageInRefListPropertyName = "IndexOfImageInRefList";
        public int IndexOfImageInRefList
        {
            get { return _indexOfImageInRefList; }
            set
            {
                if ( _indexOfImageInRefList != value )
                {
                    _indexOfImageInRefList = value;
                    DoPropertyChanged( IndexOfImageInRefListPropertyName );
                }
            }
        }
        private int _indexOfImageInRefList;
        

        public const string ImageNamePropertyName = "ImageName";
        public string ImageName
        {
            get { return _imageName; }
            set
            {
                if ( _imageName != value )
                {
                    _imageName = value;
                    DoPropertyChanged( ImageNamePropertyName );
                }
            }
        }
        private string _imageName;
        

        public const string ImageResultPropertyName = "ImageResult";
        public ImageRef ImageResult
        {
            get { return _imageResult; }
            set
            {
                if ( _imageResult != value )
                {
                    _imageResult = value;
                    DoPropertyChanged( ImageResultPropertyName );
                }
            }
        }
        private ImageRef _imageResult;

        public const string ImageCouleurPropertyName = "ImageCouleur";
        public Image<Bgr, byte> ImageCouleur
        {
            get { return _imageCouleur; }
            set
            {
                if ( _imageCouleur != value )
                {
                    _imageCouleur = value;
                    DoPropertyChanged( ImageCouleurPropertyName );
                }
            }
        }
        private Image<Bgr, byte> _imageCouleur;
        

        public const string NumbersOfCountDistPropertyName = "NumbersOfCountDist";
        public int NumbersOfCountDist
        {
            get { return _numberOfCountDist; }
            set
            {
                if ( _numberOfCountDist != value )
                {
                    _numberOfCountDist = value;
                    DoPropertyChanged( NumbersOfCountDistPropertyName );
                }
            }
        }
        private int _numberOfCountDist;
        

        public const string SizeOrientationsMatchesPropertyName = "SizeOrientationsMatches";
        public int SizeOrientationsMatches
        {
            get { return _sizeOrientationsMatches; }
            set
            {
                if ( _sizeOrientationsMatches != value )
                {
                    _sizeOrientationsMatches = value;
                    DoPropertyChanged( SizeOrientationsMatchesPropertyName );
                }
            }
        }
        private int _sizeOrientationsMatches;

        public const string KeyPointsMatchesPropertyName = "KeyPointsMatches";
        public int KeyPointsMatches
        {
            get { return _keyPointsMatches; }
            set
            {
                if ( _keyPointsMatches != value )
                {
                    _keyPointsMatches = value;
                    DoPropertyChanged( KeyPointsMatchesPropertyName );
                }
            }
        }
        private int _keyPointsMatches;

        public const string KeyPointsNumbersPropertyName = "KeyPointsNumbers";
        public int KeyPointsNumbers
        {
            get { return _keyPointsNumbers; }
            set
            {
                if ( _keyPointsNumbers != value )
                {
                    _keyPointsNumbers = value;
                    DoPropertyChanged( KeyPointsNumbersPropertyName );
                }
            }
        }
        private int _keyPointsNumbers;

        public const string NumberMatchOrientationAndDistancePropertyName = "NumberMatchOrientationAndDistance";
        public int NumberMatchOrientationAndDistance
        {
            get { return _numberMatchOrientationAndDistance; }
            set
            {
                if ( _numberMatchOrientationAndDistance != value )
                {
                    _numberMatchOrientationAndDistance = value;
                    DoPropertyChanged( NumberMatchOrientationAndDistancePropertyName );
                }
            }
        }
        private int _numberMatchOrientationAndDistance;
        

    }


    public class Model : NotifierComponent
    {
        public const string FOLDER_IMAGE_REF = "\\ImgRef\\";
        public const string FOLDER_UNKNOW_IMG = "\\UnknowImage\\";
        public const string FOLDER_CONFIG = "\\Config\\";
        public const string FILENAME_CAM_CONFIG = "ConfigCam.xml";
        public const string FILENAME_BRICKS_REF = "Bricks.xml";
        public const string FILENAME_IMG_UNKNOW = "Img_";
        public const string FILENAME_CONF_PARAM = "\\ConfigParameters.xml";

        public Capture Capture { get; set; }
        public Cam Cam { get; set; }

        public Hardware Hard { get; set; }

        public ObservableCollection<ImageRef> BricksRef { get; set; }

        public ImageRef ImageBackground { get; set; }

        public Image<Bgr, byte> BackgroundColor { get; set; }

        public ConfigParametersClass ConfigParameter { get; set; }

        //SURF _surfCpu = new SURF( 50, 4, 2, true, false );
        SIFT _sift = new SIFT( 0, 4, 0.04, 20, 0.8 );
        //SIFT _sift = new SIFT( 0, 4, 0.01, 10, 1.6 );

        public Model()
        {
            ConfigParameter = new ConfigParametersClass();

            ImageBackground = new ImageRef();

            Capture = new Capture( (int)ConfigParameter.IndexCameraNumber.Value );
            ConfigureCapture();

            Cam = new Cam();
            Hard = new Hardware();

            LoadParameters();
            Cam.Initialize( Capture );
            Cam.SetParameters();
            Cam.ReadParameters();

            ConfigParameter.Initialize();

            //CreateRefBase();
        }

        CalculateImageRefWindows _win = new CalculateImageRefWindows();

        public void Initialize()
        {
            _win.Initialize( BricksRef.Count );
            _win.Topmost = true;
            _win.Show();
            Thread t = new Thread( ThreadInitialize );
            t.Start();
        }

        void ThreadInitialize()
        {
            for ( int k = 0; k < BricksRef.Count; k++ )
            {
                BricksRef[k] = CalculMatchingImage(BricksRef[k]);

                /*Application.Current.Dispatcher.Invoke( new Action( delegate
                {
                    BricksRef[k] = CalculMatchingImage( BricksRef[k] );
                } ) );*/

                if (k % 10 == 0)
                {
                    Application.Current.Dispatcher.Invoke(new Action(delegate
                  {
                      _win.SetValue(k);
                  }));
                }
            }

            Application.Current.Dispatcher.Invoke( new Action( delegate
            {
                EndInitializeThread();
            } ) );            
        }

        void EndInitializeThread()
        {
            _win.Close();
            _win = null;
        }

        void ConfigureCapture()
        {
            var a = Capture.GetCaptureProperty( CapProp.FrameHeight );
            a = Capture.GetCaptureProperty( CapProp.FrameWidth );
            a = Capture.GetCaptureProperty( CapProp.FrameCount );
            a = Capture.GetCaptureProperty( CapProp.Fps );

            Capture.SetCaptureProperty( CapProp.FrameHeight, 720 );
            Capture.SetCaptureProperty( CapProp.FrameWidth, 1280 );
        }

        #region Image ref
        public static string GetBackgroundImageFullName()
        {
            return Directory.GetCurrentDirectory() + FOLDER_CONFIG + "ImgBackground.jpg";
        }

        public static string GetImageFullFileName( ImageRef imgRef )
        {
            return Directory.GetCurrentDirectory() + FOLDER_IMAGE_REF + GetImageFileName(imgRef);
        }

        public static string GetImageFileName( ImageRef imgRef )
        {
            return "Img_" + imgRef.Id.ToString() + ".jpg";
        }

        public static string GetImageRefName( ImageRef imgRef )
        {
            return imgRef.BrickType.ToString() + "_" + "_" + imgRef.Id.ToString();
        }

        public void AddImageRef( ImageRef imgRef )
        {
            if ( !BricksRef.Contains( imgRef ) )
            {
                imgRef.Id = BricksRef.Count;
                imgRef.ImageFileName = GetImageFileName( imgRef );
                imgRef.Name = GetImageRefName( imgRef );
                BricksRef.Add( imgRef );
            }
        }

        /** @description Create an image file for each image ref if not exist
         */
        void SaveImgRef()
        {
            foreach ( ImageRef img in BricksRef )
            {
                if ( img.Image != null )
                {
                    if ( !FileExist( GetImageFullFileName( img ) ) )
                        img.Image.Bitmap.Save( GetImageFullFileName( img ) );
                }
            }
        }
        #endregion

        #region File

        bool FileExist( string name )
        {
            FileInfo f = new FileInfo( name );
            return f.Exists;
        }

        string GetFilenameConfig( string filename )
        {
            return Directory.GetCurrentDirectory() + FOLDER_CONFIG + filename;
        }

        public void LoadParameters()
        {
            ImageBackground = new ImageRef( new Image<Gray, byte>( Directory.GetCurrentDirectory() + FOLDER_CONFIG + "ImgBackground.jpg" ) );

            if ( FileExist( GetFilenameConfig( FILENAME_CONF_PARAM ) ) )
            {
                XmlSerializer xs = new XmlSerializer( typeof( ConfigParametersClass ) );
                using ( StreamReader sr = new StreamReader( GetFilenameConfig( FILENAME_CONF_PARAM ) ) )
                {
                    ConfigParameter = xs.Deserialize( sr ) as ConfigParametersClass;
                }
            }

            if ( FileExist( GetFilenameConfig( FILENAME_CAM_CONFIG ) ) )
            {
                XmlSerializer xs = new XmlSerializer( typeof( Cam ) );
                using ( StreamReader sr = new StreamReader( GetFilenameConfig( FILENAME_CAM_CONFIG ) ) )
                {
                    Cam = xs.Deserialize( sr ) as Cam;
                }
            }

            if ( FileExist( GetFilenameConfig( FILENAME_BRICKS_REF ) ) )
            {
                XmlSerializer xs = new XmlSerializer( typeof( ObservableCollection<ImageRef> ) );
                using ( StreamReader sr = new StreamReader( GetFilenameConfig( FILENAME_BRICKS_REF ) ) )
                {
                    BricksRef = xs.Deserialize( sr ) as ObservableCollection<ImageRef>;
                }
            }
            else
                BricksRef = new ObservableCollection<ImageRef>();
        }

        public void SaveParameters()
        {
            XmlSerializer xs = new XmlSerializer( typeof( Cam ) );
            string file = GetFilenameConfig( FILENAME_CAM_CONFIG );
            using ( StreamWriter wr = new StreamWriter( file ) )
            {
                xs.Serialize( wr, Cam );
            }

            xs = new XmlSerializer( typeof( ObservableCollection<ImageRef> ) );
            file = GetFilenameConfig( FILENAME_BRICKS_REF );
            using ( StreamWriter wr = new StreamWriter( file ) )
            {
                xs.Serialize( wr, BricksRef );
            }

            xs = new XmlSerializer( typeof( ConfigParametersClass ) );
            file = GetFilenameConfig( FILENAME_CONF_PARAM );
            using ( StreamWriter wr = new StreamWriter( file ) )
            {
                xs.Serialize( wr, ConfigParameter );
            }

            SaveImgRef();
        }

        #endregion

        //public ImageHelper ImgHelp = new ImageHelper();

        public void CalculImageRefFromList()
        {
            CalculateImageRefWindows win = new CalculateImageRefWindows();

            try
            {                
                win.Initialize( BricksRef.Count );
                win.Topmost = true;
                win.Show();
                for ( int k = 0; k < BricksRef.Count; k++ )
                {
                    BricksRef[k] = CalculMatchingImage( BricksRef[k] );
                    win.SetValue( k );
                }
            }
            catch
            {
            }
            finally
            {
                win.Close();    
            }
        }

        /** @description Calcul les paramètres de comparaison pour une image ref
         */
        public ImageRef CalculMatchingImage( ImageRef img )
        {
            Stopwatch sw1 = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            Stopwatch sw3 = new Stopwatch();

            sw1.Restart();

            System.Threading.Thread.Sleep( 5 );
            img.Image = new Image<Gray, byte>( GetImageFullFileName( img ) );
            //img.Image = ImageHelper.RemoveNoise( img.Image );
            //img.ImageFileName = filename;
            //ImageHelper.ResizeImageRef( img );

            sw2.Restart();
            UMat surfDescriptorRef = new UMat();
            VectorOfKeyPoint SurfKeyPointRef = new VectorOfKeyPoint();
            _sift.DetectAndCompute( img.Image, null, SurfKeyPointRef, surfDescriptorRef, false );

            img.VectKeyPoints = SurfKeyPointRef;
            img.Descriptor = surfDescriptorRef;
            sw2.Stop();

            sw3.Restart();
            img.NumberOfCircles = GetNumberOfCircles( img );
            img.NumberOfLines = GetNumberOfLines( img );
            sw3.Stop();
            sw1.Stop();

            return img;
        }



        public ImageRef TestImage( ImageRef img )
        {            
            img.Size = (int)ImageHelper.GetSize( img.Image );

            img.NumberOfLines = GetNumberOfLines( img, false );
            img.NumberOfCircles = GetNumberOfCircles( img, false );

            if ( img.Size < 10000 )
            {
                img.BrickType = eTypeOfBrick.Special;
            }
            else if ( img.Size < 40000 )
            {

            }
            else
            {

            }

            return img;
        }


        public int GetNumberOfCircles( ImageRef img, bool plot = false )
        {
            //double cannyThreshold = 18.0;
            double cannyThreshold = 100.0;
            double circleAccumulatorThreshold = 120;
            CircleF[] circles = CvInvoke.HoughCircles(
                img.Image, 
                HoughType.Gradient, 
                2.0,                        // dp
                100.0,                       // min dist  
                cannyThreshold,             // param 1 100
                circleAccumulatorThreshold, // param 2 100
                15,                          // min radius
                0);                         // max radius
            return circles.Length;
        }

        public int GetNumberOfLines( ImageRef img, bool plot = false )
        {
            int nbr = 0;
            double cannyThreshold = 20.0;
            double cannyThresholdLinking = 120.0;
                UMat cannyEdges = new UMat();
                CvInvoke.Canny( img.Image, cannyEdges, cannyThreshold, cannyThresholdLinking );

                LineSegment2D[] lines = CvInvoke.HoughLinesP(
                   cannyEdges,
                   1, //Distance resolution in pixel-related units
                   Math.PI / 45.0, //Angle resolution measured in radians.
                   20, //threshold
                   30, //min Line width
                   50 ); //gap between lines


                using ( VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint() )
                {
                    CvInvoke.FindContours( cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple );
                    int count = contours.Size;
                    for ( int i = 0; i < count; i++ )
                    {
                        using ( VectorOfPoint contour = contours[i] )
                        using ( VectorOfPoint approxContour = new VectorOfPoint() )
                        {
                            CvInvoke.ApproxPolyDP( contour, approxContour, CvInvoke.ArcLength( contour, true ) * 0.05, true );

                            if ( plot )
                            {
                                for ( int k = 0; k < approxContour.Size; k++ )
                                {
                                    img.Image.Draw( new CircleF( approxContour[k], 10 ), new Gray( 125 ), 2 );
                                }
                            }

                            double ar = CvInvoke.ContourArea( approxContour, false );
                            if ( ar > 50 && nbr < approxContour.Size )
                                nbr = approxContour.Size;
                        }
                    }
                }            
                return nbr;
          }

        public void TestMatch( ImageRef imgAtester, ImageRef imgModel )
        {
            Image<Gray, byte> t = imgModel.Image;//.Resize( 0.5, Inter.Nearest );
            Image<Gray, float> im = imgAtester.Image.MatchTemplate( t, TemplateMatchingType.CcoeffNormed );

            double[] minValues, maxValues;
            System.Drawing.Point[] minLocations, maxLocations;

            im.MinMax( out minValues, out maxValues, out minLocations, out maxLocations );

            if ( maxValues[0] > max )
            {
                max = maxValues[0];
                res = imgModel.Name;
            }

            // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
            if ( maxValues[0] > 0.9 )
            {
                int a = 0;
                // This is a match. Do something with it, for example draw a rectangle around it.
                //Rectangle match = new Rectangle( maxLocations[0], template.Size );
                //imageToShow.Draw( match, new Bgr( Color.Red ), 3 );
            }


            float[, ,] matches = im.Data;
            for ( int y = 0; y < matches.GetLength( 0 ); y++ )
            {
                for ( int x = 0; x < matches.GetLength( 1 ); x++ )
                {
                    double matchScore = matches[y, x, 0];
                    if ( matchScore > 0.75 )
                    {
                        var a = x;
                        //Rectangle rect = new Rectangle( new Point( x, y ), new Size( 1, 1 ) );
                        //imgSource.Draw( rect, new Bgr( Color.Blue ), 1 );
                    }

                }
            }
        }


        double max = 0;
        string res = "";
        public void Compare(ImageRef imgAtester, ImageRef imgModel)
        {            
            try
            {
                double cannyThreshold = 18.0;
                double circleAccumulatorThreshold = 120;
                CircleF[] circles = CvInvoke.HoughCircles( imgAtester.Image, HoughType.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5 );
                List<Triangle2DF> triangleList = new List<Triangle2DF>();
                List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle

                double cannyThresholdLinking = 120.0;
                UMat cannyEdges = new UMat();
                CvInvoke.Canny( imgAtester.Image, cannyEdges, cannyThreshold, cannyThresholdLinking );

                LineSegment2D[] lines = CvInvoke.HoughLinesP(
                   cannyEdges,
                   1, //Distance resolution in pixel-related units
                   Math.PI / 45.0, //Angle resolution measured in radians.
                   20, //threshold
                   30, //min Line width
                   10 ); //gap between lines

                
                using ( VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint() )
                {
                    
                    CvInvoke.FindContours( cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple );
                    int count = contours.Size;
                    for ( int i = 0; i < count; i++ )
                    {
                        using ( VectorOfPoint contour = contours[i] )
                        using ( VectorOfPoint approxContour = new VectorOfPoint() )
                        {
                            CvInvoke.ApproxPolyDP( contour, approxContour, CvInvoke.ArcLength( contour, true ) * 0.05, true );
                            double ar = CvInvoke.ContourArea( approxContour, false );
                            if ( ar > 250 ) //only consider contours with area greater than 250
                            {
                                if ( approxContour.Size == 3 ) //The contour has 3 vertices, it is a triangle
                                {
                                    /*
                                    Point[] pts = approxContour.ToArray();
                                    triangleList.Add( new Triangle2DF(
                                       pts[0],
                                       pts[1],
                                       pts[2]
                                       ) );*/
                                }
                                else if ( approxContour.Size == 4 ) //The contour has 4 vertices.
                                {
                                    int a = 0;
                                    /*
                                    #region determine if all the angles in the contour are within [80, 100] degree
                                    bool isRectangle = true;
                                    Point[] pts = approxContour.ToArray();
                                    LineSegment2D[] edges = PointCollection.PolyLine( pts, true );

                                    for ( int j = 0; j < edges.Length; j++ )
                                    {
                                        double angle = Math.Abs(
                                           edges[( j + 1 ) % edges.Length].GetExteriorAngleDegree( edges[j] ) );
                                        if ( angle < 80 || angle > 100 )
                                        {
                                            isRectangle = false;
                                            break;
                                        }
                                    }
                                    #endregion

                                    if ( isRectangle ) boxList.Add( CvInvoke.MinAreaRect( approxContour ) );
                                     */
                                }
                            }
                        }
                    }
                }


                /*
                UMat gfttDescriptorImageAtester = new UMat();
                VectorOfKeyPoint gfttKeyPointImageAtester = new VectorOfKeyPoint();
                UMat gfttDescriptorImageModel = new UMat();
                VectorOfKeyPoint gfttKeyPointImageModel = new VectorOfKeyPoint();
                GFTTDetector det = new GFTTDetector( 10000, 0.1, 1, 3, false, 0.04 );
                det.DetectAndCompute( imgAtester.Image, null, gfttKeyPointImageAtester, gfttDescriptorImageAtester, false );
                det.DetectAndCompute( imgModel.Image, null, gfttKeyPointImageModel, gfttDescriptorImageModel, false );
                */
            }
            catch
            {
                int a = 0;
            }
        }


        double[] CompareHyst( ImageRef model, ImageRef aTester )
        {
            // HS hystogram H : hue, S : Saturation
            int h_bins = 50;
            int s_bins = 40;
            DenseHistogram hist1 = new DenseHistogram( 256, new RangeF( 0.0f, 255.0f ) );
            DenseHistogram hist2 = new DenseHistogram( 256, new RangeF( 0.0f, 255.0f ) );
            hist1.Calculate<byte>( new Image<Gray, Byte>[] { model.Image }, true, null );
            hist2.Calculate<byte>( new Image<Gray, Byte>[] { aTester.Image }, true, null );

            double[] comp = new double[5];
            comp[0] = CvInvoke.CompareHist( hist1, hist2, HistogramCompMethod.Bhattacharyya );  // perfect 0 mismatch 1
            comp[1] = CvInvoke.CompareHist( hist1, hist2, HistogramCompMethod.Chisqr );         // low better, perfect 0
            comp[2] = CvInvoke.CompareHist( hist1, hist2, HistogramCompMethod.ChisqrAlt );
            comp[3] = CvInvoke.CompareHist( hist1, hist2, HistogramCompMethod.Correl );         //-1 -> 1 0 no correlation -1 mauvais 1 très bon
            comp[4] = CvInvoke.CompareHist( hist1, hist2, HistogramCompMethod.Intersect );      //0 mismatch perfect 1

            return comp;
        }

        // Query A tester
        public ResultCompare CheckImageMatching( ImageRef imageModel, ImageRef imageAtester, bool drawResult = false )
        {
            Stopwatch sw1 = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            Stopwatch sw3 = new Stopwatch();
            Stopwatch sw4 = new Stopwatch();

            ResultCompare resComp = new ResultCompare();

            try
            {                
                Mat mask = new Mat();
                
                UMat surfDescriptorImageAtester = new UMat();
                Mat surfMask = new Mat();
                Mat surfHomography = new Mat();
                VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
                VectorOfKeyPoint SurfKeyPointImageAtester = new VectorOfKeyPoint();

                sw1.Restart();
                sw2.Restart();
                //var s = _surfCpu.DescriptorSize;
                //_surfCpu.DetectAndCompute( imageAtester.Image, null, SurfKeyPointImageAtester, surfDescriptorImageAtester, false );                
                _sift.DetectAndCompute( imageAtester.Image, null, SurfKeyPointImageAtester, surfDescriptorImageAtester, false );
                sw2.Stop();

                sw3.Restart();
                BFMatcher matcher = new BFMatcher( DistanceType.L2 );
                matcher.Add( imageModel.Descriptor );
                matcher.KnnMatch( surfDescriptorImageAtester, matches, 1, null );
                sw3.Stop();

                //Emgu.CV.Flann.Index flannIndex = new Emgu.CV.Flann.Index(
                //VectorOfVectorOfDMatch flann1 = new VectorOfVectorOfDMatch();
                //Emgu.CV.Flann.KdTreeIndexParamses fi = new Emgu.CV.Flann.KdTreeIndexParamses(4);
                //Emgu.CV.Flann.Index flann = new Emgu.CV.Flann.Index( imageModel.Descriptor, fi );
                //flann.KnnSearch( surfDescriptorImageAtester, matches, flann1, 2, 24 );

                //TestMatch( imageAtester, imageModel );

                surfMask = new Mat( matches.Size, 1, DepthType.Cv8U, 1 );
                surfMask.SetTo( new MCvScalar( 0 ) );

                bool[] indQuery = new bool[surfDescriptorImageAtester.Rows];
                bool[] indTrain = new bool[imageModel.Descriptor.Rows];

                List<DMatchFinder> dMatch = new List<DMatchFinder>();
                for( int k =0; k< matches.Size; k++)
                {
                    for ( int v = 0; v < matches[k].Size; v++ )
                    {
                        dMatch.Add( new DMatchFinder(matches[k][v],k,v) );
                    }
                }

                dMatch = dMatch.OrderBy( o => o.Match.Distance ).ToList();

                double factorDist = 2;
                int maxResult = 50;
                int nbrResult = 0;
                byte[] tabMask = new byte[matches.Size];
                for ( int k = 0; k < dMatch.Count; k++ )
                {
                    int indQ = dMatch[k].Match.QueryIdx;
                    int indT = dMatch[k].Match.TrainIdx;

                    if ( !indQuery[indQ] && !indTrain[indT] && tabMask[dMatch[k].Ind0] == 0 &&
                        nbrResult < maxResult &&
                        dMatch[0].Match.Distance * factorDist <= dMatch[k].Match.Distance )
                    {
                        nbrResult++;
                        indQuery[indQ] = true;
                        indTrain[indT] = true;
                        tabMask[dMatch[k].Ind0] = 255;
                    }
                }

                surfMask.SetTo( tabMask );

                 /*
                var e = matches[0];
                int count = 0;
                List<int> indMatch = new List<int>();
                for ( int k = 0; k < matches.Size; k++ )
                {
                    float d1 = matches[k][0].Distance;
                    float d2 = matches[k][1].Distance;
                    if ( matches[k][0].Distance < 0.9 * matches[k][1].Distance )
                    {
                        //trainId[matches[k][1].TrainIdx].Add( matches[k][0].QueryIdx );
                        bb[k] = 255;
                        indMatch.Add( k );
                        count++;
                    }
                }*/

                int surfNonZeroCountSizeAndOrientation = 0;
                int surfNonZeroCount = CvInvoke.CountNonZero( surfMask );
                if ( surfNonZeroCount >= 4 )
                {
                    sw4.Restart();
                    surfNonZeroCountSizeAndOrientation = Features2DToolbox.VoteForSizeAndOrientation( imageModel.VectKeyPoints, SurfKeyPointImageAtester, matches, surfMask, 1.5, 20 );
                    sw4.Stop();
                }

                double[] tab = CompareHyst( imageModel, imageAtester );
                foreach ( double res in tab )
                    resComp.ResultCompHist.Add( res );

                resComp.KeyPointsNumbers = matches.Size;
                resComp.NumbersOfCountDist = nbrResult;
                resComp.NumberMatchOrientationAndDistance = surfNonZeroCountSizeAndOrientation;                
                resComp.NumberOfCircles = imageModel.NumberOfCircles;
                resComp.NumberOfLine = imageModel.NumberOfLines;

                resComp.ErrorSize = Math.Abs( (double)( imageAtester.Size - imageModel.Size ) / imageModel.Size );
                resComp.ErrorKeyPercent = Math.Abs( (double)( matches.Size - surfNonZeroCountSizeAndOrientation ) / matches.Size );

                resComp.ErrorTotal = (resComp.ErrorKeyPercent + resComp.ErrorSize * 10 + resComp.ResultCompHist[0]) * 100;
                resComp.IdImageModel = imageModel.Id;

                //Not plot
                resComp.ImageName = imageModel.Name;
                resComp.KeyPointsMatches = surfNonZeroCount;
                resComp.SizeOrientationsMatches = CvInvoke.CountNonZero( surfMask );
                sw1.Stop();

                if ( drawResult )
                {
                    Mat surfResult = new Mat();
                    Features2DToolbox.DrawMatches( imageModel.Image, imageModel.VectKeyPoints, imageAtester.Image, SurfKeyPointImageAtester, matches, surfResult, new MCvScalar( 125, 125, 255 ), new MCvScalar( 255, 125, 125 ), surfMask );
                    resComp.ImageCouleur = surfResult.ToImage<Bgr, byte>();

                    resComp.NumberOfCircles = GetNumberOfCircles( imageAtester, false );
                    resComp.NumberOfLine = GetNumberOfLines( imageAtester, false );
                }                
            }
            catch
            {
                int a = 0;
            }

            return resComp;
        }

        public ResultCompare CompareImage( ImageRef imgTest, ImageRef imgModel )
        {
            ResultCompare resComp = new ResultCompare();

            try
            {
                double uniquenessThreshold = 0.8;

                Mat mask = new Mat();
                Mat homography = null;

                bool resize = false;

                Image<Gray, Byte> imgRef_col = imgModel.Image;
                Image<Gray, Byte> imgTest_col = imgTest.Image;

                if ( resize )
                {
                    imgRef_col = imgModel.Image.Resize( 1, Inter.Nearest );
                    imgTest_col = imgTest.Image.Resize( 1, Inter.Nearest );
                }

                UMat surfDescriptorRef = new UMat();
                UMat surfDescriptorTest = new UMat();
                Mat surfMask = new Mat();
                Mat surfHomography = new Mat();
                VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
                VectorOfKeyPoint SurfKeyPointRef = new VectorOfKeyPoint();
                VectorOfKeyPoint SurfKeyPointTest = new VectorOfKeyPoint();
                //_surfCpu.DetectAndCompute( imgRef_col, null, SurfKeyPointRef, surfDescriptorRef, false );
                //_surfCpu.DetectAndCompute( imgTest_col, null, SurfKeyPointTest, surfDescriptorTest, false );
                BFMatcher matcher = new BFMatcher( DistanceType.L2 );
                
                matcher.Add( surfDescriptorRef );
                matcher.KnnMatch( surfDescriptorTest, matches, 2, null );

                // Pas bon
                //matcher.Add( surfDescriptorTest );
                //matcher.KnnMatch( surfDescriptorRef, matches, 2, null );

                surfMask = new Mat( matches.Size, 1, DepthType.Cv8U, 1 );
                surfMask.SetTo( new MCvScalar( 255 ) );
                Features2DToolbox.VoteForUniqueness( matches, 0.8, surfMask );

                var e = matches[0];
                int count = 0;
                List<int> indMatch = new List<int>();
                for ( int k = 0; k < matches.Size; k++ )
                {
                    float d1 = matches[k][0].Distance;
                    float d2 = matches[k][1].Distance;
                    if ( matches[k][0].Distance < 0.8 * matches[k][1].Distance )
                    {
                        indMatch.Add( k );
                        count++;
                    }
                }

                int surfNonZeroCountSizeAndOrientation = 0;
                int surfNonZeroCount = CvInvoke.CountNonZero( surfMask );
                if ( surfNonZeroCount >= 4 )
                {
                    surfNonZeroCountSizeAndOrientation = Features2DToolbox.VoteForSizeAndOrientation( SurfKeyPointRef, SurfKeyPointTest, matches, surfMask, 1.5, 20 );
                    
                    if ( surfNonZeroCountSizeAndOrientation >= 4 )
                        surfHomography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures( SurfKeyPointRef, SurfKeyPointTest, matches, surfMask, 2 );
                }

                Mat surfResult = new Mat();
                Features2DToolbox.DrawMatches( imgRef_col, SurfKeyPointRef, imgTest_col, SurfKeyPointTest, matches, surfResult, new MCvScalar( 255, 255, 255 ), new MCvScalar( 255, 255, 255 ), surfMask );
                

                /*
                SIFT siftCPU = new SIFT();                
                MKeyPoint[] mKeyPointsRef = siftCPU.Detect( imgRef_col, null );
                MKeyPoint[] mKeyPointsTest = siftCPU.Detect( imgTest_col, null );

                VectorOfKeyPoint VectKeyPointRef = new VectorOfKeyPoint( mKeyPointsRef );
                VectorOfKeyPoint VectKeyPointTest = new VectorOfKeyPoint( mKeyPointsTest );

                Image<Gray, Byte> imageComputeRef = new Image<Gray, byte>( imgRef_col.ToBitmap() );
                Image<Gray, Byte> imageComputeTest = new Image<Gray, byte>( imgTest_col.ToBitmap() );

                siftCPU.Compute( imgRef_col, VectKeyPointRef, imageComputeRef );
                siftCPU.Compute( imgTest_col, VectKeyPointTest, imageComputeTest );


                BFMatcher bf = new Emgu.CV.Features2D.BFMatcher( Emgu.CV.Features2D.DistanceType.L2, false );
                VectorOfVectorOfDMatch nn_matches = new VectorOfVectorOfDMatch();
                bf.Add( imageComputeRef );
                bf.KnnMatch( imageComputeTest, nn_matches, 2, null );
                

                mask = new Mat( nn_matches.Size, 1, DepthType.Cv8U, 1 );
                mask.SetTo( new MCvScalar( 255 ) );
                Features2DToolbox.VoteForUniqueness( nn_matches, uniquenessThreshold, mask );

                //Pour debug
                var e = nn_matches[0];
                int count = 0;
                for ( int k = 0; k < nn_matches.Size; k++ )
                {
                    if ( nn_matches[k][0].Distance < 0.8 * nn_matches[k][1].Distance )
                        count++;
                }

                e = nn_matches[0];
                int count1 = 0;
                for ( int k = 0; k < nn_matches.Size; k++ )
                {
                    if ( nn_matches[k][1].Distance < 0.8 * nn_matches[k][0].Distance )
                        count1++;
                }

                int nonZeroCount_KannMatch = CvInvoke.CountNonZero( mask );
                int nonZeroCount_SizeOrientation = -1;

                if ( nonZeroCount_KannMatch >= 4 )
                {
                    nonZeroCount_SizeOrientation = Features2DToolbox.VoteForSizeAndOrientation( VectKeyPointRef, VectKeyPointTest, nn_matches, mask, 1.5, 20 );
                    if ( nonZeroCount_SizeOrientation >= 4 )
                    {
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures( VectKeyPointRef, VectKeyPointTest, nn_matches, mask, 2 );
                    }
                }

                Image<Gray, Byte> imageRefKey = new Image<Gray, byte>( imgRef_col.ToBitmap() );

                //Image<Bgr, Byte> image = Emgu.CV.Features2D.Features2DToolbox.DrawKeypoints( modelImage, modelKeyPoints, new Bgr( System.Drawing.Color.Red ), Emgu.CV.Features2D.Features2DToolbox.KeypointDrawType.Default );                
                Features2DToolbox.DrawKeypoints( imgRef_col, VectKeyPointRef, imageRefKey, new Bgr( System.Drawing.Color.Red ), Emgu.CV.Features2D.Features2DToolbox.KeypointDrawType.NotDrawSinglePoints );
                //resComp.ImageResult = new ImageRef( imageRefKey );                

                Mat result = new Mat();
                Features2DToolbox.DrawMatches( imgRef_col, VectKeyPointRef, imgTest_col, VectKeyPointTest, nn_matches, result, new MCvScalar( 255, 255, 255 ), new MCvScalar( 255, 255, 255 ), mask );
                */

                resComp.ImageResult = new ImageRef( surfResult.ToImage<Gray,byte>() );
                resComp.NumbersOfCountDist = count;
                resComp.KeyPointsNumbers = matches.Size;
                resComp.KeyPointsMatches = surfNonZeroCount;
                resComp.SizeOrientationsMatches = surfNonZeroCountSizeAndOrientation;
            }
            catch
            {
                int a = 0;
            }

            return resComp;
        }

        public ResultCompare FoundImageRef( ImageRef imgTest )
        {
            ResultCompare result = new ResultCompare();

            return result;
        }

        public void SaveImageInFile( Image<Gray, Byte> img )
        {
            DirectoryInfo dir = new DirectoryInfo( Directory.GetCurrentDirectory() + FOLDER_UNKNOW_IMG );

            FileInfo[] files = dir.GetFiles();
            int num = -1;

            foreach ( FileInfo f in files )
            {
                if ( f.Name.Contains( FILENAME_IMG_UNKNOW ) )
                {
                    try
                    {
                        string tmp = f.Name.Split('_')[1];
                        tmp = tmp.Split('.')[0];
                        int val = int.Parse(tmp);
                        if ( val > num )
                            num = val;
                    }
                    catch
                    {}
                }
            }

            img.Bitmap.Save( Directory.GetCurrentDirectory() + FOLDER_UNKNOW_IMG + FILENAME_IMG_UNKNOW + (num+1).ToString() + ".jpg" );
        }

    }
}
