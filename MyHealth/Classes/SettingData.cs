using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MyHealth
{
    public abstract class SettingData : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implamentation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        [XmlIgnore]
        private Dictionary<string, SettingItem> Values { set; get; }
        protected virtual object GetPropertyValue([CallerMemberName] string propertyName = null)
        {
            if(Values.TryGetValue(propertyName,out SettingItem item))
            {
                return item.Value;
            }
            return null;
        }
        protected virtual void SetPropertyValue(object value, [CallerMemberName] string propertyName = null)
        {
            if (Values.TryGetValue(propertyName, out SettingItem item))
            {
                Values[propertyName].Value = value;
            }
            else
            {
                Values.Add(propertyName, new SettingItem(value));
            }
            OnPropertyChanged(propertyName);
        }


        public void ResetValues()
        {
            foreach (var item in Values)
            {
                item.Value.ResetToDefault();
                OnPropertyChanged(item.Key);
            }
        }
        public void UndoChanges()
        {
            foreach (var item in Values)
            {
                item.Value.Undo();
                OnPropertyChanged(item.Key);
            }
        }
        public virtual void OnSaved()
        {
            foreach (var item in Values)
            {
                item.Value.ClearPreviousValue();
            }
        }
        public virtual void OnLoaded()
        {
            foreach (var item in Values)
            {
                item.Value.Init();
            }
        }

        public SettingData()
        {
            Values = new Dictionary<string, SettingItem>();
            DeclareValues();
        }

        private void DeclareValues()
        {
            Type selfType = GetType();
            foreach (var property in selfType.GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(true);
                if (attributes.Length != 0 && attributes[0] is DefaultValueAttribute defaultValue)
                {
                    Values.Add(property.Name, new SettingItem(defaultValue.Value));
                }
            }
        }
    }
    public class SettingItem
    {
        public readonly object Default;

        public object Value;
        public object Previous;

        public SettingItem() { }
        public SettingItem(object defaultValue)
        {
            Default = defaultValue;
            Value = defaultValue;
            Previous = defaultValue;

        }

        public void ClearPreviousValue()
        {
            Previous = Value;
        }
        public void ResetToDefault()
        {
            Value = Default;
        }
        public void Undo()
        {
            Value = Previous;
        }
        public void Init()
        {
            Previous = Value;
        }
    }

}
