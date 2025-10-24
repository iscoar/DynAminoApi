using DynAmino.Dtos.ExchangeRate;

namespace DynAmino.Repositories;

public interface IExchangeRateRepository
{
    Task<CurrentWeb> GetCurrentExchangeRateWebAsync();
    Task<CurrentErp> GetCurrentExchangeRateErpAsync();
    Task<IEnumerable<ExchangeRateWeb>> GetExchangeRatesWebAsync();
    Task<IEnumerable<ExchangeRateErp>> GetExchangeRatesErpAsync();
    Task<IEnumerable<ExchangeRateWeb>> GetLastestExchangeRatesAsync();
}