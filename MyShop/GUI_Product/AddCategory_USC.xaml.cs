using Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThreeLayerContract;

namespace GUI_Product
{
    /// <summary>
    /// Interaction logic for AddCategory_USC.xaml
    /// </summary>
    public partial class AddCategory_USC : UserControl
    {
        IBus _bus;
        public AddCategory_USC(IBus bus)
        {
            _bus = bus;
            InitializeComponent();
        }
        private void SaveCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool insert = _bus.SaveCategory(CategoryName_TextBox.Text);
                if (insert)
                {
                    MessageBox.Show("Category already exists");
                } else
                {
                    MessageBox.Show("Category added successfully");
                }

                Window parent = Window.GetWindow(this);
                parent.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            //close this screen
            Window parent = Window.GetWindow(this);
            parent.Close();
        }
    }
}
