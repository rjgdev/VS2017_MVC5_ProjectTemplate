using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Data.Repository
{
    public interface IItemRepository : IRepository<Item>
    {
        //dynamic GetByItemCode(string itemCode, long? customerId);
        //IEnumerable<ItemSelectListViewModel> GetSelectList(long productId);
        IEnumerable<ItemSelectListViewModel> GetSelectListByBrand(long brandId);
        IEnumerable<ItemSelectListViewModel> GetSelectListByProductCode(string productCode);
        //IEnumerable<Item> GetAll();
        dynamic GetItemDescriptionByItemCode(long id);
        IEnumerable<Item> GetListByProductId(long id);
        IEnumerable<Item> GetListByBrandId(long id);

        //void Detach(Item obj);
    }
}
