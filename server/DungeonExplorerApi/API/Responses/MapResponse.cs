namespace DungeonExplorerApi.API.Responses
{
    public class MapResponse
    {
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Position Start { get; set; }
        public Position Goal { get; set; }
        public List<Position> Obstacles { get; set; }
    }
}
