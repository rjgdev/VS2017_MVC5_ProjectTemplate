using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Data.Repository
{
    /// <summary>
    ///     Delivery Request Repository
    /// </summary>
    public interface IDeliveryRequestRepository : IRepository<DeliveryRequest>
    {
        #region Header

        //IEnumerable<DeliveryRequest> GetList(int page, int pageSize);
        IEnumerable<DeliveryRequest> GetByStatus(string status);

        //Task<IEnumerable<DeliveryRequest>> GetListAsync(int page, int pageSize);
        IQueryable<DeliveryRequest> GetList(Expression<Func<DeliveryRequest, bool>> predicate, int page, int pageSize);

        IEnumerable<DeliveryRequest> GetList(Func<DeliveryRequest, bool> predicate);

        //Task<IEnumerable<T>> GetListAsync(int page, int pageSize);
        //IQueryable<T> GetList(Expression<Func<T, bool>> predicate, int page, int pageSize);
        #endregion

        #region Lines

        /// <summary>
        ///     Add line record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>boolean</returns>
        long AddLine(DeliveryRequestLine obj);

        /// <summary>
        /// Batch add line records
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool AddLine(List<DeliveryRequestLine> obj);

        /// <summary>
        ///     Update line record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>boolean</returns>
        bool UpdateLine(DeliveryRequestLine obj);

        /// <summary>
        ///     Deletes line record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteLine(long id);

        /// <summary>
        ///     Get Line record by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeliveryRequestLine GetLineById(long id);

        /// <summary>
        ///     Get Line record list.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        //IEnumerable<DeliveryRequestLine> GetLineList(int take);


        #endregion

        #region Line Items

        /// <summary>
        ///     Add line item record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        long AddLineItem(DeliveryRequestLineItem obj);

        /// <summary>
        ///     Update line item record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool UpdateLineItem(DeliveryRequestLineItem obj);

        /// <summary>
        ///     Delete line item record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteLineItem(long id);

        /// <summary>
        ///     Get line item record by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeliveryRequestLineItem GetLineItemById(long id);

        /// <summary>
        ///     Get line item listing.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<DeliveryRequestLineItem> GetLineItemList(int take);

        #endregion
    }
}