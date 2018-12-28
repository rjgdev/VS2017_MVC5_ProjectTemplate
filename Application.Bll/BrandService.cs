using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Data.Repository;
using Application.Model;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class BrandService : IBrandService
    {
        private IBrandRepository _brandRepository;
        private IItemRepository _itemRepository;
        private IProductRepository _productRepository;

        public BrandService(IBrandRepository brandRepository,
            IItemRepository itemRepository, IProductRepository productRepository)
        {
            _brandRepository = brandRepository;
            _itemRepository = itemRepository;
            _productRepository = productRepository;
        }

        public long Add(Brand obj)
        {

            long retId = 0;
            if (obj.ProductId != 0 && obj.ProductId != null)
            {
                var product = _productRepository.GetById(obj.ProductId ?? 0);
                obj.ProductCode = product.ProductCode;
            }
            if (IsDuplicate(obj.Code, obj.Id, obj.CustomerId) == false)
            {
                retId = _brandRepository.Add(obj);
            }
            else
            {
                Expression<Func<Brand, bool>> res;

                res = x => x.Code.ToLower() == obj.Code.ToLower() && x.CustomerId == obj.CustomerId && x.IsActive == false;
                var brand = _brandRepository.Get(res);
                if (brand != null)
                {
                    retId = brand.Id;

                    obj.Id = retId;
                    obj.IsActive = true;

                    _brandRepository.Detach(brand);

                    _brandRepository.Update(obj);
                    //return obj.Id;
                }
            }

            return retId;
        }

        public bool Delete(long id, string updatedBy)
        {
            var brand = _brandRepository.GetById(id);
            brand.IsActive = false;
            brand.UpdatedBy = updatedBy;
            var retVal = _brandRepository.Update(brand);

            if (retVal)
            {
                var items = _itemRepository.GetListByBrandId(id).ToList();

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
        }

        public bool Enable(long id,string UpdatedBy)
        {
            var brand = _brandRepository.GetById(id);
            brand.UpdatedBy = UpdatedBy;
            brand.IsActive = true;
            return _brandRepository.Update(brand);
        }

        public IEnumerable<Brand> GetAll()
        {
            return _brandRepository.GetAll();
        }


        public Brand GetById(long id)
        {
            return _brandRepository.GetById(id);
        }

        //public IEnumerable<Brand> GetList(int take)
        //{
        //    return _brandRepository.GetList(take);
        //}

        public bool  Update(Brand obj)
        {
            if (obj.ProductId != 0 && obj.ProductId != null)
            {
                var product = _productRepository.GetById(obj.ProductId ?? 0);
                obj.ProductCode = product.ProductCode;
            }
            if (IsDuplicate(obj.Code, obj.Id, obj.CustomerId) == false) return _brandRepository.Update(obj);
            else return false;
        }

        public IEnumerable<Brand> GetByProductId(long id)
        {
            Expression<Func<Brand, bool>> res = x => x.ProductId == id;
            return _brandRepository.GetByProductId(id);
        }

        public IEnumerable<Brand> GetByProductCode(string code, bool isActive, long customerId)
        {
            //Expression<Func<Product, bool>> resProduct = x => x.ProductCode.ToLower() == code.ToLower() && x.IsActive == isActive && x.CustomerId == customerId;
            //var product = _productRepository.Get(resProduct);
            Expression<Func<Brand, bool>> resBrand = x => x.ProductCode.ToLower() == code.ToLower() && x.IsActive == isActive && x.CustomerId == customerId; 

            return _brandRepository.GetList(resBrand);
            //return _brandRepository.GetByProductCode(code);
        }

        public IEnumerable<Brand> GetByItemCode(string code)
        {
            return _brandRepository.GetByItemCode(code);
        }

        public dynamic GetByBrandCode(string code)
        {
            return _brandRepository.GetByBrandCode(code);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Brand, bool>> res;

            if (id == 0) res = x => x.Code.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && x.Id != id && x.CustomerId == customerId;

            return _brandRepository.Get(res) != null;
        }

        public IEnumerable<Brand> GetList(bool isActive, long customerId)
        {
            Expression<Func<Brand, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _brandRepository.GetList(res);
        }

        public IEnumerable<Brand> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Brand, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _brandRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}
