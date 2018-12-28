using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Model.Transaction;

namespace Application.Model
{
    [Table("DeliveryRequests")]
    public class DeliveryRequest : BaseClass
    {
        [Key]
        [Column("Id", TypeName = "bigint", Order = 0)]
        public long Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [Column("DeliveryRequestCode", TypeName = "varchar", Order = 1)]
        public string DeliveryRequestCode { get; set; }

        [StringLength(50)]
        [Column("RequestType", TypeName = "varchar", Order = 2)]
        public string RequestType { get; set; }

        [Column("RequestedDate", TypeName = "datetime2", Order = 3)]
        public DateTime? RequestedDate { get; set; }

        [Column("RequiredDeliveryDate", TypeName = "datetime2", Order = 4)]
        public DateTime? RequiredDeliveryDate { get; set; }

        [Column("HaulierId", TypeName = "bigint", Order = 5)]
        public long? HaulierId { get; set; }

        [StringLength(50)]
        [Column("ServiceCode", TypeName = "varchar", Order = 6)]
        public string ServiceCode { get; set; }

        [StringLength(50)]
        [Column("CustomerRef", TypeName = "varchar", Order = 7)]
        public string CustomerRef { get; set; }

        [Column("RequiredDate", TypeName = "datetime2", Order = 8)]
        public DateTime RequiredDate { get; set; }

        [Column("EarliestDate", TypeName = "datetime2", Order = 9)]
        public DateTime EarliestDate { get; set; }

        [Column("LatestDate", TypeName = "datetime2", Order = 10)]
        public DateTime LatestDate { get; set; }

        [StringLength(50)]
        [Column("SalesOrderRef", TypeName = "varchar", Order = 11)]
        public string SalesOrderRef { get; set; }

        [Column("WarehouseId", TypeName = "bigint", Order = 12)]
        public long WarehouseId { get; set; }

        [Column("Priority", TypeName = "int", Order = 13)]
        public int Priority { get; set; }

        [Column("IsFullfilled", TypeName = "bit", Order = 14)]
        public bool IsFullfilled { get; set; }

        [Column("CustomerClientId", TypeName = "bigint", Order = 15)]
        public long? CustomerClientId { get; set; }

        [StringLength(150)]
        [Column("DespatchedBy", TypeName = "varchar", Order = 16)]
        public string DespatchedBy { get; set; }

        [StringLength(150)]
        [Column("PickedBy", TypeName = "varchar", Order = 17)]
        public string PickedBy { get; set; }

        [Column("Despatched", TypeName = "bit", Order = 18)]
        public bool Despatched { get; set; }

        [Column("DespatchedDate", TypeName = "datetime2", Order = 19)]
        public DateTime? DespatchedDate { get; set; }

        public long? StatusId { get; set; }

        public bool IsProcessing { get; set; }

        public virtual Status Status { get; set; }
        public virtual CustomerClient CustomerClient { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Haulier Haulier { get; set; }
        public virtual ICollection<DeliveryRequestLine> DeliveryRequestLines { get; set; }
    }

    [Table("DeliveryRequestLines")]
    public class DeliveryRequestLine : BaseClass
    {
        [Key]
        [Column("Id", TypeName = "bigint", Order = 0)]
        public long Id { get; set; }

        [Column("DeliveryRequestId", TypeName = "Bigint", Order = 1)]
        public long DeliveryRequestId { get; set; }

        [Column("ProductId", TypeName = "bigint", Order = 2)]
        public long ProductId { get; set; }

        [Column("BrandId", TypeName = "bigint", Order = 3)]
        public long? BrandId { get; set; }

        [Column("UomId", TypeName = "bigint", Order = 4)]
        public long UomId { get; set; }

        public long? PickTypeId { get; set; }

        [Column("Quantity", TypeName = "Int", Order = 6)]
        public int Quantity { get; set; }

        [Column("SpecialInstructions", TypeName = "nvarchar(max)", Order = 7)]
        public string SpecialInstructions { get; set; }

        [Column("Memo", TypeName = "Nvarchar(max)", Order = 8)]
        public string Memo { get; set; }

        public long? ItemId { get; set; }

        public bool IsItemExist { get; set; }

        public long? StatusId { get; set; }

        public virtual Status Status { get; set; }
        public virtual Item Item { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Product Product { get; set; }
        public virtual Uom Uom { get; set; }
        public virtual DeliveryRequest DeliveryRequest { get; set; }
        public virtual PickType PickType { get; set; }

        public virtual ICollection<DeliveryRequestLineItem> DeliveryRequestLineItems { get; set; }
    }

    [Table("DeliveryRequestLineItems")]
    public class DeliveryRequestLineItem : BaseClass
    {
        [Key]
        [Column("Id", TypeName = "Bigint", Order = 0)]
        public long Id { get; set; }

        [Column("DeliveryRequestLineId", TypeName = "bigint", Order = 1)]
        [Index("IX_DeliveryRequestLineId_ItemId",  IsUnique = true, Order = 0)]
        public long DeliveryRequestLineId { get; set; }

        [Column("ItemId", TypeName = "bigint", Order = 2)]
        [Index("IX_DeliveryRequestLineId_ItemId",  IsUnique = true, Order = 1)]
        public long? ItemId { get; set; }

        [StringLength(50)]
        [Column("Status", TypeName = "varchar", Order = 3)]
        public string Status { get; set; }

        public virtual Item Item { get; set; }
        public virtual DeliveryRequestLine DeliveryRequestLine { get; set; }
    }
}