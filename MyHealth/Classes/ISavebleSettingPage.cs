using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealth
{
    public interface ISavebleSettingPage
    {
        public bool IsChanged { get; set; }
        public void SetValuesToAppSettings();
        public bool CanSave();
        public void UndoChanges();

    }
}
