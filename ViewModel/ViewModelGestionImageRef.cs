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

using System.Collections.ObjectModel;


namespace Appl
{
    class ViewModelGestionImageRef : NotifierComponent
    {
        ViewmodelConfigControl _vmConfig;

        public ObservableCollection<ImageRef> BricksRef { get { return MainWindow.Model.BricksRef; } }

        public const string SelectedImagePropertyName = "SelectedImage";
        public ImageRef SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                if ( _selectedImage != value )
                {
                    _selectedImage = value;
                    if( InfosImageControl != null )
                        InfosImageControl.DataContext = _selectedImage;
                    DoPropertyChanged( SelectedImagePropertyName );
                }
            }
        }
        private ImageRef _selectedImage;
        
        // Control qui affiche une image et ces paramètres (taille, nom, rond, ligne ... )
        public const string InfosImageControlPropertyName = "InfosImageControl";
        public Control InfosImageControl
        {
            get { return _infosImageControl; }
            set
            {
                    _infosImageControl = value;
                    DoPropertyChanged( InfosImageControlPropertyName );
            }
        }
        private Control _infosImageControl;

        public ViewModelGestionImageRef( ViewmodelConfigControl vm )
        {
            _vmConfig = vm;
            InfosImageControl = new PlotImageRefInfosControl();
        }
    }
}
