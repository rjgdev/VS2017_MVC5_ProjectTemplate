using Application.Model;

namespace Application.Bll
{
    public interface IHaulierService : IGenericService<Haulier>
    {
        long Add(object obj);
        bool Update(object obj);
        bool Enable(long id, string updatedBy);
        Haulier GetByHaulierCode(string code);
    }
}
