using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Bll
{
    public interface IItemService : IGenericService<Item>
    {
        dynamic GetByItemCode(string itemCode, long? customerId);
        //IEnumerable<ItemSelectListViewModel> GetSelectList(int productId);
        IEnumerable<ItemSelectListViewModel> GetSelectListByBrandId(long brandId);
        IEnumerable<ItemSelectListViewModel> GetSelectListByProductId(long brandId);
        IEnumerable<ItemSelectListViewModel> GetSelectListByProductCode(string productCode);
        long Add(object model);
        bool Update(object model);
        //IEnumerable<Item> GetAll();
        dynamic GetItemDescriptionByItemCode(long id);
        bool Enable(long id, string updatedBy);
        long AddItem(dynamic model);
    }
}
