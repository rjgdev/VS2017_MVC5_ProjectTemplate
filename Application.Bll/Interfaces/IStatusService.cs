using Application.Model;

namespace Application.Bll
{
    public interface IStatusService : IGenericService<Status>
    {
        long Add(object obj);
        bool Update(object obj);
        bool Enable(long id, string updatedBy);
    }
}