using Dapper;
using DynAmino.Data;
using DynAmino.Dtos.Formula;

namespace DynAmino.Repositories;

public class FormulaRepository : IFormulaRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<FormulaRepository> _logger;

    public FormulaRepository(IDbConnectionFactory dbConnectionFactory, ILogger<FormulaRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<NewFormulas> GetNewFormulasAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<NewFormulas>(@"SELECT COUNT(DISTINCT F.COD_PT) AS Count
                FROM IDEAVW_Formulas AS F
                WHERE NOT EXISTS (
                    SELECT 1 
                    FROM NuAmVersionFormulaDOC AS FI 
                    WHERE LTRIM(RTRIM(FI.feedFormulaNo)) = LTRIM(RTRIM(F.COD_PT COLLATE Modern_Spanish_CI_AS))
                        AND FI.versionNo = F.NUMERO_VERSION
                        AND FI.versionDate = F.FECHA_VERSION
                )
                GROUP BY F.COD_PT, F.VERSION, F.FECHA_VERSION, F.NUMERO_VERSION");
            return result ?? new NewFormulas { Count = 0 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching new orders");
            throw;
        }
    }

    public async Task<PendingFormulas> GetPendingFormulasAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<PendingFormulas>("SELECT COUNT(*) AS Count FROM NuAmVersionFormulaDoc WHERE Envio_Amino = 0");
            return result ?? new PendingFormulas { Count = 0 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching pending orders");
            throw;
        }
    }
}