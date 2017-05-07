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
using System.Threading;

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
    class TrieurViewModel : BaseControl
    {
        public enum eStateSystem { Idle, WaitPiece, PieceGoToTest, CompareImage, SelectionPlace, EjectPieceDetect, EjectPieceEnd, SwitchState }

        public Hardware Hard { get { return Appl.MainWindow.Model.Hard; } }

        public TrieurViewModel()
        {
            StateSystem = eStateSystem.Idle;
            NextState = eStateSystem.Idle;
            SwitchAutoNextState = true;
            PerformNextState = false;
        }

        #region Commands
        public override void InitializeCommands()
        {
            CommandStartSystem = new RoutedUICommand( "StartSystem", "StartSystem", typeof( TrieurViewModel ) );
            CommandBinding commandStartSystemBinding = new CommandBinding( CommandStartSystem, StartSystem, CanStartSystem );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandStartSystemBinding );
            
            CommandStopSystem = new RoutedUICommand( "StopSystem", "StopSystem", typeof( TrieurViewModel ) );
            CommandBinding commandStopSystemBinding = new CommandBinding( CommandStopSystem, StopSystem, CanStopSystem );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandStopSystemBinding );

            CommandSwitchNextState = new RoutedUICommand( "SwitchNextState", "SwitchNextState", typeof( TrieurViewModel ) );
            CommandBinding commandSwitchNextStateBinding = new CommandBinding( CommandSwitchNextState, SwitchNextState, CanSwitchNextState );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandSwitchNextStateBinding );
        }
        
        public RoutedUICommand CommandSwitchNextState { get; set; }
        void SwitchNextState( object param, ExecutedRoutedEventArgs e )
        {
            PerformNextState = true;
        }

        void CanSwitchNextState( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        public RoutedUICommand CommandStopSystem { get; set; }
        void StopSystem ( object param, ExecutedRoutedEventArgs e )
        {
            StopThread();
            StateSystem = eStateSystem.Idle;
            Hard.EnableConvoyeur( false );
        }

        void CanStopSystem ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }
        public RoutedUICommand CommandStartSystem { get; set; }
        void StartSystem ( object param, ExecutedRoutedEventArgs e )
        {
            StartThread();
            StateSystem = eStateSystem.WaitPiece;
            Sleep = 100;
        }

        void CanStartSystem ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }
        #endregion

        void AddLineToLog( string line )
        {
            if ( LogEnable )
            {
                Application.Current.Dispatcher.Invoke( new Action( delegate
                {
                    AddLineToLogIntern( line );
                } ) );
            }
        }

        void AddLineToLogIntern(string line)
        {
            Log += line + "\n";
        }

        #region Program
        public override void CyclicExecute()
        {
            switch ( StateSystem )
            {
                case eStateSystem.Idle:
                    break;

                case eStateSystem.WaitPiece:
                    WaitPieceState();
                    break;

                case eStateSystem.PieceGoToTest:
                    PieceGoToTestState();
                    break;

                case eStateSystem.CompareImage:
                    CompareImageState();
                    break;

                case eStateSystem.SelectionPlace:
                    SelectionPlaceState();
                    break;

                case eStateSystem.EjectPieceDetect:
                    EjectPieceDetectState();
                    break;

                case eStateSystem.EjectPieceEnd:
                    EjectPieceEndState();
                    break;

                case eStateSystem.SwitchState:
                    SwitchAutoState();
                    break;
            }
        }
        #endregion

        void SwitchAutoState()
        {
            if ( SwitchAutoNextState || NextState == eStateSystem.PieceGoToTest || NextState == eStateSystem.EjectPieceEnd )
                StateSystem = NextState;
            else if( PerformNextState )
                StateSystem = NextState;
                
            PerformNextState = false;
        }


        int GetImageAndSize(WindowsAquisition acq)
        {
            Image<Bgr, Byte> imgCapture = MainWindow.Model.Capture.QueryFrame().ToImage<Bgr, Byte>();

            var roi = new System.Drawing.Rectangle( acq.PosX, acq.PosY, acq.Width, acq.Height );

            imgCapture = ImageHelper.RemoveBackGroundColorForAcq( imgCapture, acq );
            Image<Gray, byte> imgHisto = imgCapture.Convert<Gray, byte>();
            imgHisto.ROI = roi;
            ImageAtester = new ImageRef( imgHisto );
            int size = (int)ImageHelper.GetSize( imgHisto );
            return size;
        }

        void WaitPieceState()
        {
            int size = GetImageAndSize( MainWindow.Model.Cam.AquBrickImage );

            if (!Hard.IsConvoyeurEnable)
            {
                size = GetImageAndSize(MainWindow.Model.Cam.AquBrickImage);
                Hard.EnableConvoyeur(true);
            }

            //if ( size > MainWindow.Model.ConfigParameter.SizePieceDetection.Value )
            if( Hard.IsPieceDetected || size > MainWindow.Model.ConfigParameter.SizePieceDetection.Value )
            {
                Hard.EnableConvoyeur(false);
                TextInfos = "Piece trouvée : taille = " + size.ToString();
                AddLineToLog( TextInfos );
                NextState = eStateSystem.PieceGoToTest;
            }
            else
            {
                string t = "Size piece : " + size.ToString() + "  <  " + MainWindow.Model.ConfigParameter.SizePieceDetection.Value;
                TextInfos = t;
            }
        }


        void PieceGoToTestState()
        {
            int size = GetImageAndSize( MainWindow.Model.Cam.AquBrickImage );
            
            if ( size > MainWindow.Model.ConfigParameter.SizePieceDetection.Value )
            {
                TextInfos = "Piece en place pour test : taille = " + size.ToString();
                AddLineToLog( TextInfos );
                Hard.EnableConvoyeur( false );
                NextState = eStateSystem.CompareImage;
                Thread.Sleep( 1000 ); // Wait piece stabilisation
            }
            else
            {
                Hard.EnableConvoyeur(true);
                Thread.Sleep(200);
                Hard.EnableConvoyeur(false);
                Thread.Sleep(800);

                string t = "Size piece : " + size.ToString();
                TextInfos = t;
            }
        }

        void CompareImageState()
        {
            Hard.EnableConvoyeur(false);

            int size = GetImageAndSize( MainWindow.Model.Cam.AquBrickImage );

            // Found image
            ImageAtester = MainWindow.Model.TestImage( ImageAtester );

            ImageAtester.BrickType = eTypeOfBrick.Unknown;

            // Lors d'une erreur, l'image est sauvée
            if ( ImageAtester.BrickType == eTypeOfBrick.Unknown )
            {
                MainWindow.Model.SaveImageInFile( ImageAtester.Image );
            }

            NextState = eStateSystem.SelectionPlace;
        }

        void SelectionPlaceState()
        {
            TextInfos = "Plateau va en : " + ImageAtester.CaseNumber.ToString();
            AddLineToLog( TextInfos );

            Thread.Sleep( 100 );
            //Hard.MovePlateau( ImageAtester.CaseNumber );

            NextState = eStateSystem.EjectPieceDetect;
        }

        void EjectPieceDetectState()
        {
            NextState = eStateSystem.EjectPieceEnd;
            //Plus besoin avec nouveau système à bascule
            /*int size = GetImageAndSize( MainWindow.Model.Cam.AquEndConvWindow );

            if ( !Hard.IsConvoyeurEnable )
                Hard.EnableConvoyeur( true );

            if ( size > MainWindow.Model.ConfigParameter.SizeEndConvMinPiecePresent.Value )
            {
                TextInfos = "Piece en fin de conv : " + size.ToString();
                NextState = eStateSystem.EjectPieceEnd;
                AddLineToLog( TextInfos );
            }
            else
            {
                string t = "   Size end : " + size.ToString();
                t += "   < : " + MainWindow.Model.ConfigParameter.SizeEndConvMinPiecePresent.Value.ToString( "0" );
                TextInfos = t;
            }*/
        }

        void EjectPieceEndState()
        {
            Hard.EjectPiece();
            NextState = eStateSystem.WaitPiece;
            AddLineToLog("End");
            AddLineToLog("");

            //Plus besoin avec nouveau système à bascule
            /*
            int size = GetImageAndSize( MainWindow.Model.Cam.AquEndConvWindow );

            if ( size < MainWindow.Model.ConfigParameter.SizeEndConvMaxEmpty.Value && size > 100 )
            {
                NextState = eStateSystem.WaitPiece;
                AddLineToLog( "End" );
                AddLineToLog( "" );
                Hard.EnableConvoyeur( false );
            }
            else
            {
                string t = "   Size end : " + size.ToString();
                t += "   > : " + MainWindow.Model.ConfigParameter.SizeEndConvMaxEmpty.Value.ToString( "0" );
                TextInfos = t;
            }*/
        }


        #region Properties      

        public const string LogEnablePropertyName = "LogEnable";
        public bool LogEnable
        {
            get { return _logEnable; }
            set
            {
                if ( _logEnable != value )
                {
                    _logEnable = value;
                    DoPropertyChanged( LogEnablePropertyName );
                }
            }
        }
        private bool _logEnable;

        public const string LogPropertyName = "Log";
        public string Log
        {
            get { return _log; }
            set
            {
                if ( _log != value )
                {
                    _log = value;
                    DoPropertyChanged( LogPropertyName );
                }
            }
        }
        private string _log;

        public const string PerformNextStatePropertyName = "PerformNextState";
        public bool PerformNextState
        {
            get { return _performNextState; }
            set
            {
                if ( _performNextState != value )
                {
                    _performNextState = value;
                    DoPropertyChanged( PerformNextStatePropertyName );
                }
            }
        }
        private bool _performNextState;

        public const string SwitchAutoNextStatePropertyName = "SwitchAutoNextState";
        public bool SwitchAutoNextState
        {
            get { return _switchAutoNextState; }
            set
            {
                if ( _switchAutoNextState != value )
                {
                    _switchAutoNextState = value;
                    DoPropertyChanged( SwitchAutoNextStatePropertyName );
                }
            }
        }
        private bool _switchAutoNextState;

        public const string NextStatePropertyName = "NextState";
        public eStateSystem NextState
        {
            get { return _nextState; }
            set
            {
                if ( _nextState != value )
                {
                    StateSystem = eStateSystem.SwitchState;
                    _nextState = value;
                    DoPropertyChanged( NextStatePropertyName );
                }
            }
        }
        private eStateSystem _nextState;

        public const string TextInfosPropertyName = "TextInfos";
        public string TextInfos
        {
            get { return _TextInfos; }
            set
            {
                if ( _TextInfos != value )
                {
                    _TextInfos = value;
                    DoPropertyChanged( TextInfosPropertyName );
                }
            }
        }
        private string _TextInfos;
        

        public const string StateSystemPropertyName = "StateSystem";
        public eStateSystem StateSystem
        {
            get { return _StateSystem; }
            set
            {
                if ( _StateSystem != value )
                {
                    _StateSystem = value;
                    DoPropertyChanged( StateSystemPropertyName );
                }
            }
        }
        private eStateSystem _StateSystem;
        

        public const string ImageAtesterPropertyName = "ImageAtester";
        public ImageRef ImageAtester
        {
            get { return _imageAtester; }
            set
            {
                if ( _imageAtester != value )
                {
                    _imageAtester = value;
                    DoPropertyChanged( ImageAtesterPropertyName );
                }
            }
        }
        private ImageRef _imageAtester;

        public const string ImageRefPropertyName = "ImageRef";
        public ImageRef ImageRef
        {
            get { return _imageRef; }
            set
            {
                if ( _imageRef != value )
                {
                    _imageRef = value;
                    DoPropertyChanged( ImageRefPropertyName );
                }
            }
        }
        private ImageRef _imageRef;
        #endregion
    }
}
