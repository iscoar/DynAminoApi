using DynAmino.Models;

namespace DynAmino.Repositories;

public interface ILogsRepository
{
    public Task<IEnumerable<Log>> GetAllLogsAsync();
}