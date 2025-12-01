using DungeonExplorerApi.Entities;
using DungeonExplorerApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DungeonExplorerApi.Repository
{
    public class MapRepository : IMapRepository
    {
        private readonly DataContext _context;

        public MapRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Map> AddMapAsync(Map map)
        {
            _context.Maps.Add(map);

            await _context.SaveChangesAsync();

            return map;
        }

        public async Task<Map?> GetByIdAsync(int id)
        {
            return await _context.Maps
                .Include(m => m.Obstacles)
                .Include(m => m.Start)
                .Include(m => m.Goal)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
