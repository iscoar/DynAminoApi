namespace DynAmino.Dtos.ExchangeRate;

public class ExchangeRateErp
{
    public DateTimeOffset Crtd_DateTime { get; set; }
    public required string Crtd_User { get; set; }
    public DateTimeOffset EffDate { get; set; }
    public required string FromCuryId { get; set; }
    public required double Rate { get; set; }
}
