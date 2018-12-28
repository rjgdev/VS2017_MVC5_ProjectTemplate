using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class ExpectedReceiptLineRepository : IExpectedReceiptLineRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExpectedReceiptLine GetById(long id)
        {
            return _db.ExpectedReceiptLines.Find(id);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long Add(ExpectedReceiptLine obj)
        {
            var expectedReceiptLine = _db.ExpectedReceiptLines.FirstOrDefault(x => x.ExpectedReceiptId == obj.ExpectedReceiptId && x.ItemCode == obj.ItemCode);
            if(expectedReceiptLine != null)
            {
                if(expectedReceiptLine.IsActive)
                {
                    expectedReceiptLine.Quantity += obj.Quantity;
                }
                else
                {
                    expectedReceiptLine.Quantity = obj.Quantity;
                    expectedReceiptLine.UpdatedBy = obj.UpdatedBy;
                    expectedReceiptLine.ExpiryDate = obj.ExpiryDate;
                    expectedReceiptLine.IsActive = true;
                }
                _db.Entry(expectedReceiptLine).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                _db.ExpectedReceiptLines.Add(obj);
                _db.SaveChanges();
            }
            
            return obj.Id;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            _db.ExpectedReceiptLines.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public bool AddLine(List<ExpectedReceiptLine> obj)
        {

            foreach (var item in obj)
            {
                var expectedReceiptLine = _db.ExpectedReceiptLines.FirstOrDefault(x => x.ExpectedReceiptId == item.ExpectedReceiptId && x.ItemCode == item.ItemCode);
                if(expectedReceiptLine != null)
                {
                    expectedReceiptLine.Quantity += item.Quantity;
                    _db.Entry(expectedReceiptLine).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    _db.ExpectedReceiptLines.Add(item);
                    _db.SaveChanges();
                }

            

            }
            //_db.ExpectedReceiptLines.AddRange(obj);
            //_db.SaveChanges();
            return true;
        }

        public bool Update(ExpectedReceiptLine obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.ExpectedReceiptLines.Attach(obj);
            _db.SaveChanges();

            return true;
        }

        //public IEnumerable<ExpectedReceiptLine> GetList(int take)
        //{
        //    return _db.ExpectedReceiptLines.Take(take);
        //}

        public IEnumerable<ExpectedReceiptLine> GetLineList(int id)
        {
            return _db.ExpectedReceiptLines.Where(x => x.ExpectedReceiptId == id);
        }

        public IEnumerable<ExpectedReceiptLine> GetAll()
        {
            return _db.ExpectedReceiptLines;
        }

        public bool Update(List<ExpectedReceiptLine> list)
        {
            foreach (var item in list)
            {
                //var getObj = GetById(item.Id);
                //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
                _db.Entry(item).State = EntityState.Modified;
                //_db.ExpectedReceiptLines.Attach(item);

            }
            _db.SaveChanges();
            return true;
        }

        ///// <inheritdoc />
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        ///// <summary>
        /////     Dispose
        ///// </summary>
        ///// <param name="disposing"></param>
        //protected void Dispose(bool disposing)
        //{
        //    if (disposing)
        //        if (_db != null)
        //        {
        //            _db.Dispose();
        //            _db = null;
        //        }
        //}

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

        public ExpectedReceiptLine Get(Expression<Func<ExpectedReceiptLine, bool>> predicate)
        {
            return _db.ExpectedReceiptLines.FirstOrDefault(predicate);
        }

        public IEnumerable<ExpectedReceiptLine> GetList(Expression<Func<ExpectedReceiptLine, bool>> predicate)
        {
            return _db.ExpectedReceiptLines.Where(predicate);
        }

        public void Detach(ExpectedReceiptLine obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}