using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Data.Repository;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IItemRepository _itemRepository;

        public ProductService(IProductRepository productRepository,
            IBrandRepository brandRepository,
            IItemRepository itemRepository)
        {
            this._productRepository = productRepository;
            this._brandRepository = brandRepository;
            this._itemRepository = itemRepository;
        }

        public long Add(Product obj)
        {
            if (IsDuplicate(obj.ProductCode, obj.Id, obj.CustomerId) == false) return _productRepository.Add(obj);
            else
            {
                Expression<Func<Product, bool>> res = x => x.ProductCode == obj.ProductCode && x.CustomerId == obj.CustomerId && x.IsActive == false;
                var model = _productRepository.Get(res);

                if(model != null)
                {
                    obj.Id = model.Id;
                    obj.IsActive = true;

                    _productRepository.Detach(model);

                    _productRepository.Update(obj);
                    return obj.Id;
                }
                else return 0;
            }
        }

        public bool Delete(long id, string updatedBy)
        {
            var obj = _productRepository.GetById(id);
            obj.IsActive = false;
            obj.UpdatedBy = updatedBy;

            var retVal = _productRepository.Update(obj);

            if (retVal)
            {
                var brands = _brandRepository.GetByProductId(obj.Id);
                if(brands != null && brands.Count() > 0)
                {
                    foreach(var brand in brands)
                    {
                        brand.IsActive = false;
                        _brandRepository.Update(brand);
                    }
                }

                var items = _itemRepository.GetListByProductId(id).ToList();
                if(items != null && items.Count() > 0)
                {
                    foreach(var item in items)
                    {
                        item.IsActive = false;
                        item.UpdatedBy = updatedBy;
                        _itemRepository.Update(item);
                    }
                }
            }

            return retVal;

            //return _productRepository.Delete(id);
        }

        public bool Enable(long id, string updatedBy)
        {
            var obj = _productRepository.GetById(id);
            obj.IsActive = true;
            obj.UpdatedBy = updatedBy;
            return _productRepository.Update(obj);

            //return _productRepository.Delete(id);
        }

        public Product GetById(long id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByProductCode(string productCode)
        {
            return _productRepository.GetByProductCode(productCode);
        }

        public IEnumerable<ProductSelectListViewModel> GetSelectList()
        {
            return _productRepository.GetSelectList();
        }

        //public IEnumerable<Product> GetList(int take)
        //{
        //    return _productRepository.GetList(take);
        //}

        public bool Update(Product obj)
        {
            if (IsDuplicate(obj.ProductCode, obj.Id, obj.CustomerId) == false) return _productRepository.Update(obj);
            else return false;
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Product, bool>> res;

            if (id == 0) res = x => x.ProductCode.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.ProductCode.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId;

            return _productRepository.Get(res) != null;
        }

        public IEnumerable<Product> GetList(bool isActive, long customerId)
        {
            Expression<Func<Product, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _productRepository.GetList(res);
        }

        public IEnumerable<Product> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Product, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _productRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}
