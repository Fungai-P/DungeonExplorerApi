using System.ComponentModel.DataAnnotations;

namespace DungeonExplorerApi.Entities
{
    public class Map
    {
        [Key]
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Position Start { get; set; }
        public Position Goal { get; set; }
        public HashSet<Position> Obstacles { get; set; } = new HashSet<Position>();
    }
}
