namespace DynAmino.Dtos.ExchangeRate;

public class ExchangeRateWeb
{
    public DateTimeOffset EffectiveDate { get; set; }
    public required string SourceCurrencyNo { get; set; }
    public required string DestCurrencyNo { get; set; }
    public required double ExchangeRate { get; set; }
}
