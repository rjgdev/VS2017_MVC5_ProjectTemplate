using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Data.Models;
using Application.Model;

namespace Application.Data.Repository
{
    public class DeliveryRequestRepository : IDeliveryRequestRepository, IDisposable
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        #region Header

        /// <inheritdoc />
        public long Add(DeliveryRequest obj)
        {
            _db.DeliveryRequests.Add(obj);
            _db.SaveChanges();

            return obj.Id;
        }

        /// <inheritdoc />
        public bool Update(DeliveryRequest obj)
        {
            //var getObj = GetById(obj.Id);
            _db.Entry(obj).State = EntityState.Modified;
            //_db.DeliveryRequests.Attach(obj);
            _db.SaveChanges();

            return true;
        }


        public IEnumerable<DeliveryRequest> GetList(Func<DeliveryRequest, bool> predicate)
        {
            return _db.DeliveryRequests.Where(predicate);
        }


        /// <inheritdoc />
        public DeliveryRequest GetById(long id)
        {
            return _db.DeliveryRequests.FirstOrDefault(x => x.Id == id);
        }

        /// <inheritdoc />
        //public IEnumerable<DeliveryRequest> GetList(int take)
        //{
        //    return _db.DeliveryRequests.Take(take);
        //}

        /// <inheritdoc />
        public IEnumerable<DeliveryRequest> GetList(int page = 0, int pageSize = 10)
        {
            _db.Configuration.LazyLoadingEnabled = false;

            if (page > 0) page--;
            else if (page < 0) page = 0;

            if (pageSize < 0) pageSize = 10;


            var list = _db.DeliveryRequests.ToList();

            var totalCount = list.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            _db.Configuration.LazyLoadingEnabled = true;

            return list.Skip(pageSize * page).Take(pageSize).ToList();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DeliveryRequest>> GetListAsync(int page = 0, int pageSize = 10)
        {
            _db.Configuration.LazyLoadingEnabled = false;

            if (page > 0) page--;
            else if (page < 0) page = 0;

            if (pageSize < 0) pageSize = 10;


            var list = await _db.DeliveryRequests.ToListAsync();

            var totalCount = list.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            _db.Configuration.LazyLoadingEnabled = true;

            return list.Skip(pageSize * page).Take(pageSize).ToList();
        }

        public IQueryable<DeliveryRequest> GetList(Expression<Func<DeliveryRequest, bool>> predicate, int page, int pageSize)
        {
            _db.Configuration.LazyLoadingEnabled = false;

            if (page > 0) page--;
            else if (page < 0) page = 0;

            if (pageSize < 0) pageSize = 10;


            var list = _db.DeliveryRequests.Where(predicate);

            var totalCount = list.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            _db.Configuration.LazyLoadingEnabled = true;

            return list.Skip(pageSize * page).Take(pageSize).AsQueryable();
        }

        public IEnumerable<DeliveryRequest> GetByStatus(string status)
        {
            var statusId = _db.Statuses.FirstOrDefault(x => x.Name.ToLower() == status.ToLower()).Id;
            return _db.DeliveryRequests.Where(x => x.StatusId == statusId).ToList();
        }

        public static Expression<Func<DeliveryRequest, bool>> RequestType(string keyword)
        {
            var predicate = PredicateBuilder.True<DeliveryRequest>();
            predicate = predicate.And(p => p.RequestType==keyword);
            return predicate;
        }

        /// <inheritdoc />
        public bool Delete(long id)
        {
            try
            {
                _db.DeliveryRequests.Remove(GetById(id));
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Lines

       
        /// <inheritdoc />
        public long AddLine(DeliveryRequestLine obj)
        {
            _db.DeliveryRequestLines.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        public bool AddLine(List<DeliveryRequestLine> obj)
        {
            _db.DeliveryRequestLines.AddRange(obj);
            _db.SaveChanges();
            return true;
        }

        /// <inheritdoc />
        public bool UpdateLine(DeliveryRequestLine obj)
        {
            //var getObj = GetById(obj.Id);
            //_db.Entry(getObj).State = System.Data.Entity.EntityState.Detached;
            _db.Entry(obj).State = EntityState.Modified;
            //_db.DeliveryRequestLines.Attach(obj);
            _db.SaveChanges();

            return true;
        }

        /// <inheritdoc />
        public bool DeleteLine(long id)
        {
            try
            {
                _db.DeliveryRequestLines.Remove(GetLineById(id));
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public DeliveryRequestLine GetLineById(long id)
        {
            return _db.DeliveryRequestLines.Find(id);
        }

        /// <inheritdoc />
        public IEnumerable<DeliveryRequestLine> GetLineList(int take)
        {
            return _db.DeliveryRequestLines.Take(take);
        }

        #endregion

        #region Line Items

        /// <inheritdoc />
        public long AddLineItem(DeliveryRequestLineItem obj)
        {
            _db.DeliveryRequestLineItems.Add(obj);
            _db.SaveChanges();
            return obj.Id;
        }

        /// <inheritdoc />
        public bool UpdateLineItem(DeliveryRequestLineItem obj)
        {
            _db.Entry(obj).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        /// <inheritdoc />
        public bool DeleteLineItem(long id)
        {
            try
            {
                _db.DeliveryRequestLineItems.Remove(GetLineItemById(id));
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public DeliveryRequestLineItem GetLineItemById(long id)
        {
            return _db.DeliveryRequestLineItems.Find(id);
        }

        /// <inheritdoc />
        public IEnumerable<DeliveryRequestLineItem> GetLineItemList(int take)
        {
            return _db.DeliveryRequestLineItems.Take(take);
        }

        public IEnumerable<DeliveryRequest> GetAll()
        {
            return _db.DeliveryRequests;
        }

        #endregion

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
        }

        public DeliveryRequest Get(Expression<Func<DeliveryRequest, bool>> predicate)
        {
            return _db.DeliveryRequests.FirstOrDefault(predicate);
        }

        public IEnumerable<DeliveryRequest> GetList(Expression<Func<DeliveryRequest, bool>> predicate)
        {
            return _db.DeliveryRequests.Where(predicate);
        }

        public void Detach(DeliveryRequest obj)
        {
            _db.Entry(obj).State = EntityState.Detached;
        }

      
    }
}