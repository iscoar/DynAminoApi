namespace DynAmino.Dtos.Dashboard;

public record DashboardDataDto(
    decimal? ExchangeRate,
    decimal? ExchangeRateErp,
    IEnumerable<object> LastestExchangeRates,
    decimal? LastExchangeRate,
    decimal? LastExchangeRatePercentage,
    int NewFormulas,
    int NewOrders,
    int NewSalesOrders,
    int PendingFormulas,
    int PendingOrders,
    int PendingSalesOrders,
    double TotalProduction,
    double TotalProductionErp
);