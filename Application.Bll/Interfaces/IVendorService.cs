using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Bll.Interfaces;
using Application.Model;

namespace Application.Bll
{
    public interface IVendorService : IGenericService<Vendor>, IGenericServiceAsync<Vendor>
    {
        long Add(object obj);
        bool Update(object obj);
        bool Enable(long id, string updatedBy);

        Task<long> AddAsyc(object obj);
        Task<bool> UpdateAsync(object obj);
    }
}
