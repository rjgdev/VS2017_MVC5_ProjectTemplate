using Application.Data.Models;
using Application.Data.Repository.Interfaces;
using Application.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Application.Data.Repository
{
    public class ExpectedReceiptRepository:  IExpectedReceiptRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        //public ExpectedReceiptRepository(ApplicationDbContext db)
        //{
        //    _db = db;
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExpectedReceipt GetById(long id)
        {
            return _db.ExpectedReceipts.Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long Add(ExpectedReceipt obj)
        {
            _db.ExpectedReceipts.Add(obj);
            _db.SaveChanges();
            return obj.Id;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            try
            {
                _db.ExpectedReceipts.Remove(GetById(id));
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(ExpectedReceipt obj)
        {
            _db.Entry(obj).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        //public IEnumerable<ExpectedReceipt> GetList(int take)
        //{
        //    return _db.ExpectedReceipts.Take(take);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grn"></param>
        /// <returns></returns>
        public ExpectedReceipt GetByGrn(string grn)
        {
            return _db.ExpectedReceipts.FirstOrDefault(x => x.GoodsReceivedNumber.Equals(grn));
        }

        public IEnumerable<ExpectedReceipt> GetAll()
        {
            return _db.ExpectedReceipts;
        }

        public IEnumerable<ExpectedReceipt> GetList(Func<ExpectedReceipt, bool> predicate)
        {
            return _db.ExpectedReceipts.Where(predicate);
        }

        //public ExpectedReceipt Get(Func<ExpectedReceipt, bool> predicate)
        //{
        //    return _db.ExpectedReceipts.FirstOrDefault(predicate);
        //}

        public IEnumerable<ExpectedReceipt> GetReceipt(Func<ExpectedReceipt, bool> predicate)
        {
            return _db.ExpectedReceipts.Where(predicate);
        }

        public ExpectedReceipt Get(Expression<Func<ExpectedReceipt, bool>> predicate)
        {
            return _db.ExpectedReceipts.FirstOrDefault(predicate);
        }

        public IEnumerable<ExpectedReceipt> GetList(Expression<Func<ExpectedReceipt, bool>> predicate)
        {
            return _db.ExpectedReceipts.Where(predicate);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
        }

        public void Detach(ExpectedReceipt obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}
