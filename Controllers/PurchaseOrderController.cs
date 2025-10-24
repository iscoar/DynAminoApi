using Microsoft.AspNetCore.Mvc;
using DynAmino.Repositories;
using DynAmino.Dtos.PurchaseOrder;

namespace DynAmino.Controllers;

[ApiController]
[Route("api/")]
public class PurchaseOrderController : ControllerBase
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ILogger<PurchaseOrderController> _logger;

    public PurchaseOrderController(IPurchaseOrderRepository purchaseOrderRepository, ILogger<PurchaseOrderController> logger)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _logger = logger;
    }

    [HttpGet("purchase-orders")]
    public async Task<ActionResult> GetNewOrdersAsync()
    {
        try
        {
            var newOrdersTask = _purchaseOrderRepository.GetPurchaseOrdersErpAsync();
            var pendingOrdersTask = _purchaseOrderRepository.GetPurchaseOrdersWebAsync();

            await Task.WhenAll(newOrdersTask, pendingOrdersTask);

            var newOrders = newOrdersTask.Result;
            var pendingOrders = pendingOrdersTask.Result;

            var result = new
            {
                new_purchase_orders = newOrders,
                pending_purchase_orders = pendingOrders
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching purchase orders.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("purchase-orders/pending")]
    public async Task<ActionResult> GetPurchaseOrdersSummaryAsync()
    {
        try
        {
            var pendingToSentOrders = await _purchaseOrderRepository.GetPendingToSentOrdersAsync();
            var result = pendingToSentOrders.Select(order => new
            {
                order.PurchaseOrderNo,
                VendorNo = order.VendorNo.Trim(),
                order.DestCostCenterNo,
                PurchaseOrderDate = order.PurchaseOrderDate.ToString("yyyy-MM-dd"),
                order.VoidFlag,
                order.AdditionalProp1,
                order.AdditionalProp2,
                order.AdditionalProp3,
                Items = order.PurchaseOrderDetails.Select(detail => new
                {
                    detail.PurchaseOrderLineNo,
                    SkuProductNo = detail.SkuProductNo.Trim(),
                    detail.PriceMode,
                    detail.PriceBasisMode,
                    detail.UnitCost,
                    detail.Units,
                    CurrencyNo = detail.CurrencyNo.Trim(),
                    detail.AdditionalProp1,
                    detail.AdditionalProp2,
                    detail.AdditionalProp3
                })
            });
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching purchase orders.");
            return StatusCode(500, "Internal server error");
        }
    }
}