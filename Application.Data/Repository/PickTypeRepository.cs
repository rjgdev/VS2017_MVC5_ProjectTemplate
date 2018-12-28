using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Data.Models;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Application.Data.Repository
{
    public class PickTypeRepository : IPickTypeRepository, IDisposable
    {

        private ApplicationDbContext _db = new ApplicationDbContext();

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

        public long Add(PickType obj)
        {
            try
            {
                _db.PickTypes.Add(obj);
                _db.SaveChanges();
                return obj.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Delete(long id)
        {
            _db.PickTypes.Remove(GetById(id));
            return true;
        }

        public PickType Get(Expression<Func<PickType, bool>> predicate)
        {
            return _db.PickTypes.FirstOrDefault(predicate);
        }

        public IEnumerable<PickType> GetAll()
        {
            return _db.PickTypes;
        }

        public PickType GetByCode(string code)
        {
            return _db.PickTypes.FirstOrDefault(x => x.Code.Equals(code));
        }

        public PickType GetById(long id)
        {
            return _db.PickTypes.Find(id);
        }

        //public IEnumerable<PickType> GetList(int take)
        //{
        //    return _db.PickTypes.Take(take);
        //}

        public IEnumerable<PickType> GetList(Expression<Func<PickType, bool>> predicate)
        {
            return _db.PickTypes.Where(predicate);
        }

        public bool Update(PickType obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.PickTypes.Attach(obj);
            _db.SaveChanges();

            return true;
        }

        public void Detach(PickType obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}
