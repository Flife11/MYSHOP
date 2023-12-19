using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Book : INotifyPropertyChanged
    {
        public int id;
        public string title;
        public string description;
        public string category;
        public string imgurl;
        public int availability;
        public double price;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }

        }
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public string Category
        {
            set { SetProperty(ref category, value); }
            get { return category; }
        }
        public string ImageUrl
        {
            get { return imgurl; }
            set { SetProperty(ref imgurl, value); }
        }
        public int Availability
        {
            get { return availability; }
            set { SetProperty(ref availability, value); }
        }
        public double Price
        {
            set { SetProperty(ref price, value); }
            get { return price; }
        }
        public string Description
        {
            set { SetProperty(ref description, value); }
            get { return description; }
        }
        public Book()
        {
            Id = 0;
            Title = string.Empty;
            Category = string.Empty;
            ImageUrl = string.Empty;
            Availability = 0;
            Price = 0;
            Description = string.Empty;
        }
        // Constructor để dễ dàng tạo một đối tượng Book mới
        public Book(int id, string title, string category, string imageUrl, int availability, double price, string description)
        {
            Id = id;
            Title = title;
            Category = category;
            ImageUrl = imageUrl;
            Availability = availability;
            Price = price;
            Description = description;
        }

        // Phương thức clone
        public Book Clone()
        {
            return new Book(Id, Title, Category, ImageUrl, Availability, Price, Description);
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
