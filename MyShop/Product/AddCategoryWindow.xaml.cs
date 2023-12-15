using Enity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Product
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {

        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void SaveCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sql = $"SELECT* FROM Category where Name = '{CategoryName_TextBox.Text}'";
                var command = new SqlCommand(sql, DB.Instance.Connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MessageBox.Show("Category already exists");
                    reader.Close();
                    return;
                }
                reader.Close();

                //select max id of Category +1 and cast to int
                sql = "SELECT MAX(ID) FROM Category";
                command = new SqlCommand(sql, DB.Instance.Connection);
                reader = command.ExecuteReader();
                reader.Read();
                var id = reader.GetInt32(0) + 1;
                reader.Close();

                sql = $"INSERT INTO Category VALUES({id},'{CategoryName_TextBox.Text}')";
                command = new SqlCommand(sql, DB.Instance.Connection);
                command.ExecuteNonQuery();
                MessageBox.Show("Category added successfully");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void CloseCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            //close this screen
            this.Close();
        }
    }
}
