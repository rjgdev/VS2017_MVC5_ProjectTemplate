using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bll.Interfaces
{
    public interface IGenericServiceAsync<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        //Task<IEnumerable<T>> GetListAsync(int take);
    }
}
