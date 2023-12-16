using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product
{
    public class book : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public int Availability { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
