namespace DungeonExplorerApi.API.Requests
{
    public class MapRequest
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Position Start { get; set; }
        public Position Goal { get; set; }
        public List<Position> Obstacles { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
