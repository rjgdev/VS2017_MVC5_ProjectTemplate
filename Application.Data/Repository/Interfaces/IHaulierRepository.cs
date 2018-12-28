using Application.Model;

namespace Application.Data.Repository.Interfaces
{
    public interface IHaulierRepository : IRepository<Haulier>
    {
        Haulier GetByHaulierCode(string code);
    }
}