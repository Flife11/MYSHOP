using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditCategoryWindow.xaml
    /// </summary>   /// 

   
    public partial class EditCategoryWindow : Window
    {
        public Category editCategory { get; set; }
        public EditCategoryWindow(Category category)
        {
            InitializeComponent();
            editCategory = (Category)category.Clone();
        }

        private void SaveCategory_Button_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;
        }

        private void CloseCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = editCategory;
            Title = "Edit - "+editCategory.ID.ToString();
        }
    }
}
