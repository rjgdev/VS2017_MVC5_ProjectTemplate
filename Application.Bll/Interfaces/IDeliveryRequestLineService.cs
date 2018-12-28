using Application.Model;
using System.Collections.Generic;

namespace Application.Bll
{
    public interface IDeliveryRequestLineService : IGenericService<DeliveryRequestLine>
    {
        IEnumerable<DeliveryRequestLine> GetList(bool isActive, long customerId, long deliveryRequestId);
        IEnumerable<dynamic> GetListDynamic(bool isActive, long customerId, long deliveryRequestId);

        IEnumerable<DeliveryRequestLine> GetList(bool isActive, long customerId, long deliveryRequestId, int pageNo, int pageSize);
        IEnumerable<dynamic> GetListDynamic(bool isActive, long customerId, long deliveryRequestId, int pageNo, int pageSize);
    }
}