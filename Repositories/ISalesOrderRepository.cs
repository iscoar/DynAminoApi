using DynAmino.Dtos.SalesOrder;

namespace DynAmino.Repositories;

public interface ISalesOrderRepository
{
    Task<NewOrders> GetNewOrdersAsync();
    Task<PendingOrders> GetPendingOrdersAsync();
}
