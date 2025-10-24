using DynAmino.Dtos.PurchaseOrder;
using DynAmino.Models;

namespace DynAmino.Repositories;

public interface IPurchaseOrderRepository
{
    Task<NewOrders> GetNewOrdersAsync();
    Task<PendingOrders> GetPendingOrdersAsync();
    Task<IEnumerable<PurchaseOrderWeb>> GetPurchaseOrdersWebAsync();
    Task<IEnumerable<PurchaseOrderErp>> GetPurchaseOrdersErpAsync();
    Task<IEnumerable<PurchaseOrder>> GetPendingToSentOrdersAsync();
}