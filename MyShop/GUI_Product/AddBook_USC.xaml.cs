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
    /// Interaction logic for AddBook_USC.xaml
    /// </summary>
    public partial class AddBook_USC : UserControl
    {
        IBus _bus;
        public BindingList<Category> addCategory;
        public AddBook_USC(IBus bus, BindingList<Category> category)
        {
            _bus = bus;
            this.addCategory = category;
            InitializeComponent();
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            categoriesComboBox.ItemsSource = addCategory;
        }

        private void SaveBook_Button_Click(object sender, RoutedEventArgs e)
        {
            var title = Title_TextBox.Text;
            var price = Price_TextBox.Text;
            var description = Description_TextBox.Text;
            description = description.Replace("'", "''");

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

            //query select max id of book+1
            _bus.AddBook(title, price, description, category, image, availability);            

            MessageBox.Show("Book added successfully");

            Window parent = Window.GetWindow(this);
            parent.Close();
        }

        private void CloseBook_Button_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            parent.Close();
        }
    }
}
