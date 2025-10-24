using DynAmino.Dtos.Formula;

namespace DynAmino.Repositories;

public interface IFormulaRepository
{
    Task<NewFormulas> GetNewFormulasAsync();
    Task<PendingFormulas> GetPendingFormulasAsync();
}