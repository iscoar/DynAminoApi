using DynAmino.Models;

namespace DynAmino.Repositories;

public interface IProcessRepository
{
    Task<IEnumerable<Proceso>> GetAllProcessesAsync();
    Task<Proceso?> GetProcessByIdAsync(int id);
    Task<Proceso?> UpdateProcessAsync(int id, Proceso process);
    Task<bool> UpdateStatusProcessAsync(int id, bool status);
}