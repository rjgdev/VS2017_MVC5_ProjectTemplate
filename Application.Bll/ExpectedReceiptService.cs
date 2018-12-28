using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Data.Repository;
using Application.Bll.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class ExpectedReceiptService : IExpectedReceiptService
    {

        private readonly IExpectedReceiptRepository _expectedReceiptRepository;
        private readonly IExpectedReceiptLineRepository _expectedReceiptLineRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IHaulierService _haulierService;
        private readonly IStatusRepository _statusRepository;
        private readonly IItemService _itemService;

        #region Ctor
        public ExpectedReceiptService(IExpectedReceiptRepository expectedReceiptRepository,
            IExpectedReceiptLineRepository expectedReceiptLineRepository,
            IWarehouseRepository warehouseRepository,
            IProductRepository productRepository,
            IUomRepository uomRepository,
            IItemRepository itemRepository,
            IHaulierService haulierService, IStatusRepository statusRepository, IItemService itemService)
        {
            _expectedReceiptRepository = expectedReceiptRepository;
            _expectedReceiptLineRepository = expectedReceiptLineRepository;
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _uomRepository = uomRepository;
            _itemRepository = itemRepository;
            _haulierService = haulierService;
            _statusRepository = statusRepository;
            _itemService = itemService;
        }
        #endregion Ctor

        #region Header

        public long Add(ExpectedReceipt expectedReceipt)
        {
            return _expectedReceiptRepository.Add(expectedReceipt);
        }

        public long Add(ExpectedReceiptBindingModel bindingModel)
        {
            var warehouse = _warehouseRepository.GetByWarehouseCode(bindingModel.WarehouseCode);
            var haulier = _haulierService.GetByHaulierCode(bindingModel.HaulierCode);
            Func<ExpectedReceipt, bool> res = x => x.CustomerId == bindingModel.CustomerId;
            var count = _expectedReceiptRepository.GetList(res).Count();
            long? haulierId;
            var grn = String.Format("GRN-{0:0000000000}", count + 1);
            var arn = String.Format("ARN-{0:0000000000}", count + 1);

            if (bindingModel.HaulierId != null)
            {
                haulierId =  bindingModel.HaulierId;
            }
            else
            {
                haulierId = haulier?.Id ?? null;
            }
            ExpectedReceipt expectedReceipt = new ExpectedReceipt
            {
                //Id = (int) bindingModel.Id,
                ExpectedReceiptDate = bindingModel.ExpectedReceiptDate,
                GoodsReceivedNumber = grn,
                AutoReferenceNumber = arn,
                Received = bindingModel.Received,
                Comments = bindingModel.Comments,
                Address = bindingModel.Address,
                CustomerClientId = bindingModel.CustomerClientId,
                Supplier = bindingModel.Supplier,
                HaulierId = haulierId,
                StatusId = bindingModel.StatusId,
                CustomerId = (int)bindingModel.CustomerId,
                WarehouseId = warehouse.Id,
                ReceivedBy = bindingModel.ReceivedBy,
                ReceivedDate = bindingModel.ReceivedDate,
                Planned = bindingModel.Planned,
                ReferenceNumber = bindingModel.ReferenceNumber,
                CreatedBy = bindingModel.CreatedBy,
                UpdatedBy = bindingModel.UpdatedBy
            };
            if (IsDuplicate(expectedReceipt.ReferenceNumber, expectedReceipt.Id, expectedReceipt.CustomerId) == false) return _expectedReceiptRepository.Add(expectedReceipt);
            else
            {
                Expression<Func<ExpectedReceipt, bool>> expectedRes = x => x.ReferenceNumber == expectedReceipt.ReferenceNumber && x.CustomerId == expectedReceipt.CustomerId && x.IsActive == false;
                var model = _expectedReceiptRepository.Get(expectedRes);

                if (model != null)
                {
                    expectedReceipt.Id = model.Id;
                    expectedReceipt.IsActive = true;

                    _expectedReceiptRepository.Detach(model);
                    
                    _expectedReceiptRepository.Update(expectedReceipt);
                    return haulier.Id;
                }
                else return 0;
            }

            //return _expectedReceiptRepository.Add(expectedReceipt);
        }

        public ExpectedReceipt GetById(long id)
        {
            return _expectedReceiptRepository.GetById(id);
        }

        public IEnumerable<dynamic> GetForReceiving(bool isActive, long customerId)
        {
            var retVal = new List<dynamic>();
            Func<ExpectedReceipt, bool> predicate = (x => x.Status.Name.Equals("For Receiving") && x.IsActive == isActive && x.CustomerId == customerId);
            var list = _expectedReceiptRepository.GetList(predicate);

            foreach (var item in list)
                retVal.Add(new
                {
                    item.Address,
                    item.Comments,
                    item.Courier,
                    item.CustomerClientId,
                    item.CustomerId,
                    item.Customer,
                    item.ExpectedReceiptDate,
                    item.ExpectedReceiptLines,
                    item.GoodsReceivedNumber,
                    item.Haulier,
                    item.HaulierId,
                    item.Id,
                    item.Planned,
                    item.Received,
                    item.ReceivedBy,
                    item.ReceivedDate,
                    item.ReferenceNumber,
                    item.Status,
                    item.StatusId,
                    item.Supplier,
                    item.Warehouse,
                    WarehouseCode = item.Warehouse.WarehouseCode,
                    item.WarehouseId,
                    item.CreatedBy,
                    item.DateCreated,
                    item.UpdatedBy,
                    item.DateUpdated,
                    item.IsActive,
                    item.IsProcessing
                });


            return retVal;
        }

        public IEnumerable<dynamic> GetForReceivingMobile(bool isActive, long customerId)
        {
            var retVal = new List<dynamic>();
            Func<ExpectedReceipt, bool> predicate = (x => x.Status.Name.Equals("For Receiving") && x.IsActive == isActive && x.CustomerId == customerId && x.IsProcessing == false);
            var list = _expectedReceiptRepository.GetList(predicate);

            foreach (var item in list)
                retVal.Add(new
                {
                    item.Address,
                    item.Comments,
                    item.Courier,
                    item.CustomerClientId,
                    item.CustomerId,
                    item.Customer,
                    item.ExpectedReceiptDate,
                    item.ExpectedReceiptLines,
                    item.GoodsReceivedNumber,
                    item.Haulier,
                    item.HaulierId,
                    item.Id,
                    item.Planned,
                    item.Received,
                    item.ReceivedBy,
                    item.ReceivedDate,
                    item.ReferenceNumber,
                    item.Status,
                    item.StatusId,
                    item.Supplier,
                    item.Warehouse,
                    WarehouseCode = item.Warehouse.WarehouseCode,
                    item.WarehouseId,
                    item.CreatedBy,
                    item.DateCreated,
                    item.UpdatedBy,
                    item.DateUpdated,
                    item.IsActive,
                    item.IsProcessing
                });


            return retVal;
        }

        public IEnumerable<ExpectedReceipt> GetReceived(bool isActive, long customerId)
        {
            Func<ExpectedReceipt, bool> predicate = (x => x.Status.Name.Equals("Received") && x.IsActive == isActive && x.CustomerId == customerId);
            return _expectedReceiptRepository.GetList(predicate);
        }

        public IEnumerable<ExpectedReceipt> GetCompleted(bool isActive, long customerId)
        {
            Func<ExpectedReceipt, bool> predicate = (x => x.Status.Name.Equals("Completed") && x.IsActive == isActive && x.CustomerId == customerId);
            return _expectedReceiptRepository.GetList(predicate);
        }

        public ExpectedReceipt GetByReferenceNo(string refNo, long customerId)
        {
            Expression<Func<ExpectedReceipt, bool>> predicate = (x => x.ReferenceNumber.ToLower() == refNo.ToLower() && x.CustomerId == customerId);
            return _expectedReceiptRepository.Get(predicate);
        }

        public bool Delete(long id, string updatedBy)
        {
            var expectedReceiptDelete = _expectedReceiptRepository.GetById(id);
            expectedReceiptDelete.IsActive = false;
            return _expectedReceiptRepository.Update(expectedReceiptDelete);
        }

        public bool Enable(long id, string updatedBy)
        {
            var expectedReceiptDelete = _expectedReceiptRepository.GetById(id);
            expectedReceiptDelete.IsActive = true;
            expectedReceiptDelete.UpdatedBy = updatedBy;
            return _expectedReceiptRepository.Update(expectedReceiptDelete);
        }

        public bool Update(ExpectedReceipt obj)
        {
          
            return _expectedReceiptRepository.Update(obj);

        }

        public bool Update(ExpectedReceiptBindingModel obj)
        {
            
            Warehouse warehouse = _warehouseRepository.GetByWarehouseCode(obj.WarehouseCode);
            
            ExpectedReceipt expectedReceipt = new ExpectedReceipt
            {
                Id = (int)obj.Id,
                ExpectedReceiptDate = (DateTime)obj.ExpectedReceiptDate,
                GoodsReceivedNumber = obj.GoodsReceivedNumber,
                Received = obj.Received,
                Comments = obj.Comments,
                Address = obj.Address,
                CustomerId = (int)obj.CustomerId,
                CustomerClientId = obj.CustomerClientId,
                Supplier = obj.Supplier,
                HaulierId = obj.HaulierId,
                StatusId = obj.StatusId,
                Haulier = obj.Haulier,
                WarehouseId = warehouse.Id,
                ReceivedBy = obj.ReceivedBy,
                ReceivedDate = obj.ReceivedDate,
                Planned = obj.Planned,
                ReferenceNumber = obj.ReferenceNumber,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                UpdatedBy = obj.UpdatedBy,
                DateUpdated = obj.DateUpdated,
                IsActive = obj.IsActive,
                IsProcessing = obj.IsProcessing,
                AutoReferenceNumber = obj.AutoReferenceNumber
            };
            
            if (IsDuplicate(expectedReceipt.ReferenceNumber, expectedReceipt.Id, expectedReceipt.CustomerId) == false) return _expectedReceiptRepository.Update(expectedReceipt);
            else return false;
         
        }

        public bool ReceivedExpectedReceipt(ExpectedReceiptBindingModel obj)
        {
            var expectedReceipt = _expectedReceiptRepository.GetById(obj.Id ?? 0);

            if (expectedReceipt != null)
            {
              
                expectedReceipt.ReceivedDate = DateTime.Now;
                expectedReceipt.Received = true;
                expectedReceipt.UpdatedBy = obj.UpdatedBy;
                expectedReceipt.StatusId = 1;
                expectedReceipt.ReceivedBy = obj.ReceivedBy;
                expectedReceipt.UpdatedBy = obj.UpdatedBy;
                expectedReceipt.IsProcessing = true;
                _expectedReceiptRepository.Update(expectedReceipt);
                return true;
            }
            else return false;
        }

        //public IEnumerable<ExpectedReceipt> GetList(int take)
        //{
        //    return _expectedReceiptRepository.GetList(take);
        //}

        public ExpectedReceipt GetByGrn(string grn)
        {
            return _expectedReceiptRepository.GetByGrn(grn);
        }

        public IEnumerable<ExpectedReceipt> GetAll()
        {
            return _expectedReceiptRepository.GetAll();
        }

        public IEnumerable<ExpectedReceipt> GetPlannedReceipt(bool isActive, long customerId)
        {
            Func<ExpectedReceipt, bool> predicate = (x => x.Planned && x.IsActive == isActive && x.CustomerId == customerId);
            return _expectedReceiptRepository.GetList(predicate);
        }

        public IEnumerable<ExpectedReceipt> GetUnplannedReceipt(bool isActive, long customerId)
        {
            Func<ExpectedReceipt, bool> predicate = (x => !x.Planned && x.IsActive == isActive && x.CustomerId == customerId);
            return _expectedReceiptRepository.GetList(predicate);
        }

        public IEnumerable<ExpectedReceipt> GetReceipt(DateTime from, DateTime to, int take, string status, bool isActive, long customerId)
        {
            var statusId = _statusRepository.GetByStatus(status).Id;
            Func<ExpectedReceipt, bool> predicate = (x => x.DateCreated >= from && x.DateCreated <= to && x.StatusId == statusId && x.CustomerId == x.CustomerId && x.IsActive == isActive);
            return _expectedReceiptRepository.GetReceipt(predicate);
            
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<ExpectedReceipt, bool>> res;

            if (id == 0) res = x => x.ReferenceNumber.ToLower() == code.ToLower() && x.CustomerId == customerId && (!string.IsNullOrEmpty(code.Trim()));
            else res = x => x.ReferenceNumber.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId &&(!string.IsNullOrEmpty(code.Trim()));

            return _expectedReceiptRepository.Get(res) != null;
        }


        public bool CheckIsProcessing(long id)
        {
            var obj = _expectedReceiptRepository.GetById(id);
            return obj.IsProcessing;

        }


        public bool Onprocessing(long id, bool isprocessed, string updatedBy)
        {
            var obj = _expectedReceiptRepository.GetById(id);
            if (obj != null)
            {
                obj.IsProcessing = isprocessed;
                obj.UpdatedBy = updatedBy;
                return _expectedReceiptRepository.Update(obj);
            }

            else return false;

        }


        #endregion Header

        #region Lines
        public IEnumerable<ExpectedReceiptLine> GetLineList(int id)
        {
            return _expectedReceiptLineRepository.GetLineList(id);
        }

        public IEnumerable<ExpectedReceiptLine> GetLineList(int id, int take)
        {
            throw new NotImplementedException();
        }

        public ExpectedReceiptLine GetLineById(int id)
        {
            return _expectedReceiptLineRepository.GetById(id);
        }

        public bool AddLine(List<ExpectedReceiptLine> bindingModel)
        {
            var list = new List<ExpectedReceiptLine>();
            
            foreach (var line in bindingModel)
            {
                var item = _itemService.GetByItemCode(line.ItemCode, line.CustomerId);
                if(item != null)
                {
                   
                    line.BrandId = item.BrandId;
                    line.ProductId = item.ProductId;
                    line.ItemId = item.Id;
                    line.StatusId = 33;
       
                }
                line.IsItemExist = item != null;

                list.Add(line);
            }
            return _expectedReceiptLineRepository.AddLine(list);
        }

        public long AddLine(ExpectedReceiptLine expectedReceiptLine)
        {
            return _expectedReceiptLineRepository.Add(expectedReceiptLine);
        }

        public bool UpdateLine(ExpectedReceiptLine expectedReceiptLine)
        {
            return _expectedReceiptLineRepository.Update(expectedReceiptLine);
        }

        public bool UpdateLine(object obj)
        {

            var objModel = JObject.Parse(obj.ToString());
            var bindingModel = objModel.ToObject<ExpectedReceiptLineBindingModel>();

            Product product = _productRepository.GetByProductCode(bindingModel.ProductCode);


            var expectedReceiptLine = new ExpectedReceiptLine
            {
                Id = bindingModel.Id,
                ExpectedReceiptId = (int)bindingModel.ExpectedReceiptId,
                ProductId = product?.Id,
                UomId = bindingModel.UomId,
                Quantity = bindingModel.Quantity,
                BrandId = bindingModel.BrandId,
                Batch = bindingModel.Batch,
                ItemCode = bindingModel.ItemCode,
                ItemId = bindingModel.ItemId,
                Line = bindingModel.Line,
                ItemDescription = bindingModel.ItemDescription,
                ExpiryDate = bindingModel.ExpiryDate,
                Comments = bindingModel.Comments,
                Image = bindingModel.Image,
                CustomerId = bindingModel.CustomerId,
                IsItemExist = bindingModel.IsItemExist,
                IsActive = bindingModel.IsActive,
                IsChecked = bindingModel.IsChecked,
                DateCreated = bindingModel.DateCreated,
                DateUpdated = bindingModel.DateUpdated,
                CreatedBy = bindingModel.CreatedBy,
                UpdatedBy = bindingModel.UpdatedBy,
                StatusId = bindingModel.StatusId
            };

            return _expectedReceiptLineRepository.Update(expectedReceiptLine);
        }

        public bool BatchUpdate(List<object> obj)
        {
            //var objModel = JObject.Parse(obj.ToString());
            //List<ExpectedReceiptLine> expectedReceiptLine = obj.OfType<ExpectedReceiptLine>().ToList();
            List<ExpectedReceiptLine> expectedReceiptLines = new List<ExpectedReceiptLine>();
            foreach (var item in obj)
            {
                var objModel = JObject.Parse(item.ToString());
                var line = objModel.ToObject<ExpectedReceiptLine>();
                var expectedReceiptLine = _expectedReceiptLineRepository.GetById(line.Id);
                expectedReceiptLine.Quantity = line.Quantity;
                expectedReceiptLine.Comments = line.Comments;
                expectedReceiptLine.UpdatedBy = line.UpdatedBy;
                expectedReceiptLines.Add(objModel.ToObject<ExpectedReceiptLine>());
            }

            return _expectedReceiptLineRepository.Update(expectedReceiptLines);
        }

        public long AddLine(ExpectedReceiptLineBindingModel bindingModel)
        {
            Product product = _productRepository.GetByProductCode(bindingModel.ProductCode);
            Uom uom = _uomRepository.GetByDescription(bindingModel.UomDescription);


            
            var expectedReceiptLine = new ExpectedReceiptLine
            {
                ExpectedReceiptId = (int)bindingModel.ExpectedReceiptId,
                ProductId = product?.Id ,
                UomId = uom?.Id ,
                Quantity = bindingModel.Quantity,
                BrandId = bindingModel.BrandId,
                Batch = bindingModel.Batch,
                ItemCode = bindingModel.ItemCode,
                ItemId = bindingModel.ItemId,
                Line = bindingModel.Line,
                ItemDescription = bindingModel.ItemDescription,
                ExpiryDate = bindingModel.ExpiryDate,
                Comments = bindingModel.Comments,
                Image = bindingModel.Image,
                CustomerId = bindingModel.CustomerId,
                IsItemExist = bindingModel.IsItemExist,
                IsActive = bindingModel.IsActive,
                IsChecked = bindingModel.IsChecked,
                DateCreated = bindingModel.DateCreated,
                DateUpdated = bindingModel.DateUpdated,
                CreatedBy = bindingModel.CreatedBy,
                UpdatedBy = bindingModel.UpdatedBy,
                StatusId = 32
            };
            //var expectedReceiptLineId = _expectedReceiptLineRepository.Add(expectedReceiptLine);


            if (IsDuplicateLine(expectedReceiptLine.ItemCode, expectedReceiptLine.Id, expectedReceiptLine.CustomerId) == false) return _expectedReceiptLineRepository.Add(expectedReceiptLine);
            else
            {
                Expression<Func<ExpectedReceiptLine, bool>> res = x => x.ItemCode.ToLower() == expectedReceiptLine.ItemCode.ToLower() && x.CustomerId == expectedReceiptLine.CustomerId && x.IsActive == false;
                var getResult = _expectedReceiptLineRepository.Get(res);
                if (getResult != null)
                {
                    expectedReceiptLine.Id = getResult.Id;
                    expectedReceiptLine.IsActive = true;
                    expectedReceiptLine.StatusId = 32;

                    _expectedReceiptLineRepository.Detach(expectedReceiptLine);

                    _expectedReceiptLineRepository.Update(expectedReceiptLine);
                    return expectedReceiptLine.Id;
                }
                else
                {
                    return 0;
                }


            }


            ////Add Quantity on Items assign it to put away
            //if (expectedReceiptLineId != 0)
            //{
            //    Item item = _itemRepository.GetById(bindingModel.Id);
            //    item.Qty += expectedReceiptLine.Quantity;
            //    _itemRepository.Update(item);
            //    //return expectedReceiptLineId;
            //}

            //return expectedReceiptLineId;
        }

        public bool UpdateLine(ExpectedReceiptLineBindingModel bindingModel)
        {
            Product product = _productRepository.GetByProductCode(bindingModel.ProductCode);
            Uom uom = _uomRepository.GetByDescription(bindingModel.UomDescription);
            Item item = _itemService.GetByItemCode(bindingModel.ItemCode, bindingModel.CustomerId);


            ExpectedReceiptLine expectedReceiptLine = new ExpectedReceiptLine
            {
                Id = (int) bindingModel.Id,
                ExpectedReceiptId = (int)bindingModel.ExpectedReceiptId,
                Line = bindingModel.Line,
                Batch = bindingModel.Batch,
                ProductId = product?.Id,
                Quantity = bindingModel.Quantity,
                BrandId = bindingModel.BrandId,
                ItemCode = bindingModel.ItemCode,
                ItemId = item?.Id ,
                ItemDescription = bindingModel.ItemDescription,
                ExpiryDate = bindingModel.ExpiryDate,
                UomId = uom?.Id,
                CustomerId = bindingModel.CustomerId,
                IsItemExist = bindingModel.IsItemExist,
                IsActive = bindingModel.IsActive,
                IsChecked = bindingModel.IsChecked,
                DateCreated = bindingModel.DateCreated,
                DateUpdated = bindingModel.DateUpdated,
                CreatedBy = bindingModel.CreatedBy,
                UpdatedBy = bindingModel.UpdatedBy,
                StatusId = bindingModel.StatusId
               

            };
            //return _expectedReceiptLineRepository.Update(expectedReceiptLine);
            if (IsDuplicateLine(expectedReceiptLine.ItemCode, expectedReceiptLine.Id, expectedReceiptLine.CustomerId) == false) return _expectedReceiptLineRepository.Update(expectedReceiptLine);
            else return false;
        }

        public bool DeleteLine(long id, string updatedBy)
        {
            var obj = _expectedReceiptLineRepository.GetById(id);
            obj.IsActive = false;
            obj.UpdatedBy = updatedBy;
            return _expectedReceiptLineRepository.Update(obj);

        }

        public bool EnableLine(long id, string updatedBy)
        {
            var obj = _expectedReceiptLineRepository.GetById(id);
            obj.IsActive = true;
            obj.UpdatedBy = updatedBy;
            return _expectedReceiptLineRepository.Update(obj);

        }

        //public bool DeleteLine(int id)
        //{
        //    throw new NotImplementedException();
        //}
        
        public IEnumerable<ExpectedReceipt> GetList(bool isActive, long customerId)
        {
            Expression<Func<ExpectedReceipt, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _expectedReceiptRepository.GetList(res);
        }

        public IEnumerable<ExpectedReceipt> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<ExpectedReceipt, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _expectedReceiptRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }



        public bool IsDuplicateLine(string code, long id, long? customerId)
        {
            Expression<Func<ExpectedReceiptLine, bool>> res;

            if (id == 0) res = x => x.ExpectedReceiptId == id && x.ItemCode == code  && x.CustomerId == customerId;
            else res = x => x.ExpectedReceiptId == id && x.ItemCode == code && x.Id != id && x.CustomerId == customerId;

            return _expectedReceiptLineRepository.Get(res) != null;
        }

        public string GenerateReferenceNo(long customerId)
        {
            Expression<Func<ExpectedReceipt, bool>> res = x => x.CustomerId == customerId;
            var expectedReceiptCount = _expectedReceiptRepository.GetList(res).Count();
            var rn = String.Format("RN-{0:0000000000}", expectedReceiptCount + 1);
            return rn;
        }

        public IEnumerable<ExpectedReceipt> GetListMobile(bool isActive, long customerId)
        {
            throw new NotImplementedException();
        }


        #endregion Lines
    }
}
