using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Data.Repository
{
    public interface IDeliveryRequestLineItemRepository : IRepository<DeliveryRequestLineItem>
    {
        IEnumerable<DeliveryRequestLineItem> GetListByLineId(long lineId);
        IEnumerable<DeliveryRequestLineItem> GetListByDeliveryRequestId(long deliveryRequestId);
        IEnumerable<DeliveryRequestLineItem> GetListByDeliveryRequestCode(string deliveryRequestCode);

        bool Update(List<DeliveryRequestLineItem> list);
    }
}
