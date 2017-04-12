using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace Appl
{
    public abstract class NotifierComponent : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void DoPropertyChanged(string propertyName)
        {
            if (Application.Current != null && Application.Current.Dispatcher != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                    InternDoPropertyChanged(propertyName);
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        InternDoPropertyChanged(propertyName);
                    }));
                }
            }
        }

        private void InternDoPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
