using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        ///     Get record by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(long id);

        /// <summary>
        ///     Get list
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        //IEnumerable<T> GetList(int take);

        /// <summary>
        ///     Add record to the database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        long Add(T obj);

        /// <summary>
        ///     Update record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Update(T obj);

        /// <summary>
        ///     Delete record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Delete(long id);

        IEnumerable<T> GetAll();

        T Get(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);

        void Detach(T obj);
        //Task<IEnumerable<T>> GetListAsync(int page, int pageSize);

        //IQueryable<T> GetList(Expression<Func<T, bool>> predicate, int page, int pageSize);

    }
}