using Application.Model;
using Application.Bll.Models;

namespace Application.Bll
{
    public interface ITransactionTypeService : IGenericService<TransactionType>
    {
        long Add(TransactionTypeBindingModel obj);
        long Add(object obj);

        bool Update(object obj);
        bool Enable(long id, string updatedBy);

    }
}