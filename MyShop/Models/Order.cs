using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ElementOrder : INotifyPropertyChanged
    {
        private int id;
        private string date;
        private int quantity;
        private double price;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }
        public string Date
        {
            set { SetProperty(ref date, value); }
            get { return date; }
        }
        public int Quantity
        {
            set { SetProperty<int>(ref quantity, value); }
            get { return quantity; }
        }
        public double Price
        {
            get { return price; }
            set { SetProperty<double>(ref price, value); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
