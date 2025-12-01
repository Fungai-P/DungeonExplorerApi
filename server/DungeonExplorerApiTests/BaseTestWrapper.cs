using DungeonExplorerApi.API.Requests;
using DungeonExplorerApi.Entities;
using DungeonExplorerApi.Helpers;
using DungeonExplorerApiTests.Base;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace DungeonExplorerApiTests
{
    public abstract class BaseTestWrapper
    {
        protected HttpClient client;
        protected DataContext dataContext;

        [SetUp]
        public virtual async Task Setup()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .Options;

            dataContext = new DataContext(contextOptions);
            var app = new TestApplication(_connection);

            client = app.CreateClient();

            await dataContext.Database.EnsureDeletedAsync();
            await dataContext.Database.EnsureCreatedAsync();
        }


        [TearDown]
        public virtual void TearDown()
        {
            dataContext.Database.EnsureDeleted();
            client.Dispose();
            dataContext.Dispose();
        }

        protected async Task CreateMap(TestMapModel? request = null)
        {
            request ??= defaultRequest;
            Map model = new()
            {
                Width = request.Width,
                Height = request.Height,
                Start = new DungeonExplorerApi.Entities.Position(request.Start.X, request.Start.Y),
                Goal = new DungeonExplorerApi.Entities.Position(request.Goal.X, request.Goal.Y),
                Obstacles = request.Obstacles.Select(o => new DungeonExplorerApi.Entities.Position(o.X, o.Y)).ToHashSet()
            };

            dataContext.Maps.Add(model);
            await dataContext.SaveChangesAsync();
        }

        protected TestMapModel defaultRequest = new()
        {
            Width = 10,
            Height = 10,
            Start = new TestPosition { X = 0, Y = 0 },
            Goal = new TestPosition { X = 9, Y = 9 },
            Obstacles = new List<TestPosition>
            {
                new TestPosition { X = 1, Y = 1 },
                new TestPosition { X = 2, Y = 2 },
                new TestPosition { X = 3, Y = 3 }
            }
        };
    }
}
