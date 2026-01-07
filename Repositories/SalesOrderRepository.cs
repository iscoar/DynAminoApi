using Dapper;
using DynAmino.Data;
using DynAmino.Dtos.SalesOrder;

namespace DynAmino.Repositories;

public class SalesOrderRepository : ISalesOrderRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<SalesOrderRepository> _logger;

    public SalesOrderRepository(IDbConnectionFactory dbConnectionFactory, ILogger<SalesOrderRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<NewOrders> GetNewOrdersAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var sql = @"
                SELECT COUNT(DISTINCT LoteId) AS Count
                FROM AGQSLAPP.dbo.NuVenAlimHdr
                WHERE status <> 'RE' AND NOT EXISTS (
                    SELECT 1 FROM NuAmOrdVtaAlimHdr WHERE salesOrderNo = LoteId
                )
            ";
            var result = await connection.QuerySingleOrDefaultAsync<NewOrders>(sql);
            return result ?? new NewOrders { Count = 0 };
        }
        catch (System.Exception ex)
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
            var sql = @"SELECT COUNT(*) AS Count FROM NuAmOrdVtaAlimHdr WHERE Envio_Amino = 0;
            ";
            var result = await connection.QuerySingleOrDefaultAsync<PendingOrders>(sql);
            return result ?? new PendingOrders { Count = 0 };
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching pending orders");
            throw;
        }
    }
}