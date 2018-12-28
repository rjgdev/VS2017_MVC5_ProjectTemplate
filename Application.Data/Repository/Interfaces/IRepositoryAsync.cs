using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Data.Repository.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);

        Task<IEnumerable<T>> GetListAsync(int take);

        Task<long> AddAsync(T obj);

        Task<bool> UpdateAsync(T obj);

        Task<bool> DeleteAsync(long id);

        Task<IEnumerable<T>> GetAllAsync();
    }
}
