using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Models
{
    public class DeviceStatus : Dictionary<string, int>
    {
        public DeviceStatus()
        { }
        public DeviceStatus(string b)
        {
            //var p = new Dictionary<string, int>();
            int index_1 = b.IndexOf("0 : ") + 4;
            this.Add("LED0",b[index_1] == '0' ? 0: 1 );
            int index_2 = b.IndexOf("1 : ") + 4;
            this.Add("LED1", b[index_2] == '0' ? 0 : 1);
            int index_3 = b.IndexOf("2 : ") + 4;
            this.Add("LED2", b[index_3] == '0' ? 0 : 1);
            int index_4 = b.IndexOf("3 : ") + 4;
            this.Add("LED3", b[index_4] == '0' ? 0 : 1);

            
        }
        public override string ToString()
        {
            string dictionaryString = "{";
            foreach (KeyValuePair<string, int> keyValues in this)
            {
                dictionaryString += keyValues.Key + " : " + keyValues.Value + ", ";
            }
            return dictionaryString.TrimEnd(',', ' ') + "}";
        }

        public static explicit operator DeviceStatus(string b) => new DeviceStatus(b);
    }

    public class Device : BsonData.Document
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        //public Device()
        //{
        //    PropertyChanged += (o, p) =>
        //    {
        //        var newDeviceView = new DeviceViewModel();
        //        newDeviceView.UpdateStatus(Convert.ToInt32(this.strStatus, 2));
        //        this.Status = newDeviceView.Status;
        //        System.Diagnostics.Debug.WriteLine(this.strStatus);
        //        //this is the place you would do what you wanted to do
        //        //when the property has changed.
        //    };
        //}
        

        public string Name { get; set; }
        public DeviceStatus Status { get; set; } = new DeviceStatus();
        //public DeviceStatus _Status
        //{
        //    get {
        //        if (!strStatus.Equals(getStatus()) && !strStatus.Equals("") )
        //        {
        //            var _deviceView = new DeviceViewModel();
        //            _deviceView.UpdateStatus(Convert.ToInt32(strStatus, 2));
        //            Status = _deviceView.Status;
        //        }    
        //        return Status; }

        //    set
        //    {
        //        Status = value;
        //    }
        //}
        //public string strStatus
        //{
        //    get; set;
        //} 
        public string getStatus()
        {
            string res="";
            foreach (var p in this.Status)
            {
                res = p.Value.ToString() + res;
            }
            return res;
        }
    }
    public class DeviceViewModel : Device
    {
        public void UpdateStatus(int value)
        {
            System.Diagnostics.Debug.WriteLine("Update Status");
            int i = 0;
            bool changed = false;

            var s = new DeviceStatus();
            foreach (var p in Status)
            {
                var b = (value & (8 >> i)) == 0 ? 0 : 1;
                i++;
                if (b != p.Value)
                {
                    changed = true;
                }
                s.Add(p.Key, b);
            }
            if (changed)
            {
                Status = s;
                Changed?.Invoke(this, value);
            }
        }
        public event Action<Device, int> Changed;
    }
}