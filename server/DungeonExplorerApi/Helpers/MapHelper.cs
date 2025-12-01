using DungeonExplorerApi.API.Requests;
using DungeonExplorerApi.API.Responses;
using DungeonExplorerApi.Entities;

namespace DungeonExplorerApi.Helpers
{
    public static class MapHelper
    {
        public static Map Map(this MapRequest request)
        {
            return new Map
            {
                Width = request.Width,
                Height = request.Height,
                Start = new Entities.Position(request.Start.X, request.Start.Y),
                Goal = new Entities.Position(request.Goal.X, request.Goal.Y),
                Obstacles = request.Obstacles.Select(o => new Entities.Position(o.X, o.Y)).ToHashSet()
            };
        }

        public static MapResponse Map(this Map map)
        {
            return new MapResponse
            {
                Id = map.Id,
                Width = map.Width,
                Height = map.Height,
                Start = new API.Responses.Position
                {
                    X = map.Start.X,
                    Y = map.Start.Y
                },
                Goal = new API.Responses.Position
                {
                    X = map.Goal.X,
                    Y = map.Goal.Y
                },
                Obstacles = map.Obstacles.Select(o => new API.Responses.Position
                {
                    X = o.X,
                    Y = o.Y
                }).ToList()
            };
        }
    }
}
