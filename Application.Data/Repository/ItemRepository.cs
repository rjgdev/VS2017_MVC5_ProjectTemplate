using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class ItemRepository : IItemRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public long Add(Item obj)
        {
            _db.Items.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool Update(Item obj)
        {
            //var item = _db.Set<Item>().Local.FirstOrDefault(f => f.Id == obj.Id);
            //if (item != null)
            //{
            //    _db.Entry(item).State = EntityState.Detached;
            //}

            _db.Entry(obj).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        public void Detach(Item obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }

        public bool Delete(long id)
        {
            _db.Items.Remove(GetById(id));
            _db.SaveChanges();
            return true;
        }

        public Item GetById(long id)
        {
            return _db.Items.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Item> GetListByProductId(long id)
        {
            return _db.Items.Where(x => x.ProductId == id);
        }

        public IEnumerable<Item> GetListByBrandId(long id)
        {
            return _db.Items.Where(x => x.BrandId == id);
        }

        //public IEnumerable<ItemSelectListViewModel> GetSelectList(long productId)
        //{
        //    var list = new List<ItemSelectListViewModel>();
        //    foreach (var item in _db.Items.Where(w => w.ProductId == productId).OrderBy(o => o.Description))
        //        list.Add(new ItemSelectListViewModel {Id = item.Id, Description = item.Description});
        //    return list;
        //}

        //public IEnumerable<Item> GetList(int take)
        //{
        //    return _db.Items.Take(take);
        //}

        public IEnumerable<Item> GetAll()
        {
            return _db.Items;
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

        public IEnumerable<ItemSelectListViewModel> GetSelectListByBrand(long brandId)
        {
            var list = new List<ItemSelectListViewModel>();
            foreach (var item in _db.Items.Where(w => w.BrandId == brandId).OrderBy(o => o.Description))
                list.Add(new ItemSelectListViewModel { Id = item.Id, Code = item.ItemCode, Description = item.Description });
            return list;
        }

        public IEnumerable<ItemSelectListViewModel> GetSelectListByProductCode(string productCode)
        {
            var list = new List<ItemSelectListViewModel>();
            foreach (var item in _db.Items.Where(w => w.Product.ProductCode == productCode).OrderBy(o => o.Description))
                list.Add(new ItemSelectListViewModel { Id = item.Id, Code = item.ItemCode, Description = item.Description });
            return list;
            
        }

        public dynamic GetItemDescriptionByItemCode(long id)
        {
            var item = _db.Items.FirstOrDefault(x => x.Id == id);
            var brand = _db.Brands.FirstOrDefault(x => x.Id == item.BrandId);


            var obj = new 
            {
                ItemDescription = item.Description,
                BrandId = brand.Id,
                BrandName = brand.Name
            };

            return obj;

        }

        //IEnumerable<Item> IItemRepository.GetAll()
        //{
        //    return _db.Items.ToList();
        //}

        public Item Get(Expression<Func<Item, bool>> predicate)
        {
            return _db.Items.FirstOrDefault(predicate);
        }

        public IEnumerable<Item> GetList(Expression<Func<Item, bool>> predicate)
        {
            return _db.Items.Where(predicate);
        }
    }
}