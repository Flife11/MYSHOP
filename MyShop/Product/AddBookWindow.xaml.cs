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
using System.Windows.Shapes;
using Enity;

namespace Product
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public BindingList<Category> addCategory;

        public AddBookWindow(BindingList<Category> category)
        {
            InitializeComponent();
            this.addCategory = category;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            categoriesComboBox.ItemsSource = addCategory;
        }

        private void SaveBook_Button_Click(object sender, RoutedEventArgs e)
        {
            var title= Title_TextBox.Text;
            var price = Price_TextBox.Text;
            var description = Description_TextBox.Text;
            description = description.Replace("'", "''");

            var categorySelection = categoriesComboBox.SelectedItem as Category;
            var category = "";
            if(categorySelection != null)
            {
                category = categorySelection.Name;
            }
            var image = Image_TextBox.Text;
            var availability = Availability_TextBox.Text;

            //check if required fields are filled
            if(title == "" || price == "" || category == "" || availability == "")
            {
                MessageBox.Show("Please fill all the required fields (*)");
                return;
            }
            //use regrex to check if price is float
            string pattern = @"^[-+]?[0-9]*\.?[0-9]+$";
            Regex regex = new Regex(pattern);
            if(!regex.IsMatch(price))
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
            var sql = "SELECT MAX(ID) FROM book";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var id = reader.GetInt32(0) + 1;
            reader.Close();

            sql = "INSERT INTO book VALUES (@ID, @Title, @Price, @Description, @Category, @Image, @Availability)";
            command = new SqlCommand(sql, DB.Instance.Connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@Price", float.Parse(price));
            command.Parameters.AddWithValue("@Description", description);
            command.Parameters.AddWithValue("@Category", category);
            command.Parameters.AddWithValue("@Image",image);
            command.Parameters.AddWithValue("@Availability", int.Parse(availability));

            command.ExecuteNonQuery();

            MessageBox.Show("Book added successfully");

            this.Close();
        }

        private void CloseBook_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
