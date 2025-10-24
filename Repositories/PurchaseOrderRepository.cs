using Dapper;
using DynAmino.Data;
using DynAmino.Dtos.PurchaseOrder;
using DynAmino.Models;
using Microsoft.EntityFrameworkCore;

namespace DynAmino.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<PurchaseOrderRepository> _logger;

    public PurchaseOrderRepository(AppDbContext context, IDbConnectionFactory dbConnectionFactory, ILogger<PurchaseOrderRepository> logger)
    {
        _context = context;
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<NewOrders> GetNewOrdersAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<NewOrders>(@"SELECT COUNT(DISTINCT h.PONbr) as Count
                FROM AGQSLAPP.dbo.PurchOrd h
                LEFT JOIN AGQSLAPP.dbo.purorddet d
                ON h.PONbr = d.PONbr
                WHERE (d.InvtID LIKE 'PMP%' OR InvtID='PPT9001')
                AND h.PODate >= '20250901'
                AND h.PODate < CONVERT(DATE,GETDATE()) 
                AND h.PONbr NOT IN (SELECT purchaseOrderNo FROM nuampurchaseorderHdr)");
            return result ?? new NewOrders { Count = 0 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching new orders");
            throw;
        }
    }

    public async Task<PendingOrders> GetPendingOrdersAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<PendingOrders>("SELECT COUNT(*) AS Count FROM NuAmPurchaseOrderHdr WHERE Envio_Amino = 0");
            return result ?? new PendingOrders { Count = 0 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching pending orders");
            throw;
        }
    }

    public async Task<IEnumerable<PurchaseOrderWeb>> GetPurchaseOrdersWebAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var result = await connection.QueryAsync<PurchaseOrderWeb>(@"SELECT purchaseOrderNo AS PurchaseOrderNo, vendorNo AS VendorNo, destCostCenterNo AS DestCostCenterNo, purchaseOrderDate AS PurchaseOrderDate, Envio_Amino
                FROM NuAmPurchaseOrderHdr
                WHERE Envio_Amino = 0
                ORDER BY purchaseOrderDate DESC");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching purchase orders (Web)");
            throw;
        }
    }

    public async Task<IEnumerable<PurchaseOrderErp>> GetPurchaseOrdersErpAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var result = await connection.QueryAsync<PurchaseOrderErp>(@"
                SELECT h.PONbr AS PurchaseOrderNo, h.VendID AS VendorId, h.PODate AS PurchaseOrderDate, 'PA-ALIM-000000' AS DestCostCenterNo
                FROM AGQSLAPP.dbo.PurchOrd h
                LEFT JOIN AGQSLAPP.dbo.purorddet d
                ON h.PONbr = d.PONbr
                WHERE (d.InvtID LIKE 'PMP%' OR d.InvtID = 'PPT9001')
                AND h.PODate >= '20250901'
                AND h.PODate < CONVERT(DATE, GETDATE()) 
                AND h.PONbr NOT IN (SELECT purchaseOrderNo FROM NuAmPurchaseOrderHdr)
                ORDER BY h.PODate DESC
            ");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching purchase orders (ERP)");
            throw;
        }
    }

    public async Task<IEnumerable<PurchaseOrder>> GetPendingToSentOrdersAsync()
    {
        try
        {
            return await _context.PurchaseOrders
                .Include(po => po.PurchaseOrderDetails)
                .Where(po => po.IsSent == 0)
                .OrderByDescending(po => po.PurchaseOrderDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching pending to sent orders");
            throw;
        }
    }
}