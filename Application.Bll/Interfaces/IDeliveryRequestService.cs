using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Bll.Models;
using Application.Model;

// ReSharper disable once CheckNamespace
namespace Application.Bll
{
    public interface IDeliveryRequestService : IGenericService<DeliveryRequest>
    {
        #region Header

        long AddDeliveryRequest(object obj, out string deliveryRequestCode);
        bool UpdateDeliveryRequest(object obj);
        bool Enable(long id, string updatedBy);
        dynamic GetByIdDynamic(long id);
        IEnumerable<dynamic> GetListDynamic(bool isActive, long customerId, long statusId);
        IEnumerable<dynamic> GetAvailableListDynamic(bool isActive, long customerId, bool isProcessing);
        IEnumerable<dynamic> GetListDynamic(bool isActive, long customerId, long statusId, int pageNo, int pageSize);
        IEnumerable<dynamic> GetByStatusDynamic(string status);
        IEnumerable<dynamic> GetByStatusDynamic(bool isActive, long customerId, string status);
        IEnumerable<dynamic> GetByStatusIdDynamic(bool isActive, long customerId, long statusId);
        //IEnumerable<DeliveryRequest> GetList(int page, int pageSize);
        //Task<IEnumerable<DeliveryRequest>> GetListAsync(int page, int pageSize);
        bool DispatchDeliveryRequest(DeliveryRequestBindingModel obj);
        bool CheckIsProcessing(long id);
        bool Onprocessing(long id, bool isprocessed, string updatedBy);
        //IEnumerable<dynamic> GetDynamicList(int take);
        //IEnumerable<dynamic> GetDynamicList(int page, int pageSize);

        IEnumerable<DeliveryRequest> GetByStatus(bool isActive, long customerId, string status);
        IEnumerable<DeliveryRequest> GetByStatusId(bool isActive, long customerId, long statusId);
        #endregion

        #region Lines

        long AddDeliveryRequestLine(object obj);
        bool AddDeliveryRequestLine(List<object> obj);
        bool UpdateDeliveryRequestLine(object obj);
        bool UpdateDeliveryRequestLines(object obj);
        bool DeleteDeliveryRequestLine(long id, string updatedBy);
        bool EnableDeliveryRequestLine(long id, string updatedBy);
        DeliveryRequestLine GetDeliveryRequestLineById(long id);
        dynamic GetDeliveryRequestLineByIdDynamic(long id);
        bool BatchUpdateDeliveryRequestLine(List<DeliveryRequestLine> objs);
        //IEnumerable<DeliveryRequestLine> GetDeliveryRequestLineList(int take);

        IEnumerable<DeliveryRequestLine> GetLinesByDeliveryRequestId(long id);
        IEnumerable<dynamic> GetLinesByDeliveryRequestIdDynamic(long deliveryRequestId);

        #endregion
        
        #region Line Items

        long AddLineItem(object obj);
        bool UpdateLineItem(object obj);
        bool UpdateLineItems(List<object> obj);
        bool DeleteLineItem(long id);
        DeliveryRequestLineItem GetLineItemById(long id);

        //IEnumerable<DeliveryRequestLineItem> GetLineItemList(int take);
        IEnumerable<DeliveryRequestLineItem> GetLineItemsByLineId(long lineId);
        IEnumerable<dynamic> GetLineItemsDynamicByLineId(long lineId);

        #endregion

        #region Transaction

        bool StockAssign(int id);

        #endregion
    }
}