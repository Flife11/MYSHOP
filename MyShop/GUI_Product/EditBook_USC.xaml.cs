using Enity;
using Entity;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThreeLayerContract;

namespace GUI_Product
{
    /// <summary>
    /// Interaction logic for EditBook_USC.xaml
    /// </summary>
    public partial class EditBook_USC : UserControl
    {
        BindingList<Category> editCategory = new BindingList<Category>();
        Book editBook;
        IBus _bus;
        Book _backupBook;
        public EditBook_USC(IBus bus, BindingList<Category> categories, Book book)
        {
            _bus = bus;
            editCategory = categories;
            _backupBook = book;
            editBook = (Book)book.Clone();
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = editBook;
            categoriesComboBox.ItemsSource = editCategory;
            categoriesComboBox.SelectedItem = editCategory.FirstOrDefault(x => x.Name == editBook.Category);
        }

        private void SaveBook_Button_Click(object sender, RoutedEventArgs e)
        {
            var title = Title_TextBox.Text;
            var price = Price_TextBox.Text;
            var description = Description_TextBox.Text;

            var categorySelection = categoriesComboBox.SelectedItem as Category;
            var category = "";
            if (categorySelection != null)
            {
                category = categorySelection.Name;
            }
            var image = Image_TextBox.Text;
            var availability = Availability_TextBox.Text;

            //check if required fields are filled
            if (title == "" || price == "" || category == "" || availability == "")
            {
                MessageBox.Show("Please fill all the required fields (*)");
                return;
            }
            //use regrex to check if price is float
            string pattern = @"^[-+]?[0-9]*\.?[0-9]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(price))
            {
                MessageBox.Show("Price must be a float");
                return;
            }

            //check if availability is int
            pattern = @"^-?\d+$";
            regex = new Regex(pattern);
            if (!regex.IsMatch(availability))
            {
                MessageBox.Show("Availability must be an integer");
                return;
            }

            editBook.title = title;
            editBook.price = float.Parse(price);
            editBook.description = description.Replace("'", "''");
            editBook.category = category;
            editBook.imgurl = image;
            editBook.availability = int.Parse(availability);

            _bus.EditBook(editBook, _backupBook.id);            

            editBook.CopyProperties(_backupBook);

            Window parent = Window.GetWindow(this);            
            parent.DialogResult = true;
        }

        private void CloseBook_Button_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            parent.Close();
        }
    }
}
