using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using FTD2XX_NET;

namespace Appl
{
    public class Hardware : NotifierComponent
    {
        public enum eStateMachine { Idle, Avance, Eject, Plateau }

        Thread _threadCyclic;
        bool _inProgress;

        bool[] ValRead = new bool[8];
        byte[] ValWrite = new byte[1];

        FTDI device = new FTDI();
        FTDI.FT_STATUS status;

        public Hardware()
        {
            if ( InitializeConnection() )
            {
                _inProgress = true;
                _threadCyclic = new Thread( UpdateHardware );
                _threadCyclic.Start();
            }
        }

        public void Close()
        {
            device.Close();
        }

        bool InitializeConnection()
        {
            bool result = false;

            uint nbrDevice = 0;
            status = device.GetNumberOfDevices( ref nbrDevice );

            if ( status == FTDI.FT_STATUS.FT_OK && nbrDevice > 0 )
            {
                status = device.OpenByIndex( 0 );

                if ( status == FTDI.FT_STATUS.FT_OK )
                {
                    status = device.SetBaudRate( 9600 );

                    if ( status == FTDI.FT_STATUS.FT_OK )
                    {
                        status = device.SetBitMode( 0xE8, FTDI.FT_BIT_MODES.FT_BIT_MODE_ASYNC_BITBANG );
                        if( status == FTDI.FT_STATUS.FT_OK )
                            return true;
                    }
                }
            }

            return result;
        }

        public bool MovePlateau( int position )
        {
            bool result = false;

            IsPlateauEnable = true;

            while ( PlateauPosition != position )
            {
                Thread.Sleep( 100 );
            }

            IsPlateauEnable = false;
            result = true;

            return result;
        }

        public void EnableConvoyeur( bool state )
        {
            IsConvoyeurEnable = state;
        }

        public bool EjectPiece()
        {
            bool result = false;

            IsPushEnable = true;

            while ( _switchPush == true )
                Thread.Sleep( 10 );

            Thread.Sleep(1000);

            while ( _switchPush == false )
                Thread.Sleep( 10 );

            IsPushEnable = false;

            result = true;
            return result;
        }

        void DecodePlateauPosition()
        {
            if ( _oldSwitchPlateau0 == false && SwitchPlateauRef0 == true )
            {
                PlateauPosition = 0;
            }
            else if ( _oldSwitchPlateauPos == false && SwitchPlateauPosition == true )
            {
                PlateauPosition++;
            }

            _oldSwitchPlateauPos = SwitchPlateauPosition;
            _oldSwitchPlateau0 = SwitchPlateauRef0;
        }

        void DecodeBinary( byte val )
        {
            if ( ( val & 1 ) == 1 )
                ValRead[0] = true;
            else
                ValRead[0] = false;

            if ( ( val & 2 ) == 2 )
                ValRead[1] = true;
            else
                ValRead[1] = false;

            if ( ( val & 4 ) == 4 )
                ValRead[2] = true;
            else
                ValRead[2] = false;

            if ( ( val & 8 ) == 8 )
                ValRead[3] = true;
            else
                ValRead[3] = false;

            if ( ( val & 16 ) == 16 )
                ValRead[4] = true;
            else
                ValRead[4] = false;

            if ( ( val & 32 ) == 32 )
                ValRead[5] = true;
            else
                ValRead[5] = false;

            if ( ( val & 64 ) == 64 )
                ValRead[6] = true;
            else
                ValRead[6] = false;

            if ( ( val & 128 ) == 128 )
                ValRead[7] = true;
            else
                ValRead[7] = false;
        }

        void UpdateHardware()
        {
            uint nbrRes = 0;
            
            while ( _inProgress )
            {
                byte val = new byte();
                status = device.GetPinStates( ref val );

                DecodeBinary(val);

                SwitchPlateauRef0 = !ValRead[0];
                SwitchPlateauPosition = !ValRead[1];
                IsPieceDetected = ValRead[2];
                SwitchPush = !ValRead[4];

                DecodePlateauPosition();

                ValWrite[0] = 0;

                // D3 - Cmd2
                if( IsConvoyeurEnable )
                    ValWrite[0] |= 8;

                // D5 - Cmd3
                if(IsPlateauEnable)
                    ValWrite[0] |= 32;

                //D6 - Cmd1
                if( IsPushEnable )
                    ValWrite[0] |= 64;

                //D7 - Cmd4
                //if ( IsPushForward )
                //    ValWrite[0] |= 128;

                status = device.Write( ValWrite, 1, ref nbrRes );

                Thread.Sleep( 200 );
            }
        }

