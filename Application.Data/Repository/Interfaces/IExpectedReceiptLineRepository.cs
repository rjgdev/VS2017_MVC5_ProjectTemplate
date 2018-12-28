using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Data.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExpectedReceiptLineRepository : IRepository<ExpectedReceiptLine>
    {
        IEnumerable<ExpectedReceiptLine> GetLineList(int id);
        bool AddLine(List<ExpectedReceiptLine> obj);
        bool Update(List<ExpectedReceiptLine> list);

    }
}
