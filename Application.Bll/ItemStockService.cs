using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Bll.Interfaces;
using Application.Data.Repository;
using Application.Model;

namespace Application.Bll
{
    public class ItemStockService : IItemStockService
    {
        private readonly IItemStockRepository _itemStockRepository;
        private readonly IProductService _productService;
        private readonly IWarehouseService _warehouseService;
        private readonly ILocationService _locationService;
        private readonly IBrandService _brandService;


        public ItemStockService(IItemStockRepository itemStockRepository,
           IWarehouseService warehouseService, ILocationService locationService, IBrandService brandService,
           IProductService productService)
        {
            _itemStockRepository = itemStockRepository;
            _warehouseService = warehouseService;
            _locationService = locationService;
            _brandService = brandService;
            _productService = productService;
        }

        public long Add(ItemStock obj)
        {
           return _itemStockRepository.Add(obj);
        }

        public bool Delete(long id, string updatedBy)
        {
            var obj = _itemStockRepository.GetById(id);
            obj.IsActive = false;
            obj.UpdatedBy = updatedBy;
            return _itemStockRepository.Update(obj);
        }

        public bool Enable(long id, string updatedBy)
        {
            var obj = _itemStockRepository.GetById(id);
            obj.IsActive = true;
            obj.UpdatedBy = updatedBy;
            return _itemStockRepository.Update(obj);
        }

        public IEnumerable<ItemStock> GetAll()
        {
            return _itemStockRepository.GetAll();
        }

        public ItemStock GetById(long id)
        {
            return _itemStockRepository.GetById(id);
        }

        public IEnumerable<ItemStock> GetList(bool isActive, long customerId)
        {
            Expression<Func<ItemStock, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _itemStockRepository.GetList(res);
        }

        public IEnumerable<ItemStock> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<ItemStock, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _itemStockRepository.GetList(res).Skip(pageNo).Take(pageSize);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            throw new NotImplementedException();
        }

        public bool Update(ItemStock obj)
        {
            return _itemStockRepository.Update(obj);
        }
    }
}
