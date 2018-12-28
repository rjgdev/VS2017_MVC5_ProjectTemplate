using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Data.Repository;
using Application.Model;
using Newtonsoft.Json.Linq;
using Application.Bll.Models;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class ExpectedReceiptLineService : IExpectedReceiptLineService
    {
        private readonly IExpectedReceiptLineRepository _ExpectedReceiptLineRepository;
        //private readonly IProductRepository _productRepository;
        private readonly IItemService _itemService;
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly IWarehouseService _warehouseService;
        private readonly ILocationService _locationService;
            

        public ExpectedReceiptLineService(IExpectedReceiptLineRepository ExpectedReceiptLineRepository,
            IItemService itemService, IProductService productService, IWarehouseService warehouseService,
            ILocationService locationService, IBrandService brandService)
        {
            this._ExpectedReceiptLineRepository = ExpectedReceiptLineRepository;
            this._itemService = itemService;
            _productService = productService;
            _locationService = locationService;
            _brandService = brandService;
            _warehouseService = warehouseService;
        }

        public long Add(ExpectedReceiptLine ExpectedReceiptLine)
        {

            return _ExpectedReceiptLineRepository.Add(ExpectedReceiptLine);
        }

        public ExpectedReceiptLine GetById(long id)
        {
            
            return _ExpectedReceiptLineRepository.GetById(id);
        }

        public bool Delete(long id, string updatedBy)
        {
         
            var obj = _ExpectedReceiptLineRepository.GetById(id);
            obj.IsActive = false;
            obj.UpdatedBy = updatedBy;
            return _ExpectedReceiptLineRepository.Update(obj);
            //var ExpectedReceiptLineDelete = _ExpectedReceiptLineRepository.GetById(id);
            //return _ExpectedReceiptLineRepository.Delete(id);
        }

        public bool Update(ExpectedReceiptLine obj)
        {
            return _ExpectedReceiptLineRepository.Update(obj);
        }

        //public IEnumerable<ExpectedReceiptLine> GetList(int take)
        //{
        //    return _ExpectedReceiptLineRepository.GetList(take);
        //}

        public IEnumerable<ExpectedReceiptLine> GetAll()
        {
            return _ExpectedReceiptLineRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExpectedReceiptLine> GetList(bool isActive, long customerId)
        {
            Expression<Func<ExpectedReceiptLine, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _ExpectedReceiptLineRepository.GetList(res);
        }

        public IEnumerable<ExpectedReceiptLine> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<ExpectedReceiptLine, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _ExpectedReceiptLineRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }


        public bool BatchUpdatExpectedReceiptLine(List<dynamic> objs)
        {
  
            foreach (var item in objs)
            {
                long id = (long)item.Id;
                var expectedReceiptLine = _ExpectedReceiptLineRepository.GetById(id);
                expectedReceiptLine.Quantity = item.Quantity;
                expectedReceiptLine.Comments = item.Comments;
                expectedReceiptLine.Image = item.Image;
                expectedReceiptLine.StatusId = 33;
                expectedReceiptLine.UpdatedBy = item.UpdatedBy;
                _ExpectedReceiptLineRepository.Update(expectedReceiptLine);

            }
            return true;

        }

        public long AddLineItem(ItemBindingModel model, long expectedReceiptLineId)
        {
         
            Expression<Func<Product, bool>> res = x => x.ProductCode.ToLower() == model.ProductCode.ToLower();
            var product = _productService.GetByProductCode(model.ProductCode);
            var brand = _brandService.GetByBrandCode(model.BrandCode);
            var warehouse = _warehouseService.GetByWarehouseCode(model.WarehouseCode);
            var location = _locationService.GetByLocationCode(model.LocationCode);
            var item = new Item
            {
                ProductId = product?.Id ?? null,
                ItemCode = model.ItemCode,
                Description = model.Description,
                CustomerId = model.CustomerId,
                WarehouseId = warehouse?.Id ?? null,
                LocationId = location?.Id ?? null,
                BrandId = model?.BrandId ?? brand?.Id ?? null,
                ReceivedBy = model.ReceivedBy,
                ReceivedDate = model.ReceivedDate,
                ExpiryDate = model.ExpiryDate,
            };


            long retId = _itemService.AddItem(item);
            long itemId = 0;
            if(retId == 0)
            {
                itemId = _itemService.GetByItemCode(item.ItemCode,item.CustomerId).Id;

            }

            var expectedReceiptLine = _ExpectedReceiptLineRepository.GetById(expectedReceiptLineId);

            expectedReceiptLine.BrandId = model?.BrandId ?? brand?.Id ?? null;
            expectedReceiptLine.ProductId  = product?.Id ?? null;
            expectedReceiptLine.UomId = model?.UomId ?? null;
            expectedReceiptLine.Quantity = model.Quantity;
            expectedReceiptLine.ItemCode = model.ItemCode;
            expectedReceiptLine.ItemDescription = model.Description;
            expectedReceiptLine.ItemId = retId == 0 ? itemId : retId;
            expectedReceiptLine.IsItemExist = true;
            expectedReceiptLine.StatusId = 32;
            _ExpectedReceiptLineRepository.Update(expectedReceiptLine);
            return expectedReceiptLine.Id;

        }

        public IEnumerable<ExpectedReceiptLine> GetByExpectedReceipt(long expectedReceiptid, bool isActive, long customerId)
        {
            Expression<Func<ExpectedReceiptLine, bool>> res = x => x.ExpectedReceiptId == expectedReceiptid && x.IsActive == isActive && x.CustomerId == customerId;
            return _ExpectedReceiptLineRepository.GetList(res);
        }

        public IEnumerable<dynamic> GetByExpectedReceiptMobile(long expectedReceiptid, bool isActive, long customerId)
        {
            Expression<Func<ExpectedReceiptLine, bool>> res = x => x.ExpectedReceiptId == expectedReceiptid && x.IsActive == isActive && x.CustomerId == customerId;

            var objs = _ExpectedReceiptLineRepository.GetList(res);

            return objs.Select(obj => new
            {
                obj.Id,
                obj.ExpectedReceiptId,
                obj.ItemId,
                obj.Quantity,
                obj.ItemDescription,
                obj.ItemCode,
                obj.StatusId,
                Status = obj.Status.Name

            });
        }

        public IEnumerable<ExpectedReceiptLine> GetByExpectedReceipt(long expectedReceiptid, bool isActive, long customerId, int pageNo, int pageSize)
        {
            Expression<Func<ExpectedReceiptLine, bool>> res = x => x.ExpectedReceiptId == expectedReceiptid && x.IsActive == isActive && x.CustomerId == customerId;
            return _ExpectedReceiptLineRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }

        //public IEnumerable<ExpectedReceiptLine> GetByExpectedReceiptMobile(long expectedReceiptid, bool isActive, long customerId)
        //{
        //    Expression<Func<ExpectedReceiptLine, bool>> res = x => x.ExpectedReceiptId == expectedReceiptid && x.IsActive == isActive && x.CustomerId == customerId && x.ExpectedReceipt.IsProcessing == false;
        //    return _ExpectedReceiptLineRepository.GetList(res);
        //}
    }
    
}
