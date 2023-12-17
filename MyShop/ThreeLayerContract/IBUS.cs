using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeLayerContract
{
    public abstract class IBus
    {
        protected IDAO _dao;
        public abstract IBus CreateNew(IDAO dao);
        public abstract Task<bool> ConnectDB(string userName, string password);
        public abstract void SaveUserToConfig(string userName, string password);
        public abstract void SaveServerToConfig(string server, string database);
        public abstract Tuple<string, string> LoadUserFromConfig();
        public abstract Tuple<string, string> LoadServerFromConfig();
        public abstract string Name();
        public abstract void deleteInOrderDetail(Book _book, int IDOrder);
        public abstract void DeleteOrder(ElementOrder _order);
        public abstract Task<System.ComponentModel.BindingList<global::Models.Book>> GetBookByCategory(string _nameCategory);
        public abstract Task<System.ComponentModel.BindingList<global::Models.Book>> GetDetailOrder(int Id);
        public abstract Task<List<global::Models.Category>> getListCateGory();
        public abstract Task<Tuple<List<global::Models.ElementOrder>, int>> getListOrder(int _offset);
        public abstract Task<Tuple<List<global::Models.ElementOrder>, int>> getListOrderBySearch(string dateFrom, string dateTo, int _offset);
        public abstract Task<List<global::Models.ElementOrder>> getListOrderBySearchPage(string dateFrom, string dateTo, int _offset);
        public abstract Task<List<global::Models.ElementOrder>> getListOrderPage(int _offset);
        public abstract int GetNextId(string tableName, string idColumnName);
        public abstract Task<int> insertOrder(string _date, ElementOrder _order);
        public abstract void insertOrderDetail(Book _book, int IDOrder);
        public abstract Task<System.ComponentModel.BindingList<global::Models.Book>> loadListProduct(int Id);
        public abstract Task<System.ComponentModel.BindingList<global::Models.Book>> LoadListProductWithCategory(string _nameCategory);
    }
}
