using Microsoft.AspNetCore.Mvc;
using DynAmino.Repositories;

[ApiController]
[Route("api/")]
public class LogController : ControllerBase
{
    private readonly ILogsRepository _logsRepository;

    public LogController(ILogsRepository logsRepository)
    {
        _logsRepository = logsRepository;
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetAllLogs()
    {
        var logs = await _logsRepository.GetAllLogsAsync();
        return Ok(logs);
    }
}