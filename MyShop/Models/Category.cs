using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Category : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
