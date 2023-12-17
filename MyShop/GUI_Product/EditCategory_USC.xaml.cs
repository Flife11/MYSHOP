using Enity;
using Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThreeLayerContract;

namespace GUI_Product
{
    /// <summary>
    /// Interaction logic for EditCategory_USC.xaml
    /// </summary>
    public partial class EditCategory_USC : UserControl
    {
        Category editCat = new Category();
        Category _backupCat;
        IBus _bus;
        public EditCategory_USC(IBus bus, Category cate)
        {
            _bus = bus;
            _backupCat = cate;
            editCat = (Category)cate.Clone();
            InitializeComponent();
        }
        private void SaveCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            _bus.EditCategory(editCat, _backupCat.ID, _backupCat.Name);            

            editCat.CopyProperties(_backupCat);
            Window parent = Window.GetWindow(this);
            parent.DialogResult = true;
        }

        private void CloseCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            parent.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            DataContext = editCat;
            parent.Title = "Edit - " + editCat.ID.ToString();
        }
    }
}
