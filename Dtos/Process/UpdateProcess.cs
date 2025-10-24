namespace DynAmino.Dtos.Process;

public class UpdateProcess
{
    public int Id { get; set; }
    public bool Enabled { get; set; }
    public string? RunTime { get; set; }
    public string Frequency { get; set; } = string.Empty;
    public double ValueTime { get; set; }
}
