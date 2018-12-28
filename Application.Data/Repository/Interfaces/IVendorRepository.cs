using Application.Data.Repository.Interfaces;
using Application.Model;

namespace Application.Data.Repository
{
    public interface IVendorRepository : IRepository<Vendor>, IAsyncRepository<Vendor>
    {
    }
}