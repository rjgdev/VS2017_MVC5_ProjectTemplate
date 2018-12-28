using Application.Data.Models;
using Application.Data.Repository.Interfaces;
using Application.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Data.Repository
{
    public class TransactionRepository /* ITransactionRepository*/
    {
        //private readonly ApplicationDbContext _db = new ApplicationDbContext();

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Transaction GetById(long id)
        //{
        //    return _db.Transactions.Find(id);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public bool Add(Transaction obj)
        //{
        //    try
        //    {
        //        _db.Transactions.Add(obj);
        //        _db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public bool Delete(long id)
        //{
        //    try
        //    {
        //        _db.Transactions.Remove(GetById(id));
        //        _db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public bool Update(Transaction obj)
        //{
        //    try
        //    {
        //        var transaction = GetById(obj.Id);

        //        transaction.

        //        _db.Entry(transaction).State = EntityState.Modified;
        //        _db.SaveChanges();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="take"></param>
        ///// <returns></returns>
        //public IEnumerable<Transaction> GetList(int take)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
