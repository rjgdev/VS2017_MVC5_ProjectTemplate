using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class ItemStockRepository : IItemStockRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public long Add(ItemStock obj)
        {
            _db.ItemStocks.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Delete(long id)
        {
            _db.ItemStocks.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public void Detach(ItemStock obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ItemStock Get(Expression<Func<ItemStock, bool>> predicate)
        {
            return _db.ItemStocks.FirstOrDefault(predicate);
        }

        public IEnumerable<ItemStock> GetAll()
        {
            return _db.ItemStocks;
        }

        public ItemStock GetById(long id)
        {
            return _db.ItemStocks.Find(id);
        }

        public IEnumerable<ItemStock> GetList(Expression<Func<ItemStock, bool>> predicate)
        {
            return _db.ItemStocks.Where(predicate);
        }

        public bool Update(ItemStock obj)
        {
            _db.Entry(obj).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        #region Dispose                                                                                                                                       
        protected void Dispose(bool disposing)
        {
            if (disposing)
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
        }
        #endregion Dispose     


    }
}
