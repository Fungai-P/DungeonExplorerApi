namespace DungeonExplorerApiTests.Base
{
    public class TestMapPathModel
    {
        public int MapId { get; set; }
        public List<TestPosition> Steps { get; set; }
        public int Length { get; set; }
    }
}
