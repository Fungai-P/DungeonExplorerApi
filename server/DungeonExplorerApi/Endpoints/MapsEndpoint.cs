using DungeonExplorerApi.API.Requests;
using DungeonExplorerApi.API.Validations;
using DungeonExplorerApi.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace DungeonExplorerApi.Endpoints
{
    public static class MapsEndpoint
    {
        public static void Map(RouteGroupBuilder api)
        {
            var maps = api.MapGroup("/maps");

            maps.MapGet("/{id}/path", async ([FromServices] IMapHandler handler, int id) =>
            {
                var map = await handler.GetMapByIdAsync(id);
                if (map is null)
                {
                    return Results.NotFound();
                }
                var mapPath = await handler.GetMapPathAsync((int)id);

                return Results.Ok(mapPath);
            });

            maps.MapGet("/{id}", async ([FromServices] IMapHandler handler, int id) =>
            {
                var map = await handler.GetMapByIdAsync(id);
                if (map is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(map);
            });

            maps.MapPost("", async ([FromServices] IMapHandler handler, MapRequest request) =>
            {
                var validation = CreateMapRequestValidator.Validate(request);
                if (!validation.IsValid)
                {
                    return Results.BadRequest(new
                    {
                        Message = validation.Message
                    });
                }

                var map = await handler.CreateMapAsync(request);

                return Results.Ok(map);
            });
        }
    }
}