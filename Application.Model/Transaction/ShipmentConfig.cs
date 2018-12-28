using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Model.Transaction
{
    [Table("ShipmentConfigs")]
    public class ShipmentConfig : BaseClass
    {
        [Key]
        public long Id { get; set; }

        public bool CustomerDespatch_UseProductCustomerXref { get; set; } //If this is selected then when creating a new order line will use available Product/Customer x-references if they are available (This should be selected when entering a customer product code in the product code field)

        public bool CustomerDespatch_HaulierCodeMandatory { get; set; } // If this is selected it make the haulier code a mandatory selection (haulier code must be entered/selected)

        public bool CustomerDespatch_DeleteFullfilledOrders { get; set; } //If selected removes fulfilled orders from display

        public bool CustomerDespatch_HeaderEntry_Updateable { get; set; } //If selected will allow the user to access that particular field and enter characters within it.  If unselected then this will grey out that particular field and the user will not be able to access it.

        public bool CustomerDespatch_HeaderEntry_Default { get; set; } //If a value or tick box is selected then that default value will be presented to the user every time that the function is opened.  If a value is constant and will not change for all orders it may be beneficial to enter a default value and untick the updateable box.

        public bool CustomerDespatch_HeaderEntry_OrderNumber { get; set; } //The system generated sequential number
        public bool CustomerDespatch_HeaderEntry_ExternalDoc { get; set; } //Enter an external document number

        public bool CustomerDespatch_HeaderEntry_TheirReference { get; set; } //Enter a customer reference for the order
        public bool CustomerDespatch_HeaderEntry_OrderDate { get; set; } //The date for the order

        public bool CustomerDespatch_HeaderEntry_Priority { get; set; } //Select the priority of this order
        public int CustomerDespatch_HeaderEntry_PriorityValue { get; set; }

        public bool CustomerDespatch_HeaderEntry_WarehouseFrom { get; set; } //Which warehouse is the order from

        public bool CustomerDespatch_HeaderEntry_WarehouseFromDefault { get; set; }

        public bool CustomerDespatch_HeaderEntry_Haulier { get; set; } //Haulier for the order
        public int HaulierId { get; set; }

        public bool CustomerDespatch_LineEntry_Updateable_Description { get; set; } //If selected will allow the user to access that particular field and enter characters within it.  If unselected then this will grey out that particular field and the user will not be able to access it.
        public bool CustomerDespatch_LineEntry_Default_Description { get; set; } // If a value or tick box is selected then that default value will be presented to the user every time that the function is opened.  If a value is constant and will not change for all orders it may be beneficial to enter a default value and untick the updateable box.

        public bool CustomerDespatch_LineEntry_Updateable_WarehouseFrom { get; set; } //The warehouse that the stock will be picked from

        public bool CustomerDespatch_LineEntry_Default_WarehouseFrom { get; set; }

        public bool CustomerDespatch_LineEntry_PickType { get; set; }
        public string CustomerDespatch_LineEntry_PickTypeValue { get; set; }

        public bool CustomerDespatch_LineEntry_UnitOfMeasure { get; set; }
        public int UomId { get; set; }

        public virtual Haulier Haulier { get; set; }
        public virtual Uom Uom { get; set; }
    }
}