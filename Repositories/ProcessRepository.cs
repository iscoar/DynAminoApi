using Microsoft.EntityFrameworkCore;
using DynAmino.Data;
using DynAmino.Models;

namespace DynAmino.Repositories;

public class ProcessRepository : IProcessRepository
{
    private readonly AppDbContext _context;

    public ProcessRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Proceso>> GetAllProcessesAsync()
    {
        return await _context.Procesos.ToListAsync();
    }

    public async Task<Proceso?> GetProcessByIdAsync(int id)
    {
        return await _context.Procesos.FirstOrDefaultAsync(p => p.ID == id);
    }

    public async Task<Proceso?> UpdateProcessAsync(int id, Proceso process)
    {
        if (id != process.ID) return null;

        _context.Entry(process).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return process;
    }

    public async Task<bool> UpdateStatusProcessAsync(int id, bool status)
    {
        var process = await GetProcessByIdAsync(id);
        if (process == null) return false;

        process.Detenido = status;
        _context.Entry(process).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }
}
