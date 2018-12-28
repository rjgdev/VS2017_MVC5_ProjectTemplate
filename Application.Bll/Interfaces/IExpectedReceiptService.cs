using Application.Model;
using System.Collections.Generic;
using Application.Bll.Models;
using System;

namespace Application.Bll
{
    public interface IExpectedReceiptService : IGenericService<ExpectedReceipt>
    {
        #region Header

        bool Enable(long id, string updatedBy);
        ExpectedReceipt GetByGrn(string grn);
        ExpectedReceipt GetByReferenceNo(string refNo, long customerId);
        long Add(ExpectedReceiptBindingModel bindingModel);
        bool Update(ExpectedReceiptBindingModel obj);
        IEnumerable<ExpectedReceipt> GetPlannedReceipt(bool isActive, long customerId);
        IEnumerable<ExpectedReceipt> GetUnplannedReceipt(bool isActive, long customerId);
        IEnumerable<ExpectedReceipt> GetReceipt(DateTime from, DateTime to, int take, string status, bool isActive, long customerId);
        IEnumerable<dynamic> GetForReceiving(bool isActive, long customerId);
        IEnumerable<dynamic> GetForReceivingMobile(bool isActive, long customerId);
        IEnumerable<ExpectedReceipt> GetReceived(bool isActive, long customerId);
        IEnumerable<ExpectedReceipt> GetCompleted(bool isActive, long customerId);

        bool CheckIsProcessing(long id);
        bool Onprocessing(long id, bool isprocessed, string updatedBy);
        string GenerateReferenceNo(long customerId);

        bool ReceivedExpectedReceipt(ExpectedReceiptBindingModel obj);
        #endregion Header

        #region Lines
        bool EnableLine(long id, string updatedBy);
        ExpectedReceiptLine GetLineById(int id);
        IEnumerable<ExpectedReceiptLine> GetLineList(int id);
        IEnumerable<ExpectedReceiptLine> GetLineList(int id, int take);
        bool UpdateLine(ExpectedReceiptLine expectedReceiptLine);
        bool UpdateLine(ExpectedReceiptLineBindingModel bindingModel);
        bool UpdateLine(object obj);
        bool BatchUpdate(List<object> obj);

        //bool UpdateLine(ExpectedReceiptLineBindingModel expectedReceiptLine);
        long AddLine(ExpectedReceiptLineBindingModel bindingModel);
        bool AddLine(List<ExpectedReceiptLine> bindingModel);
        long AddLine(ExpectedReceiptLine expectedReceiptLine);
        bool DeleteLine(long id, string updatedBy);
        bool IsDuplicateLine(string code, long id, long? customerId);

        #endregion Lines

    }
}
