using System.Collections.Generic;
using Application.Model;

namespace Application.Data.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUomRepository : IRepository<Uom>
    {
        Uom GetByDescription(string desc);
        IEnumerable<UomSelectListViewModel> GetSelectList();
    }
}