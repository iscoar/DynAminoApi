using Microsoft.AspNetCore.Mvc;
using DynAmino.Repositories;
using DynAmino.Dtos.ExchangeRate;

namespace DynAmino.Controllers;

[ApiController]
[Route("api/")]
public class ExchanceRateController : ControllerBase
{
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly ILogger<ExchanceRateController> _logger;

    public ExchanceRateController(IExchangeRateRepository exchangeRateRepository, ILogger<ExchanceRateController> logger)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _logger = logger;
    }

    [HttpGet("exchange-rates")]
    public async Task<ActionResult> GetCurrentExchangeRateWebAsync()
    {
        try
        {
            var exchangeRatesWebTask = _exchangeRateRepository.GetExchangeRatesWebAsync();
            var exchangeRatesErpTask = _exchangeRateRepository.GetExchangeRatesErpAsync();

            await Task.WhenAll(exchangeRatesWebTask, exchangeRatesErpTask);

            var exchangeRatesWeb = exchangeRatesWebTask.Result;
            var exchangeRatesErp = exchangeRatesErpTask.Result;

            var result = new
            {
                pending_exchange_rates = exchangeRatesWeb.Select(er => new
                {
                    er.DestCurrencyNo,
                    er.SourceCurrencyNo,
                    EffectiveDate = er.EffectiveDate.ToString("dd/MM/yyyy"),
                    Rate = Math.Round(er.ExchangeRate, 4)
                }),
                new_exchange_rates = exchangeRatesErp.Select(er => new
                {
                    er.Crtd_User,
                    er.FromCuryId,
                    Crtd_DateTime = er.Crtd_DateTime.ToString("dd/MM/yyyy HH:mm:ss"),
                    EffDate = er.EffDate.ToString("dd/MM/yyyy"),
                    Rate = Math.Round(er.Rate, 4)
                })
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching new and pending exchange rates.");
            return StatusCode(500, "Internal server error");
        }
    }
}