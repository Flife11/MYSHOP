using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace DAO03_DashBoard
{
    public class DAO03_DashBoard : IDAO
    {
        public DAO03_DashBoard() { }

        public override void AddBook(string title, string price, string description, string category, string image, string availability)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> ConnectDB(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override void DeleteBook(int Id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCategory(string Name, int Id)
        {
            throw new NotImplementedException();
        }

        public override void deleteInOrderDetail(Book _book, int IDOrder)
        {
            throw new NotImplementedException();
        }

        public override void DeleteOrder(ElementOrder _order)
        {
            throw new NotImplementedException();
        }

        public override void EditBook(Book editBook, int id)
        {
            throw new NotImplementedException();
        }

        public override void EditCategory(Category editCat, int ID, string Name)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> GetBookByCategory(string _nameCategory)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> GetDetailOrder(int Id)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Category>> getListCateGory()
        {
            throw new NotImplementedException();
        }

        public override Task<Tuple<List<ElementOrder>, int>> getListOrder(int _offset)
        {
            throw new NotImplementedException();
        }

        public override Task<Tuple<List<ElementOrder>, int>> getListOrderBySearch(string dateFrom, string dateTo, int _offset)
        {
            throw new NotImplementedException();
        }

        public override Task<List<ElementOrder>> getListOrderBySearchPage(string dateFrom, string dateTo, int _offset)
        {
            throw new NotImplementedException();
        }

        public override Task<List<ElementOrder>> getListOrderPage(int _offset)
        {
            throw new NotImplementedException();
        }

        public override int GetNextId(string tableName, string idColumnName)
        {
            throw new NotImplementedException();
        }

        public override void insertCategoryAndBook(BindingList<Category> categories, BindingList<Book> books)
        {
            throw new NotImplementedException();
        }

        public override Task<int> insertOrder(string _date, ElementOrder _order)
        {
            throw new NotImplementedException();
        }

        public override void insertOrderDetail(Book _book, int IDOrder)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> loadListProduct(int Id)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> LoadListProductWithCategory(string _nameCategory)
        {
            throw new NotImplementedException();
        }

        public override Tuple<string, string> LoadServerFromConfig()
        {
            throw new NotImplementedException();
        }

        public override Tuple<string, string> LoadUserFromConfig()
        {
            throw new NotImplementedException();
        }

        public override string Name()
        {
            return "dashb";
        }

        public override BindingList<Book> readBookFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Category> readCategoryFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override bool SaveCategory(string CategoryName)
        {
            throw new NotImplementedException();
        }

        public override void SaveServerToConfig(string server, string database)
        {
            throw new NotImplementedException();
        }

        public override void SaveUserToConfig(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BindingList<Book>, int> searchBook(string _sortBy, string _sortOption, string _searchText, int _currentPage, int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Category> selectAllCategory()
        {
            throw new NotImplementedException();
        }

        public override Tuple<BindingList<Book>, int> selectBookByCategory(string name, string _sortBy, string _sortOption, string _searchText, int _currentPage, int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            throw new NotImplementedException();
        }
    }
}
