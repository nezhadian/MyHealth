using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealth
{
    public interface ISavebleSettingPage
    {
        public bool CanSave { get; }
        public bool IsChanged { get; set; }
        public void Save();
    }
}
