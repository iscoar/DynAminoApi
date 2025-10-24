using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DynAmino.Models;

[Table("NuAmPurchaseOrderDet")]
public class PurchaseOrderDetail
{
    [Column("purchaseOrderNo")]
    public required string PurchaseOrderNo { get; set; }

    [Column("purchaseOrderLineNo")]
    public required int PurchaseOrderLineNo { get; set; }

    [Column("skuProductNo")]
    public required string SkuProductNo { get; set; }

    [Column("priceMode")]
    public required int PriceMode { get; set; }

    [Column("priceBasisMode")]
    public required int PriceBasisMode { get; set; }

    [Column("unitCost")]
    public required double UnitCost { get; set; }

    [Column("units")]
    public required double Units { get; set; }

    [Column("currencyNo")]
    public required string CurrencyNo { get; set; }

    [Column("additionalProp1")]
    public string? AdditionalProp1 { get; set; }

    [Column("additionalProp2")]
    public string? AdditionalProp2 { get; set; }

    [Column("additionalProp3")]
    public string? AdditionalProp3 { get; set; }

    [JsonIgnore]
    public PurchaseOrder? PurchaseOrder { get; set; }
}