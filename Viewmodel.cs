using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    class Viewmodel : NotifierComponent
    {
        enum eControl { Program, Config }

        public Viewmodel()
        {
            SetControl(eControl.Program);

            InitializeCommands();

        }

        public void Close()
        {
            MainWindow.Model.Hard.Close();
        }

        #region Commands
        void InitializeCommands()
        {
            CommandBinding commandSaveBinding = new CommandBinding( ApplicationCommands.Save, Save, CanSave );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandSaveBinding );

            CommandBinding commandSaveAsBinding = new CommandBinding( ApplicationCommands.SaveAs, SaveAs, CanSaveAs );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandSaveAsBinding );

            CommandBinding commandLoadBinding = new CommandBinding( ApplicationCommands.Open, Load, CanLoad );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandLoadBinding );

            CommandControlProgram = new RoutedUICommand( "ControlProgram", "ControlProgram", typeof( Viewmodel ) );
            CommandBinding commandControlProgramBinding = new CommandBinding( CommandControlProgram, SetControlProgram, CanSetControlProgram );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandControlProgramBinding );

            CommandControlConfig = new RoutedUICommand( "ControlConfig", "ControlConfig", typeof( Viewmodel ) );
            CommandBinding commandControlConfigBinding = new CommandBinding( CommandControlConfig, SetControlConfig, CanSetControlConfig );
            CommandManager.RegisterClassCommandBinding( typeof( UIElement ), commandControlConfigBinding );
        }

        public RoutedUICommand CommandControlConfig { get; set; }
        void SetControlConfig ( object param, ExecutedRoutedEventArgs e )
        {
            SetControl( eControl.Config );
        }

        void CanSetControlConfig ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }
        
        public RoutedUICommand CommandControlProgram { get; set; }
        void SetControlProgram ( object param, ExecutedRoutedEventArgs e )
        {
            SetControl( eControl.Program );
        }

        void CanSetControlProgram ( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        void Load( object param, ExecutedRoutedEventArgs e )
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Budget File | *.bgd";
            dialog.InitialDirectory = Directory.GetCurrentDirectory();

            System.Windows.Forms.DialogResult res = dialog.ShowDialog();

            if ( res == System.Windows.Forms.DialogResult.OK )
            {
                FileInfo f = new FileInfo( dialog.FileName );
                if ( f.Exists )
                {
                }
            }
        }

        void CanLoad( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        void SaveAs( object param, ExecutedRoutedEventArgs e )
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "Budget File | *.bgd";
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            System.Windows.Forms.DialogResult res = dialog.ShowDialog();

            /*
            if ( res == System.Windows.Forms.DialogResult.OK )
            {
                FileInfo f = new FileInfo( dialog.FileName );
                if ( !f.Exists )
                    f.Create();
            }*/
        }

        void CanSaveAs( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        void Save( object param, ExecutedRoutedEventArgs e )
        {
            MainWindow.Model.SaveParameters();
        }

        void CanSave( object param, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = true;
        }

        #endregion

        void SetControl(eControl cont)
        {
            switch ( cont )
            {
                case eControl.Config:
                    MainControl = new ConfigProgram();
                    MainControl.DataContext = new ViewmodelConfigControl();
                    break;

                case eControl.Program:
                    MainControl = new TrieurControl();
                    MainControl.DataContext = new TrieurViewModel();
                    break;
            }            
        }

        public const string MainControlPropertyName = "MainControl";
        public Control MainControl
        {
            get { return _mainControl; }
            set
            {
                if ( _mainControl != value )
                {
                    _mainControl = value;
                    DoPropertyChanged( MainControlPropertyName );
                }
            }
        }
        private Control _mainControl;
        
    }
}