        #region Propterties

        bool _oldSwitchPlateau0;
        bool _oldSwitchPlateauPos;


        public const string IsPieceDetectedPropertyName = "IsPieceDetected";
        public bool IsPieceDetected
        {
            get { return _isPieceDetected; }
            set
            {
                if (_isPieceDetected != value)
                {
                    _isPieceDetected = value;
                    DoPropertyChanged(IsPieceDetectedPropertyName);
                }
            }
        }
        private bool _isPieceDetected;


        public const string SwitchPushPropertyName = "SwitchPush";
        public bool SwitchPush
        {
            get { return _switchPush; }
            set
            {
                if ( _switchPush != value )
                {
                    _switchPush = value;
                    DoPropertyChanged( SwitchPushPropertyName );
                }
            }
        }
        private bool _switchPush;
        

        public const string SwitchPlateauPositionPropertyName = "SwitchPlateauPosition";
        public bool SwitchPlateauPosition
        {
            get { return _switchPlateauPosition; }
            set
            {
                if ( _switchPlateauPosition != value )
                {
                    _switchPlateauPosition = value;
                    DoPropertyChanged( SwitchPlateauPositionPropertyName );
                }
            }
        }
        private bool _switchPlateauPosition;
        

        public const string SwitchPlateauRef0PropertyName = "SwitchPlateauRef0";
        public bool SwitchPlateauRef0
        {
            get { return _switchPlateauRef0; }
            set
            {
                if ( _switchPlateauRef0 != value )
                {
                    _switchPlateauRef0 = value;
                    DoPropertyChanged( SwitchPlateauRef0PropertyName );
                }
            }
        }
        private bool _switchPlateauRef0;
        

        public const string IsPushEnablePropertyName = "IsPushEnable";
        public bool IsPushEnable
        {
            get { return _isPushEnable; }
            set
            {
                if ( _isPushEnable != value )
                {
                    _isPushEnable = value;
                    DoPropertyChanged( IsPushEnablePropertyName );
                }
            }
        }
        private bool _isPushEnable;
                

        public const string IsConvoyeurEnablePropertyName = "IsConvoyeurEnable";
        public bool IsConvoyeurEnable
        {
            get { return _isConvoyeurEnable; }
            set
            {
                if ( _isConvoyeurEnable != value )
                {
                    _isConvoyeurEnable = value;
                    DoPropertyChanged( IsConvoyeurEnablePropertyName );
                }
            }
        }
        private bool _isConvoyeurEnable;

        public const string IsPlateauEnablePropertyName = "IsPlateauEnable";
        public bool IsPlateauEnable
        {
            get { return _isPlateauEnable; }
            set
            {
                if ( _isPlateauEnable != value )
                {
                    _isPlateauEnable = value;
                    DoPropertyChanged( IsPlateauEnablePropertyName );
                }
            }
        }
        private bool _isPlateauEnable;
        

        public const string PlateauPositionPropertyName = "PlateauPosition";
        public int PlateauPosition
        {
            get { return _plateauPosition; }
            set
            {
                if ( _plateauPosition != value )
                {
                    _plateauPosition = value;
                    DoPropertyChanged( PlateauPositionPropertyName );
                }
            }
        }
        private int _plateauPosition;


        public const string PlateauInPositionPropertyName = "PlateauInPosition";
        public bool PlateauInPosition
        {
            get { return _plateauInPosition; }
            set
            {
                if ( _plateauInPosition != value )
                {
                    _plateauInPosition = value;
                    DoPropertyChanged( PlateauInPositionPropertyName );
                }
            }
        }
        private bool _plateauInPosition;
        #endregion

    }
}
