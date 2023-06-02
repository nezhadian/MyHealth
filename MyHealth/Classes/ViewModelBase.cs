using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyHealth
{
    public abstract class ViewModelBase: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implamentation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ViewModelBase()
        {

        }
    }
}
