using System.Collections.Generic;
using Application.Model;

namespace Application.Bll
{
    public interface IDeliveryRequestLineItemService : IGenericService<DeliveryRequestLineItem>
    {
        IEnumerable<dynamic> GetLineItemsByDeliveryRequestLineId(long deliveryRequestLineId);
        IEnumerable<dynamic> GetLineItemsByDeliveryRequestId(long deliveryRequestId);
        IEnumerable<dynamic> GetLineItemsByDeliveryRequestCode(string deliveryRequestCode);

    }
}