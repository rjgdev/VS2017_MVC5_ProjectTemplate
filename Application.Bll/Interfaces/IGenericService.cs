using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bll
{
    public interface IGenericService<T> where T: class
    {
        T GetById(long id);
        //IEnumerable<T> GetList(int take);
        IEnumerable<T> GetList(bool isActive, long customerId);
        IEnumerable<T> GetList(bool isActive, long customerId, int pageNo = 0, int pageSize = 10);
        long Add(T obj);
        bool Update(T obj);
        bool Delete(long id, string updatedBy);
        bool IsDuplicate(string code, long id, long? customerId);
        IEnumerable<T> GetAll();
        //bool EnableById();
    }
}
