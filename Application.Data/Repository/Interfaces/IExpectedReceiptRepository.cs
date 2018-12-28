using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Data.Repository
{
    public interface IExpectedReceiptRepository : IRepository<ExpectedReceipt>
    {
        ExpectedReceipt GetByGrn(string grn);
        IEnumerable<ExpectedReceipt> GetList(Func<ExpectedReceipt, bool> predicate);
        //ExpectedReceipt Get(Func<ExpectedReceipt, bool> predicate);
        //IEnumerable<ExpectedReceipt> GetCurrentReceipt(DateTime from, DateTime to, int take, string status);
        IEnumerable<ExpectedReceipt> GetReceipt(Func<ExpectedReceipt, bool> predicate);
    }
}
