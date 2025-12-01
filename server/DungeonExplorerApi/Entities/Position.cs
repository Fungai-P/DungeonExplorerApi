using System.ComponentModel.DataAnnotations;

namespace DungeonExplorerApi.Entities
{
    public class Position
    {
        [Key]
        public int Id { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        private Position() { }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
