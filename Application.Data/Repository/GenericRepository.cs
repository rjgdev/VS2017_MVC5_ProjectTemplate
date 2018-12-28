using Application.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Data.Repository
{
    public class GenericRepository<T> where T: class , IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public IEnumerable<T> GetAll()
        {

            return _db.Set<T>().ToList();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate);
        }


        public T Add(T obj)
        {
            _db.Set<T>().Add(obj);
            _db.SaveChanges();
            return obj;
        }

        public T AddOrUpdate(T obj)
        {
            _db.Set<T>().AddOrUpdate(obj);
            _db.SaveChanges();
            return obj;
        }

        public bool AddList(List<T> list)
        {
            _db.Set<T>().AddRange(list);
            _db.SaveChanges();
            return true;
        }

        public bool Remove(T obj)
        {
            _db.Set<T>().Remove(obj);
            _db.SaveChanges();
            return true;
        }


        public bool RemoveList(List<T> list)
        {
            _db.Set<T>().RemoveRange(list);
            _db.SaveChanges();
            return true;
        }

        public bool Update(T obj)
        {

            _db.Set<T>().Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateList(List<T> list)
        {

            list.ForEach(x => _db.Set<T>().Attach(x));
            _db.SaveChanges();
            return true;
        }

        public void Detach(T obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}
