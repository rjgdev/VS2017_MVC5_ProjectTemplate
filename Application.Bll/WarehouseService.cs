using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Data.Repository;
using Application.Bll.Models;
using System.Linq.Expressions;

namespace Application.Bll
{
    public class WarehouseService : IWarehouseService
    {

        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICustomerService _customerService;

        public WarehouseService(IWarehouseRepository warehouseRepository, ICustomerService customerService,
            ILocationRepository locationRepository)
        {
            _warehouseRepository = warehouseRepository;
            _locationRepository = locationRepository;
            _customerService = customerService;
        }



        public long Add(Warehouse obj)
        {
            //var customer = _customerService.GetByDomain(obj.);
   
            if (IsDuplicate(obj.WarehouseCode, obj.Id, obj.CustomerId) == false) return _warehouseRepository.Add(obj);
            else
            {
                Expression<Func<Warehouse, bool>> res = x => x.WarehouseCode == obj.WarehouseCode && x.CustomerId == obj.CustomerId && x.IsActive == false;
                var model = _warehouseRepository.Get(res);

                if(model != null)
                {
                    obj.Id = model.Id;
                    obj.IsActive = true;

                    _warehouseRepository.Detach(model);

                    _warehouseRepository.Update(obj);
                    return obj.Id;
                }
                else return 0;
            }
        }

        //public long Add(WarehouseBindingModel obj)
        //{
        //    var customer = _customerService.GetByDomain(obj.Domain);
        //    Warehouse warehouse = new Warehouse
        //    {
        //        WarehouseCode = obj.WarehouseCode,
        //        CustomerId = customer.Id,
        //        Description = obj.Description,
        //        Address1 = obj.
        //        //AddressCode = obj.AddressCode,
        //    };
        //    if (IsDuplicate(warehouse.WarehouseCode, warehouse.Id, warehouse.CustomerId) == false) return _warehouseRepository.Add(warehouse);
        //    else return 0;
        //}

        public bool Delete(long id, string updatedBy)
        {
            var warehouse = _warehouseRepository.GetById(id);
            warehouse.IsActive = false;
            warehouse.UpdatedBy = updatedBy;

            var retVal = _warehouseRepository.Update(warehouse);

            if (retVal)
            {
                var locs = _locationRepository.GetListByWarehouseId(id).ToList();

                if(locs != null || locs.Count() > 0)
                {
                    foreach(var loc in locs)
                    {
                        loc.IsActive = false;
                        loc.UpdatedBy = updatedBy;
                        _locationRepository.Update(loc);
                    }
                }
            }

            return retVal;
        }

        public bool Enable(long id, string updatedBy)
        {
            var warehouse = _warehouseRepository.GetById(id);
            warehouse.IsActive = true;
            warehouse.UpdatedBy = updatedBy;
            return _warehouseRepository.Update(warehouse);
        }

        public IEnumerable<Warehouse> GetAll()
        {
            return _warehouseRepository.GetAll();
        }

        public Warehouse GetById(long id)
        {
            return _warehouseRepository.GetById(id);
        }

        public Warehouse GetByWarehouseCode(string warehouseCode)
        {
            return _warehouseRepository.GetByWarehouseCode(warehouseCode);
        }

        //public IEnumerable<Warehouse> GetList(int take)
        //{
        //    return _warehouseRepository.GetList(take);
        //}

        public IEnumerable<Warehouse> GetList(bool isActive, long customerId)
        {
            Expression<Func<Warehouse, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _warehouseRepository.GetList(res);
        }

        public IEnumerable<Warehouse> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Warehouse, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _warehouseRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Warehouse, bool>> res;

            if (id == 0) res = x => x.WarehouseCode.ToLower() == code.ToLower() && x.CustomerId == customerId;
            else res = x => x.WarehouseCode.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId;

            return _warehouseRepository.Get(res) != null;
        }

        public bool Update(Warehouse obj)
        {
            if (IsDuplicate(obj.WarehouseCode, obj.Id, obj.CustomerId) == false) return _warehouseRepository.Update(obj);
            else return false;
        }

        //public bool Update(WarehouseBindingModel obj)
        //{
        //    var customer = _customerService.GetByDomain(obj.Domain);
        //    var warehouse = new Warehouse
        //    {
        //        Id = obj.Id,
        //        Description = obj.Description,
        //        WarehouseCode = obj.WarehouseCode,
        //        CustomerId = customer.Id,
        //        //AddressCode = obj.AddressCode
        //    };

        //    if (IsDuplicate(warehouse.WarehouseCode, warehouse.Id, warehouse.CustomerId) == false) return _warehouseRepository.Update(warehouse);
        //    else return false;

        //}
    }
}
