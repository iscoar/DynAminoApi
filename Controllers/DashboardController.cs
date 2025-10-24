using Microsoft.AspNetCore.Mvc;
using DynAmino.Repositories;
using DynAmino.Dtos.Dashboard;

namespace DynAmino.Controllers
{
    [ApiController]
    [Route("api/")]
    public class DashboardController : ControllerBase
    {
        private readonly IFeedProductionRepository _feedProductionRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IFormulaRepository _formulaRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ILogger<DashboardController> _logger;

        [ActivatorUtilitiesConstructor]
        public DashboardController(
            IFeedProductionRepository feedProductionRepository,
            IPurchaseOrderRepository purchaseOrderRepository,
            IExchangeRateRepository exchangeRateRepository,
            IFormulaRepository formulaRepository,
            ISalesOrderRepository salesOrderRepository,
            ILogger<DashboardController> logger
        )
        {
            _feedProductionRepository = feedProductionRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _formulaRepository = formulaRepository;
            _salesOrderRepository = salesOrderRepository;
            _logger = logger;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var webProductionTask = _feedProductionRepository.GetCurrentWebProductionAsync();
                var erpProductionTask = _feedProductionRepository.GetCurrentErpProductionAsync();

                var newOrdersTask = _purchaseOrderRepository.GetNewOrdersAsync();
                var pendingOrdersTask = _purchaseOrderRepository.GetPendingOrdersAsync();

                var exchangeRateWebTask = _exchangeRateRepository.GetCurrentExchangeRateWebAsync();
                var exchangeRateErpTask = _exchangeRateRepository.GetCurrentExchangeRateErpAsync();
                var lastestExchangeRatesTask = _exchangeRateRepository.GetLastestExchangeRatesAsync();

                var newFormulasTask = _formulaRepository.GetNewFormulasAsync();
                var pendingFormulasTask = _formulaRepository.GetPendingFormulasAsync();

                var newSalesOrdersTask = _salesOrderRepository.GetNewOrdersAsync();
                var pendingSalesOrdersTask = _salesOrderRepository.GetPendingOrdersAsync();

                await Task.WhenAll(
                    webProductionTask,
                    erpProductionTask,
                    newOrdersTask,
                    pendingOrdersTask,
                    exchangeRateWebTask,
                    exchangeRateErpTask,
                    lastestExchangeRatesTask,
                    newFormulasTask,
                    pendingFormulasTask,
                    newSalesOrdersTask,
                    pendingSalesOrdersTask
                );

                var webProduction = webProductionTask.Result;
                var erpProduction = erpProductionTask.Result;

                var newOrders = newOrdersTask.Result;
                var pendingOrders = pendingOrdersTask.Result;

                var exchangeRateWeb = exchangeRateWebTask.Result;
                var exchangeRateErp = exchangeRateErpTask.Result;
                var lastestExchangeRates = lastestExchangeRatesTask.Result;

                var newFormulas = newFormulasTask.Result;
                var pendingFormulas = pendingFormulasTask.Result;

                var newSalesOrders = newSalesOrdersTask.Result;
                var pendingSalesOrders = pendingSalesOrdersTask.Result;

                var exchangeRates = lastestExchangeRates
                    .OrderByDescending(ex => ex.EffectiveDate)
                    .Select(ex => (decimal?)Math.Round((decimal)ex.ExchangeRate, 4))
                    .ToList();

                decimal? percentage = (exchangeRates.Count >= 2 && exchangeRates[0] is { } last && exchangeRates[1] is { } second && (last + second) != 0)
                    ? (last - second) / ((last + second) / 2) * 100
                    : null;

                var dashboardData = new DashboardDataDto(
                    ExchangeRate: exchangeRateWeb.Rate > 0 ? Math.Round((decimal)exchangeRateWeb.Rate, 4) : null,
                    ExchangeRateErp: exchangeRateErp.Rate > 0 ? Math.Round((decimal)exchangeRateErp.Rate, 4) : null,
                    LastestExchangeRates: lastestExchangeRates
                        .OrderBy(ex => ex.EffectiveDate)
                        .Select(ex => new { EffectiveDate = ex.EffectiveDate.ToString("dd/MM/yyyy"), ExchangeRate = Math.Round((decimal)ex.ExchangeRate, 4) }),
                    LastExchangeRate: exchangeRates.FirstOrDefault(),
                    LastExchangeRatePercentage: percentage is not null ? Math.Round(percentage.Value, 2) : null,
                    NewFormulas: newFormulas.Count,
                    NewOrders: newOrders.Count,
                    NewSalesOrders: newSalesOrders.Count,
                    PendingFormulas: pendingFormulas.Count,
                    PendingOrders: pendingOrders.Count,
                    PendingSalesOrders: pendingSalesOrders.Count,
                    TotalProduction: Math.Round(webProduction.Total, 2),
                    TotalProductionErp: Math.Round(erpProduction.Total, 2)
                );  

                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching dashboard data");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}