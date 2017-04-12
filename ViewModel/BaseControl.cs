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
    public class BaseControl : NotifierComponent
    {

        public const string TimeExecuteCiclicPropertyName = "TimeExecuteCiclic";
        public TimeSpan TimeExecuteCiclic
        {
            get { return _timeExecuteCyclic; }
            set
            {
                if ( _timeExecuteCyclic != value )
                {
                    _timeExecuteCyclic = value;
                    DoPropertyChanged( TimeExecuteCiclicPropertyName );
                }
            }
        }
        private TimeSpan _timeExecuteCyclic;

        static ObservableCollection<BaseControl> ListOfControls { get; set; }

        public int Sleep { get; set; }

        public bool IsActive { get; set; }
        public bool ThreadInProgress { get; set; }
        public bool ThreadInPause { get; set; }
        public Thread ThreadExecute { get; set; }

        public virtual void Start() { }
        public virtual void Stop() { }
        public virtual void Pause() { }
        public virtual void Close() { }

        public virtual void CyclicExecute() { }
        public virtual void InitializeCommands() { }

        public BaseControl()
        {
            if( ListOfControls == null )
                ListOfControls = new ObservableCollection<BaseControl>();

            ListOfControls.Add( this );

            Sleep = 500;
            ThreadInProgress = false;
            ThreadInPause = false;
            ThreadExecute = new Thread( InternalCyclicExecute );

            InitializeCommands();
        }

        public void StartThread()
        {
           ThreadExecute = new Thread( InternalCyclicExecute );

            if ( !ThreadExecute.IsAlive )
            {
                ThreadInProgress = true;
                ThreadExecute.Start();
            }
        }

        public void StopThread()
        {
            if ( ThreadExecute != null && ThreadExecute.IsAlive )
            {
                ThreadInProgress = false;
                ThreadExecute.Join( 1000 );
            }
        }

        public void InternalCyclicExecute()
        {
            System.Diagnostics.Stopwatch sw = new Stopwatch();

            while ( ThreadInProgress )
            {
                if ( !ThreadInPause )
                {
                    sw.Restart();
                    Application.Current.Dispatcher.Invoke( new Action( delegate { CyclicExecute(); } ) );
                    sw.Stop();
                    TimeExecuteCiclic = sw.Elapsed;
                }

                Thread.Sleep( Sleep );
            }
        }
    }
}
