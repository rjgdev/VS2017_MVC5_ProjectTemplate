using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Bll.Models;
using Application.Model;

namespace Application.Bll
{
    public interface IExpectedReceiptLineService : IGenericService<ExpectedReceiptLine>
    {
        long AddLineItem(ItemBindingModel model, long expectedReceiptLineId);
        bool BatchUpdatExpectedReceiptLine(List<dynamic> objs);

        IEnumerable<ExpectedReceiptLine> GetByExpectedReceipt(long expectedReceiptid, bool isActive, long customerId);
        IEnumerable<ExpectedReceiptLine> GetByExpectedReceipt(long expectedReceiptid, bool isActive, long customerId, int pageNo, int pageSize);
        IEnumerable<dynamic> GetByExpectedReceiptMobile(long expectedReceiptid, bool isActive, long customerId);
    }
}
