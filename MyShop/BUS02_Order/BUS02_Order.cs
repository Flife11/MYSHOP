﻿using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace BUS02_Order
{
    public class BUS02_Order : IBus
    {
        public BUS02_Order() { }
        public BUS02_Order(IDAO dao)
        {
            _dao = dao;
        }
        public override async Task<Tuple<List<ElementOrder>, int>> getListOrder(int _offset)
        {
            var data = await _dao.getListOrder(_offset);
            return data;

        }
        public override async Task<List<ElementOrder>> getListOrderPage(int _offset)
        {
            var data = await _dao.getListOrderPage(_offset);
            return data;
        }
        public override async Task<Tuple<List<ElementOrder>, int>> getListOrderBySearch(string dateFrom, string dateTo, int _offset)
        {
            var data = await _dao.getListOrderBySearch(dateFrom, dateTo, _offset);
            return data;
        }
        public override async Task<List<ElementOrder>> getListOrderBySearchPage(string dateFrom, string dateTo, int _offset)
        {
            var data = await _dao.getListOrderBySearchPage(dateFrom, dateTo, _offset);
            return data;
        }

        public override async Task<List<Category>> getListCateGory()
        {
            var data = await _dao.getListCateGory();
            return data;
        }
        public override int GetNextId(string tableName, string idColumnName)
        {
            return _dao.GetNextId(tableName, idColumnName);
        }
        public override void insertOrderDetail(Book _book, int IDOrder)
        {
            _dao.insertOrderDetail(_book, IDOrder);
        }
        public override async Task<int> insertOrder(string _date, ElementOrder _order)
        {
            var data = await _dao.insertOrder(_date, _order);
            return data;
        }
        public override async Task<BindingList<Book>> GetBookByCategory(string _nameCategory)
        {
            var data = await _dao.GetBookByCategory(_nameCategory);
            return data;
        }
        public override async Task<BindingList<Book>> GetDetailOrder(int Id)
        {
            var data = await _dao.GetDetailOrder(Id);
            return data;
        }
        public override async void DeleteOrder(ElementOrder _order)
        {
            _dao.DeleteOrder(_order);
        }
        public override async Task<BindingList<Book>> loadListProduct(int Id)
        {
            var data = await _dao.loadListProduct(Id);
            return data;
        }
        public override async Task<BindingList<Book>> LoadListProductWithCategory(string _nameCategory)
        {
            var data = await _dao.LoadListProductWithCategory(_nameCategory);
            return data;
        }
        public override void deleteInOrderDetail(Book _book, int IDOrder)
        {
            _dao.deleteInOrderDetail(_book, IDOrder);
        }
        public override string Name()
        {
            return "order";
        }
        public override Task<bool> ConnectDB(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override IBus CreateNew(IDAO dao)
        {
            return new BUS02_Order(dao);
        }

        public override Tuple<string, string> LoadServerFromConfig()
        {
            throw new NotImplementedException();
        }

        public override Tuple<string, string> LoadUserFromConfig()
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

        public override BindingList<Category> selectAllCategory()
        {
            throw new NotImplementedException();
        }

        public override BindingList<Category> readCategoryFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Book> readBookFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override void insertCategoryAndBook(BindingList<Category> categories, BindingList<Book> books)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BindingList<Book>, int> searchBook(string _sortBy, string _sortOption, string _searchText, int _currentPage, int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BindingList<Book>, int> selectBookByCategory(string name, string _sortBy, string _sortOption, string _searchText, int _currentPage, int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCategory(string Name, int Id)
        {
            throw new NotImplementedException();
        }

        public override bool SaveCategory(string CategoryName)
        {
            throw new NotImplementedException();
        }

        public override void AddBook(string title, string price, string description, string category, string image, string availability)
        {
            throw new NotImplementedException();
        }

        public override void EditBook(Book editBook, int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteBook(int Id)
        {
            throw new NotImplementedException();
        }

        public override void EditCategory(Category editCat, int ID, string Name)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Book> SoldingOutPr_Lv()
        {
            throw new NotImplementedException();
        }

        public override List<string> LoadDashInfor()
        {
            throw new NotImplementedException();
        }

        public override Tuple<string, List<string>, List<float>> ChartInfor(DateTime? _beginDate, DateTime? _endDate)
        {
            throw new NotImplementedException();
        }

        public override Tuple<List<string>, List<int>> BooksAndQuantity(DateTime? _begin, DateTime? _end)
        {
            throw new NotImplementedException();
        }
    }
}
