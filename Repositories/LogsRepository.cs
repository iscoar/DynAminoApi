using DynAmino.Models;
using DynAmino.Data;
using Microsoft.EntityFrameworkCore;

namespace DynAmino.Repositories;

public class LogsRepository : ILogsRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<LogsRepository> _logger;

    public LogsRepository(AppDbContext context, ILogger<LogsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Log>> GetAllLogsAsync()
    {
        try
        {
            return await _context.Logs
                .OrderByDescending(l => l.LogDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching logs");
            throw;
        }
    }
}