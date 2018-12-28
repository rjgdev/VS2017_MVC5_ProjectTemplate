using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        //public ProductRepository(ApplicationDbContext db)
        //{
        //    _db = db;
        //}

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public long Add(Product obj)
        {
            _db.Products.Add(obj);
            _db.SaveChanges();

            return obj.Id;
        }

        public bool Update(Product obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.Products.Attach(obj);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            _db.Products.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public Product GetById(long id)
        {
            return _db.Products.Find(id);
        }

        public Product GetByProductCode(string productCode)
        {
            return _db.Products.FirstOrDefault(x => x.ProductCode.ToLower() == productCode.ToLower());
        }

        public IEnumerable<ProductSelectListViewModel> GetSelectList()
        {
            var list = new List<ProductSelectListViewModel>();
            foreach (var item in _db.Products.OrderBy(o => o.Description))
                list.Add(new ProductSelectListViewModel
                {
                    Id = item.Id,
                    ProductCode = item.ProductCode,
                    Description = item.Description
                });
            return list;
        }

        //public IEnumerable<Product> GetList(int take)
        //{
        //    return _db.Products.Take(take);
        //}

        public IEnumerable<Product> GetAll()
        {
            return _db.Products;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                if (_db != null)
                    _db.Dispose();
        }

        public Product Get(Expression<Func<Product, bool>> predicate)
        {
            return _db.Products.FirstOrDefault(predicate);
        }

        public IEnumerable<Product> GetList(Expression<Func<Product, bool>> predicate)
        {
            return _db.Products.Where(predicate);
        }

        public void Detach(Product obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }
    }
}