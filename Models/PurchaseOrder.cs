using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynAmino.Models;

[Table("NuAmPurchaseOrderHdr")]
public class PurchaseOrder
{
    [Key]
    [Column("purchaseOrderNo")]
    public required string PurchaseOrderNo { get; set; }

    [Column("vendorNo")]
    public required string VendorNo { get; set; }

    [Column("destCostCenterNo")]
    public required string DestCostCenterNo { get; set; }

    [Column("purchaseOrderDate")]
    public required DateTime PurchaseOrderDate { get; set; }

    [Column("voidFlag")]
    public required int VoidFlag { get; set; }

    [Column("additionalProp1")]
    public string? AdditionalProp1 { get; set; }

    [Column("additionalProp2")]
    public string? AdditionalProp2 { get; set; }

    [Column("additionalProp3")]
    public string? AdditionalProp3 { get; set; }

    [Column("Envio_Amino")]
    public int IsSent { get; set; }

    [Column("Recibir_Amino")]
    public int IsReceived { get; set; }

    public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
}