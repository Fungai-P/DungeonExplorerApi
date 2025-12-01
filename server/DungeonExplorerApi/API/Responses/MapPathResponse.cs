namespace DungeonExplorerApi.API.Responses
{
    public class MapPathResponse
    {
        public int MapId { get; set; }
        public List<Position> Steps { get; set; }
        public int Length { get; set; }
    }
}
