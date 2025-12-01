using DungeonExplorerApi.API.Requests;
using DungeonExplorerApi.API.Responses;

namespace DungeonExplorerApi.Handlers
{
    public interface IMapHandler
    {
        Task<MapResponse> CreateMapAsync(MapRequest request);
        Task<MapResponse?> GetMapByIdAsync(int id);
        Task<MapPathResponse?> GetMapPathAsync(int mapId);
    }
}
