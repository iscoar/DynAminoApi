namespace DynAmino.Dtos.Token;

public class TokenResponse
{
	public required string access_token { get; set; }
	public required string token_type { get; set; }
	public int not_before { get; set; }
	public int expires_in { get; set; }
	public int expires_on { get; set; }
    public required string resource { get; set; }
}