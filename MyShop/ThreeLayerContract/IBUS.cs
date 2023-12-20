﻿using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeLayerContract
{
    public abstract class IBus
    {
        protected IDAO _dao;
        //------------Login----------//
        public abstract IBus CreateNew(IDAO dao);
        public abstract Task<bool> ConnectDB(string userName, string password);
        public abstract void SaveUserToConfig(string userName, string password);
        public abstract void SaveServerToConfig(string server, string database);
        public abstract Tuple<string, string> LoadUserFromConfig();
        public abstract Tuple<string, string> LoadServerFromConfig();
        //------------Order----------//
        public abstract Task<Tuple<List<ElementOrder>, int>> getListOrder(int _offset);
        public abstract Task<List<ElementOrder>> getListOrderPage(int _offset);
        public abstract Task<Tuple<List<ElementOrder>, int>> getListOrderBySearch(string dateFrom, string dateTo, int _offset);
        public abstract Task<List<ElementOrder>> getListOrderBySearchPage(string dateFrom, string dateTo, int _offset);
        public abstract Task<List<Category>> getListCateGory();
        public abstract int GetNextId(string tableName, string idColumnName);
        public abstract void insertOrderDetail(Book _book, int IDOrder);
        public abstract Task<int> insertOrder(string _date, ElementOrder _order);
        public abstract Task<BindingList<Book>> GetBookByCategory(string _nameCategory);
        public abstract Task<BindingList<Book>> GetDetailOrder(int Id);
        public abstract void DeleteOrder(ElementOrder _order);
        public abstract Task<BindingList<Book>> loadListProduct(int Id);
        public abstract Task<BindingList<Book>> LoadListProductWithCategory(string _nameCategory);
        public abstract void deleteInOrderDetail(Book _book, int IDOrder);
        //-----------------Product---------------
        public abstract BindingList<Category> selectAllCategory();
        public abstract BindingList<Category> readCategoryFile(string filePath);
        public abstract BindingList<Book> readBookFile(string filePath);
        public abstract void insertCategoryAndBook(BindingList<Category> categories, BindingList<Book> books);
        public abstract Tuple<BindingList<Book>, int> searchBook(string _sortBy, string _sortOption, string _searchText, int _currentPage,
            int _rowsPerPage, int _minPrice, int _maxPrice);
        public abstract Tuple<BindingList<Book>, int> selectBookByCategory(string name, string _sortBy, string _sortOption, string _searchText, int _currentPage,
            int _rowsPerPage, int _minPrice, int _maxPrice);
        public abstract void DeleteCategory(string Name, int Id);
        public abstract bool SaveCategory(string CategoryName);
        public abstract void AddBook(string title, string price, string description, string category, string image, string availability);
        public abstract void EditBook(Book editBook, int id);
        public abstract void DeleteBook(int Id);
        public abstract void EditCategory(Category editCat, int ID, string Name);
        //---------------------Dashboard------------------
        public abstract BindingList<Book> SoldingOutPr_Lv();
        public abstract List<String> LoadDashInfor();
        public abstract Tuple<string, List<string>, List<float>> ChartInfor(DateTime? _beginDate, DateTime? _endDate);
        public abstract Tuple<List<string>, List<int>> BooksAndQuantity(DateTime? _begin, DateTime? _end);
        public abstract string Name();
    }
}
