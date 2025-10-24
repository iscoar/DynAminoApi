namespace DynAmino.Dtos.PurchaseOrder;

public class PurchaseOrderWeb
{
    public required string PurchaseOrderNo { get; set; }
    public required string VendorNo { get; set; }
    public required string DestCostCenterNo { get; set; }
    public required DateTimeOffset PurchaseOrderDate { get; set; }
    public required string Envio_Amino { get; set; }
}