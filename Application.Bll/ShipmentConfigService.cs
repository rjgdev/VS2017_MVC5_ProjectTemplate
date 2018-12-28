using System.Collections.Generic;
using Application.Model.Transaction;
using Application.Data.Repository;

namespace Application.Bll
{
    public class ShipmentConfigService : IShipmentConfigService
    {
        private readonly IShipmentConfigRepository _shipmentConfigRepository;

        public ShipmentConfigService()
        {
        }

        public ShipmentConfigService(IShipmentConfigRepository shipmentConfigRepository)
        {
            _shipmentConfigRepository = shipmentConfigRepository;
        }

        public ShipmentConfig GetById(long id)
        {
            return _shipmentConfigRepository.GetById(id);
        }

        //public IEnumerable<ShipmentConfig> GetList(int take)
        //{
        //    return _shipmentConfigRepository.GetList(take);
        //}

        public long Add(ShipmentConfig obj)
        {
            return _shipmentConfigRepository.Add(obj);
        }

        public bool Update(ShipmentConfig obj)
        {
            return _shipmentConfigRepository.Update(obj);
        }

        public bool Delete(long id, string updatedBy)
        {
            return _shipmentConfigRepository.Delete(id);
        }

        public IEnumerable<ShipmentConfig> GetAll()
        {
            return _shipmentConfigRepository.GetAll();
        }

        public bool IsDuplicate(string code, long id, long? customerId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ShipmentConfig> GetList(bool isActive, long customerId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ShipmentConfig> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10)
        {
            throw new System.NotImplementedException();
        }
    }
}