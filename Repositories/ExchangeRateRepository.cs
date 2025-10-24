using Dapper;
using DynAmino.Data;
using DynAmino.Dtos.ExchangeRate;


namespace DynAmino.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<ExchangeRateRepository> _logger;

    public ExchangeRateRepository(IDbConnectionFactory dbConnectionFactory, ILogger<ExchangeRateRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<CurrentWeb> GetCurrentExchangeRateWebAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            DateTime tomorrow = DateTime.Today.AddDays(1);
            var parameters = new { effectiveDate = tomorrow.ToString("yyyyMMdd") };
            var result = await connection.QuerySingleOrDefaultAsync<CurrentWeb>(@"SELECT TOP 1 exchangeRate as Rate
                FROM NuAmCuryRate
                WHERE effectiveDate = @effectiveDate
                AND additionalProp1 = 'DOF'
                AND sourceCurrencyNo = 'USD'
                ORDER BY effectiveDate DESC", parameters);
            return result ?? new CurrentWeb { Rate = 0 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching current exchange rate (Web)");
            throw;
        }
    }

    public async Task<CurrentErp> GetCurrentExchangeRateErpAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            DateTime tomorrow = DateTime.Today.AddDays(1);
            var parameters = new { EffDate = tomorrow.ToString("yyyyMMdd") };
            var result = await connection.QuerySingleOrDefaultAsync<CurrentErp>(@"SELECT TOP 1 Rate FROM AGQSLAPP.dbo.CuryRate
                WHERE EffDate = @EffDate
                AND RateType = 'DOF'
                AND FromCuryId = 'USD'
                ORDER BY EffDate DESC", parameters);
            return result ?? new CurrentErp { Rate = 0 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching current exchange rate (ERP)");
            throw;
        }
    }

    public async Task<IEnumerable<ExchangeRateWeb>> GetExchangeRatesWebAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            DateTime tomorrow = DateTime.Today.AddDays(1);
            var parameters = new { effectiveDate = tomorrow.ToString("yyyyMMdd") };
            var result = await connection.QueryAsync<ExchangeRateWeb>(@"SELECT EffectiveDate, SourceCurrencyNo, DestCurrencyNo, ExchangeRate
                FROM NuAmCuryRate
                WHERE additionalProp1 = 'DOF'
                AND SourceCurrencyNo = 'USD'
                AND EffectiveDate >= @effectiveDate
                ORDER BY EffectiveDate DESC", parameters);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching exchange rates (Web)");
            throw;
        }
    }

    public async Task<IEnumerable<ExchangeRateErp>> GetExchangeRatesErpAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            DateTime tomorrow = DateTime.Today.AddDays(1);
            var parameters = new { EffDate = tomorrow.ToString("yyyyMMdd") };
            var result = await connection.QueryAsync<ExchangeRateErp>(@"
                SELECT EffDate, FromCuryId, Crtd_DateTime, Rate, Crtd_User
                FROM AGQSLAPP.dbo.CuryRate
                WHERE EffDate >= @EffDate
                AND RateType = 'DOF'
                AND FromCuryId = 'USD'
                ORDER BY EffDate DESC", parameters);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching exchange rates (ERP)");
            throw;
        }
    }

    public async Task<IEnumerable<ExchangeRateWeb>> GetLastestExchangeRatesAsync()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            DateTime tomorrow = DateTime.Today.AddDays(1);
            var parameters = new { effectiveDate = tomorrow.ToString("yyyyMMdd") };
            var result = await connection.QueryAsync<ExchangeRateWeb>(@"SELECT TOP 7 EffectiveDate, SourceCurrencyNo, DestCurrencyNo, ExchangeRate
                FROM NuAmCuryRate
                WHERE additionalProp1 = 'DOF'
                AND SourceCurrencyNo = 'USD'
                AND EffectiveDate <= @effectiveDate
                ORDER BY EffectiveDate DESC", parameters);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching exchange rates (Web)");
            throw;
        }
    }
}