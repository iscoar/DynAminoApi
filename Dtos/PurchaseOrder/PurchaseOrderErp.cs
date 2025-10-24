namespace DynAmino.Dtos.PurchaseOrder;

public class PurchaseOrderErp
{
    public required string PurchaseOrderNo { get; set; }
    public required string VendorId { get; set; }
    public required DateTimeOffset PurchaseOrderDate { get; set; }
    public required string DestCostCenterNo { get; set; }
}