using DungeonExplorerApi.Entities;

namespace DungeonExplorerApi.Repository
{
    public interface IMapRepository
    {
        Task<Map?> GetByIdAsync(int Id);
        Task<Map> AddMapAsync(Map map);
    }
}
