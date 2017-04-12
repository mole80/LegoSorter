using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace Appl
{
    class ViewModelConfigParameters : BaseControl
    {
        ViewmodelConfigControl _vmConfig;

        public ObservableCollection<Parameter> List
        {
            get { return MainWindow.Model.ConfigParameter.ConfigParametersList; }
        }

        public ViewModelConfigParameters( ViewmodelConfigControl vm )
        {
            _vmConfig = vm;            
        }
    }
}
