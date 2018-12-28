using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using Application.Data.Repository;
using Application.Model;
using System.Linq;

namespace Application.Bll
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        public LocationService(ILocationRepository locationRepository,
            IWarehouseRepository warehouseRepository)
        {
            _locationRepository = locationRepository;
            _warehouseRepository = warehouseRepository;

        }

        public dynamic GetLocationById(int id)
        {
            //dynamic retVal = null;

            var location = _locationRepository.GetById(id);
            //if (location != null)
            //{
            //    retVal = new ExpandoObject();

            //    retVal.Id = location.Id;
            //    retVal.Description = location.Description;
            //    retVal.WarehouseId = location.WarehouseId;
            //    retVal.WarehouseDescription = location.Warehouse.Description;
            //    retVal.Order = location.Order;
            //}

            return location;
        }

        //public IEnumerable<dynamic> GetDynamicList(int take)
        //{
        //    //var retVal = new List<dynamic>();

        //    //var list = _locationRepository.GetList(take);
        //    //foreach (var item in list)
        //    //    retVal.Add(new
        //    //    {
        //    //        item.Id,
        //    //        item.Order,
        //    //        item.Description,
        //    //        WarehouseDescription = item.Warehouse?.Description ?? "",
        //    //    });

        //    //return retVal;
        //    return _locationRepository.GetList(take);
        //}

        public Location GetById(long id)
        {
            return _locationRepository.GetById(id);
        }

        //public IEnumerable<Location> GetList(int take)
        //{
        //   return _locationRepository.GetList(take);
        //}

        public bool Update(Location obj)
        {
            if (IsDuplicate(obj.Description, obj.Id, obj.CustomerId) == false) return _locationRepository.Update(obj);
            else return false;
        }

        public bool Delete(long id, string updatedBy)
        {
            var location = _locationRepository.GetById(id);
            location.IsActive = false;
            location.UpdatedBy = updatedBy;
            return _locationRepository.Update(location);
        }

        public bool Enable(long id, string updatedBy)
        {
            var location = _locationRepository.GetById(id);
            location.IsActive = true;
            location.UpdatedBy = updatedBy;
            return _locationRepository.Update(location);
        }

        public long Add(Location obj)
        {

            if (IsDuplicate(obj.Description, obj.Id, obj.CustomerId) == false) return _locationRepository.Add(obj);
            else
            {
                Expression<Func<Location, bool>> res = x => x.Description == obj.Description && x.CustomerId == obj.CustomerId && x.IsActive == false;
                var model = _locationRepository.Get(res);

                if(model != null)
                {
                    obj.Id = model.Id;
                    obj.IsActive = true;

                    _locationRepository.Detach(model);

                    _locationRepository.Update(obj);
                    return obj.Id;
                }
                else return 0;
            }
        }

        public IEnumerable<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

        public Location GetByLocationCode(string code)
        {
            return _locationRepository.GetByLocationCode(code);
        }

        public IEnumerable<Location> GetByWarehouseCode(string warehouseCode, bool isActive, long customerId)
        {
            Expression<Func<Warehouse, bool>> resWarehouse = x => x.WarehouseCode.ToLower() == warehouseCode && x.IsActive == isActive && x.CustomerId == customerId;
            var wareHouse = _warehouseRepository.Get(resWarehouse);

            Expression<Func<Location, bool>> res = x => x.WarehouseId == wareHouse.Id;
            return _locationRepository.GetList(res);
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            Expression<Func<Location, bool>> res;
            //Change location code to description
            if (id == 0) res = x => x.Description.ToLower() == code.ToLower() && x.CustomerId == customerId;
            //Change location code to description
            else res = x => x.Description.ToLower() == code.ToLower() && x.Id != id && x.CustomerId == customerId;

            return _locationRepository.Get(res) != null;
        }

        public IEnumerable<Location> GetList(bool isActive, long customerId)
        {
            Expression<Func<Location, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _locationRepository.GetList(res);
        }

        public IEnumerable<Location> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            Expression<Func<Location, bool>> res = x => x.IsActive == isActive && x.CustomerId == customerId;
            return _locationRepository.GetList(res).Skip(pageNo * pageSize).Take(pageSize);
        }
    }
}