using DynAmino.Dtos.FeedProduction;

namespace DynAmino.Repositories;

public interface IFeedProductionRepository
{
    // Define methods for the repository
    Task<CurrentWeb> GetCurrentWebProductionAsync();
    Task<CurrentErp> GetCurrentErpProductionAsync();
}
