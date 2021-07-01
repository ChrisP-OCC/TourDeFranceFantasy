using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public abstract class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public virtual void RefreshAllProperties()
        {
            OnPropertyChanged(string.Empty);
        }

        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real, 
            // public, instance property on this object. 
            if (propertyName == String.Empty) return; //Empty String is a signal that all properties have been changed and is valid
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;
                throw new Exception(msg);
            }
        }
    }
}
