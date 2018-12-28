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
    public class BrandRepository : IBrandRepository, IDisposable
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        //public BrandRepository(ApplicationDbContext db)
        //{
        //    _db = db;
        //}

        public long Add(Brand obj)
        {
            _db.Brands.Add(obj);
            _db.SaveChanges();

            return obj.Id;
        }

        public bool Delete(long id)
        {
            _db.Brands.Remove(GetById(id));
            return true;
        }

        public IEnumerable<Brand> GetAll()
        {
            return _db.Brands;
        }

        public Brand GetById(long id)
        {
            return _db.Brands.Find(id);
        }

        //public IEnumerable<Brand> GetList(int take)
        //{
        //    return _db.Brands.Take(take);
        //}

        public bool Update(Brand obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            //_db.Brands.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public Brand Get(Expression<Func<Brand, bool>> predicate)
        {
            return _db.Brands.FirstOrDefault(predicate);

        }


        #region IDisposable Support
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
                }
        }

        public IEnumerable<Brand> GetByProductId(long id)
        {
            return _db.Brands.Where(x => x.ProductId == id);
        }

        public IEnumerable<Brand> GetByProductCode(string code)
        {
            return _db.Brands.Where(x => x.ProductCode.ToLower() == code.ToLower());
        }

        public IEnumerable<Brand> GetByItemCode(string code)
        {
            var brandIds = _db.Items.Where(x => x.ItemCode.ToLower() == code.ToLower()).Select(x => x.BrandId);
            return _db.Brands.Where(x => brandIds.Contains(x.Id));
        }

        public dynamic GetByBrandCode(string code)
        {
            return _db.Brands.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
        }

        public IEnumerable<Brand> GetList(Expression<Func<Brand, bool>> predicate)
        {
            return _db.Brands.Where(predicate);
        }

        public void Detach(Brand obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }



        #endregion
    }
}
