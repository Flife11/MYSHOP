using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public int Availability { get; set; }
        public double Price {  get; set; }
        public Book()
        {
            Id= 0;
            Title= string.Empty;
            Category= string.Empty;
            ImageUrl= string.Empty;
            Availability= 0;
            Price= 0;
        }
        // Constructor để dễ dàng tạo một đối tượng Book mới
        public Book(int id, string title, string category, string imageUrl, int availability, double price)
        {
            Id = id;
            Title = title;
            Category = category;
            ImageUrl = imageUrl;
            Availability = availability;
            Price = price;
        }

        // Phương thức clone
        public Book Clone()
        {
            return new Book(Id, Title, Category, ImageUrl, Availability, Price);
        }
    }
}
