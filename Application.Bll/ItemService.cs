using System;
using System.Collections.Generic;
using System.Dynamic;
using Application.Data.Repository;
using Application.Model;
using System.Linq.Expressions;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Bll
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseService _warehouseService;
        private readonly ILocationService _locationService;
        private readonly IBrandService _brandService;
        private readonly IProductService _productService;

        public ItemService(IItemRepository itemRepository, IProductRepository productRepository,
            IWarehouseService warehouseService, ILocationService locationService, IBrandService brandService,
            IProductService productService)
        {
            _itemRepository = itemRepository;
            _productRepository = productRepository;
            _warehouseService = warehouseService;
            _locationService = locationService;
            _brandService = brandService;
            _productService = productService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long Add(object model)
        {
            try
            {
                var itemObj = (dynamic)model;

                var product = itemObj.ProductId == null ?
                    _productService.GetByProductCode(itemObj.ProductCode.Value) : null;
                var brand = itemObj.BrandId == null ?
                        _brandService.GetByBrandCode(((dynamic)itemObj).BrandCode.Value) : null;

                var item = new Item
                {
                    CustomerId = itemObj.CustomerId != null ? itemObj.CustomerId.Value : null,
                    ProductId = itemObj.ProductId != null ? itemObj.ProductId.Value : product.Id,
                    BrandId = itemObj.BrandId != null ? itemObj.BrandId.Value : brand.Id,
                    ItemCode = itemObj.ItemCode != null ? itemObj.ItemCode.Value : "",
                    Description = itemObj.Description != null ? itemObj.Description.Value : "",
                    ReceivedBy = itemObj.ReceivedBy != null ? itemObj.ReceivedBy.Value : "",
                    ReceivedDate = itemObj.ReceivedDate != null ? itemObj.ReceivedDate.Value : (DateTime?)null,
                    ExpiryDate = itemObj.ExpiryDate != null ? itemObj.ExpiryDate.Value : (DateTime?)null,
                    CreatedBy = itemObj.CreatedBy != null ? itemObj.CreatedBy.Value : ""
                };

                long retId = 0;

                if (IsDuplicate(item.ItemCode, item.Id, item.CustomerId) == false)
                {
                    retId = _itemRepository.Add(item);
                }
                else
                {
                    Expression<Func<Item, bool>> res = x => x.ItemCode == item.ItemCode && x.CustomerId == item.CustomerId && x.IsActive == false;
                    var obj = _itemRepository.Get(res);

                    if (obj != null)
                    {
                        retId = obj.Id;

                        item.Id = retId;
                        item.IsActive = true;

                        _itemRepository.Detach(obj);

                        _itemRepository.Update(item);

                        //return item.Id;
                    }
                }

                return retId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Update(object model)
        {
            try
            {
                var obj = (dynamic)model;

                var product = obj.ProductId == null ?
                    _productService.GetByProductCode(obj.ProductCode.Value) : null;
                var brand = obj.BrandId == null ?
                    _brandService.GetByBrandCode(obj.BrandCode.Value) : null;

                var item = (Item)GetByItemCode(obj.ItemCode.Value, obj.CustomerId.Value);

                item.Description = obj.Description != null ? obj.Description.Value : "";
                item.ProductId = obj.ProductId != null ? obj.ProductId.Value : product.Id;
                item.BrandId = obj.BrandId != null ? obj.BrandId.Value : brand.Id;
                item.UpdatedBy = obj.UpdatedBy != null ? obj.UpdatedBy.Value : "";

                if (IsDuplicate(item.ItemCode, item.Id, item.CustomerId) == false) return _itemRepository.Update(item);
                else return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Delete(long id, string updatedBy)
        {
            var item = _itemRepository.GetById(id);
            item.IsActive = false;
            item.UpdatedBy = updatedBy;
            return _itemRepository.Update(item);
        }

        public bool Enable(long id, string updatedBy)
        {
            var item = _itemRepository.GetById(id);
            item.IsActive = true;
            item.UpdatedBy = updatedBy;
            
            return _itemRepository.Update(item);
        }

        public Item GetById(long id)
        {
            return _itemRepository.GetById(id);
        }

        
        public dynamic GetByItemCode(string itemCode, long? customerId)
        {
            //dynamic retVal = null;
            var item = new Item();

            Expression<Func<Item, bool>> res = x => x.ItemCode == itemCode && x.CustomerId == customerId;
            item = _itemRepository.Get(res); 

            return item;
        }

        public IEnumerable<Item> GetAll()
        {
            return _itemRepository.GetAll();
        }

        public IEnumerable<ItemSelectListViewModel> GetSelectListByBrandId(long brandId)
        {
            var objs = _itemRepository.GetListByBrandId(brandId);
            return objs.Select(obj => new ItemSelectListViewModel
            {
                Id = obj.Id,
                Code = obj.ItemCode,
                Description = obj.Description
            });
        }

        public IEnumerable<ItemSelectListViewModel> GetSelectListByProductId(long productId)
        {
            var objs = _itemRepository.GetListByProductId(productId);
            return objs.Select(obj => new ItemSelectListViewModel
            {
                Id = obj.Id,
                Code = obj.ItemCode,
                Description = obj.Description
            });
        }

        public IEnumerable<ItemSelectListViewModel> GetSelectListByProductCode(string productCode)
        {
            return _itemRepository.GetSelectListByProductCode(productCode);
        }

        public dynamic GetItemDescriptionByItemCode(long id)
        {
            return _itemRepository.GetItemDescriptionByItemCode(id);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Item, bool>> res;

            if (id == 0) res = x => x.ItemCode.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.ItemCode.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId;

            return _itemRepository.Get(res) != null;
        }

        public IEnumerable<Item> GetList(bool isActive, long customerId)
        {

            Expression<Func<Item, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _itemRepository.GetList(res);
        }

        public IEnumerable<Item> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {

            Expression<Func<Item, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;

            return _itemRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }

        public bool Update(Item obj)
        {
            throw new NotImplementedException();
        }

        public long Add(Item obj)
        {
            throw new NotImplementedException();
        }

        public long AddItem(dynamic model)
        {

            try
            {
                if (IsDuplicate(model.ItemCode, model.Id, model.CustomerId)) return 0;

                var brand = model.BrandId == null ?
                    _brandService.GetByBrandCode(model.BrandCode) : null;
                var product = model.ProductId == null ?
                    _productService.GetByProductCode(model.ProductCode) : null;
                //var warehouse = model.WarehouseId == null ?
                //    _warehouseService.GetByWarehouseCode(model.WarehouseCode) : null;
                //var location = model.LocationId == null ?
                //    _locationService.GetByLocationCode(model.LocationCode) : null;
                var item = new Item
                {
                    CustomerId = model.CustomerId != null ? model.CustomerId : null,
                    //WarehouseId = model.WarehouseId != null ? model.WarehouseId : warehouse.Id,
                    //LocationId = model.LocationId != null ? model.LocationId : location.Id,
                    ProductId = model.ProductId != null ? model.ProductId : product.Id,
                    BrandId = model.BrandId != null ? model.BrandId : brand.Id,
                    ItemCode = model.ItemCode != null ? model.ItemCode : "",
                    Description = model.Description != null ? model.Description : "",
                    ReceivedBy = model.ReceivedBy != null ? model.ReceivedBy : "",
                    ReceivedDate = model.ReceivedDate != null ? model.ReceivedDate : (DateTime?)null,
                    ExpiryDate = model.ExpiryDate != null ? model.ExpiryDate : (DateTime?)null,
                    CreatedBy = model.CreatedBy != null ? model.CreatedBy : ""
                };

                return _itemRepository.Add(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}