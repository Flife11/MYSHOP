using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace BUS04_Product
{
    public class BUS04_Product : IBus
    {
        public BUS04_Product() { }
        public BUS04_Product(IDAO dao)
        {
            _dao = dao;
        }
        public override BindingList<Category> selectAllCategory()
        {
            return _dao.selectAllCategory();
        }
        public override BindingList<Category> readCategoryFile(string filePath)
        {
            return _dao.readCategoryFile(filePath);
        }
        public override BindingList<Book> readBookFile(string filePath)
        {
            return _dao.readBookFile(filePath);
        }
        public override void insertCategoryAndBook(BindingList<Category> categories, BindingList<Book> books)
        {
            _dao.insertCategoryAndBook(categories, books);
        }
        public override Tuple<BindingList<Book>, int> searchBook(string _sortBy, string _sortOption, string _searchText, int _currentPage,
            int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            return _dao.searchBook(_sortBy, _sortOption, _searchText, _currentPage, _rowsPerPage, _minPrice, _maxPrice);
        }
        public override Tuple<BindingList<Book>, int> selectBookByCategory(string name, string _sortBy, string _sortOption, string _searchText, int _currentPage,
            int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            return _dao.selectBookByCategory(name, _sortBy, _sortOption, _searchText, _currentPage, _rowsPerPage, _minPrice, _maxPrice);
        }
        public override void DeleteCategory(string Name, int Id)
        {
            _dao.DeleteCategory(Name, Id);
        }
        public override bool SaveCategory(string CategoryName)
        {
            return _dao.SaveCategory(CategoryName);
        }
        public override void AddBook(string title, string price, string description, string category, string image, string availability)
        {
            _dao.AddBook(title, price, description, category, image, availability);
        }
        public override Task<bool> ConnectDB(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override IBus CreateNew(IDAO dao)
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
            return "product";
        }

        public override void SaveServerToConfig(string server, string database)
        {
            throw new NotImplementedException();
        }

        public override void SaveUserToConfig(string userName, string password)
        {
            throw new NotImplementedException();
        }

        
    }
}
