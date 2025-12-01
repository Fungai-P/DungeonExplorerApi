using DungeonExplorerApi.API.Requests;
using DungeonExplorerApi.API.Responses;
using DungeonExplorerApi.Helpers;
using DungeonExplorerApi.Repository;

namespace DungeonExplorerApi.Handlers
{
    public class MapHandler : IMapHandler
    {
        public readonly IMapRepository _repository;

        public MapHandler(IMapRepository repository)
        {
            _repository = repository;
        }

        public async Task<MapResponse> CreateMapAsync(MapRequest request)
        {
            var map = await _repository.AddMapAsync(request.Map());

            return map.Map();
        }

        public async Task<MapResponse?> GetMapByIdAsync(int id)
        {
            var map = await _repository.GetByIdAsync(id);

            return map is null ? null : map.Map();
        }

        public async Task<MapPathResponse?> GetMapPathAsync(int mapId)
        {
            return new MapPathResponse
            {
                MapId = mapId,
                Steps = new List<API.Responses.Position>
                {
                    new API.Responses.Position { X = 0, Y = 0 },
                    new API.Responses.Position { X = 1, Y = 0 },
                    new API.Responses.Position { X = 2, Y = 0 }
                },
                Length = 3
            };
        }
    }
}
