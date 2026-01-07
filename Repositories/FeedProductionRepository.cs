using Dapper;
using DynAmino.Data;
using DynAmino.Dtos.FeedProduction;

namespace DynAmino.Repositories;

class FeedProductionRepository : IFeedProductionRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<FeedProductionRepository> _logger;

    public FeedProductionRepository(IDbConnectionFactory dbConnectionFactory, ILogger<FeedProductionRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<CurrentWeb> GetCurrentWebProductionAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            DateTime yesterday = DateTime.Today.AddDays(-1);
            const string sql = @"SELECT SUM(totalFeedProduction) AS Total
                FROM NuAmProdAliHdr WHERE productionDate = @productionDate";
            var parameters = new { productionDate = yesterday.ToString("yyyyMMdd") };
            var result = await connection.QuerySingleOrDefaultAsync<CurrentWeb>(sql, parameters);
            return result ?? new CurrentWeb { Total = 0 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching current web production");
            throw;
        }
    }

    public async Task<CurrentErp> GetCurrentErpProductionAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            connection.Execute("SET ARITHABORT ON;");
            DateTime yesterday = DateTime.Today.AddDays(-1);
            var sql = @"SELECT Produccion AS Total 
                FROM AUGIVW_ProdAlimento WHERE Fecha = @Fecha";
            var parameters = new { Fecha = yesterday.ToString("yyyyMMdd") };
            var resultAugi = await connection.QuerySingleOrDefaultAsync<CurrentErp>(sql, parameters);

            sql = @"SELECT * FROM IDEAFN_TotalProdAlimento(@Fecha, @Fecha);";
            var resultIdeas = await connection.QuerySingleOrDefaultAsync<CurrentErp>(sql, parameters);
            var result = new CurrentErp { Total = (resultAugi?.Total ?? 0) + (resultIdeas?.Total ?? 0) };
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching current ERP production");
            throw;
        }
    }
}