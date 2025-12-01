namespace DungeonExplorerApiTests.Base
{
    public class TestMapModel
    {
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public TestPosition Start { get; set; }
        public TestPosition Goal { get; set; }
        public List<TestPosition> Obstacles { get; set; }
    }
}
