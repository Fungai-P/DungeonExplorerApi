using DungeonExplorerApiTests.Base;
using System.Net;
using System.Net.Http.Json;
using System.Numerics;

namespace DungeonExplorerApiTests
{
    public class CreateMapTests : BaseTestWrapper
    {
        [Test]
        public async Task TestCreateMap_Success()
        {
            var expectedId = 1;
            TestMapModel request = new()
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

            var response = await client.PostAsJsonAsync("/api/maps", request);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<TestMapModel>();

            Assert.That(result.Id, Is.EqualTo(expectedId));
            Assert.That(result.Width, Is.EqualTo(request.Width));
            Assert.That(result.Height, Is.EqualTo(request.Height));
            Assert.That(result.Start.X, Is.EqualTo(request.Start.X));
            Assert.That(result.Start.Y, Is.EqualTo(request.Start.Y));
            Assert.That(result.Goal.X, Is.EqualTo(request.Goal.X));
            Assert.That(result.Goal.Y, Is.EqualTo(request.Goal.Y));

            var obstacle = result.Obstacles[0];
            Assert.That(obstacle.X, Is.EqualTo(request.Obstacles[0].X));
            Assert.That(obstacle.Y, Is.EqualTo(request.Obstacles[0].Y));

            obstacle = result.Obstacles[1];
            Assert.That(obstacle.X, Is.EqualTo(request.Obstacles[1].X));
            Assert.That(obstacle.Y, Is.EqualTo(request.Obstacles[1].Y));

            obstacle = result.Obstacles[2];
            Assert.That(obstacle.X, Is.EqualTo(request.Obstacles[2].X));
            Assert.That(obstacle.Y, Is.EqualTo(request.Obstacles[2].Y));
        }

        [Test]
        public async Task TestCreateMap_WidthOutsideRange_Fail()
        {
            var expectedMessage = "{\"message\":\"Width cannot be less than 5 or greater than 50.\"}";

            TestMapModel request = new()
            {
                Width = 1, // Below minimum of 5
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

            var response = await client.PostAsJsonAsync("/api/maps", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Is.EqualTo(expectedMessage));
        }

        [Test]
        public async Task TestCreateMap_HeightOutsideRange_Fail()
        {
            var expectedMessage = "{\"message\":\"Height cannot be less than 5 or greater than 50.\"}";

            TestMapModel request = new()
            {
                Width = 10,
                Height = 60, // Height above max 50
                Start = new TestPosition { X = 0, Y = 0 },
                Goal = new TestPosition { X = 9, Y = 9 },
                Obstacles = new List<TestPosition>
                {
                    new TestPosition { X = 1, Y = 1 },
                    new TestPosition { X = 2, Y = 2 },
                    new TestPosition { X = 3, Y = 3 }
                }
            };

            var response = await client.PostAsJsonAsync("/api/maps", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Is.EqualTo(expectedMessage));
        }

        [Test]
        public async Task TestCreateMap_StartOutsideGrid_Fail()
        {
            var expectedMessage = "{\"message\":\"Start must be inside the grid.\"}";

            TestMapModel request = new()
            {
                Width = 10,
                Height = 50,
                Start = new TestPosition { X = -1, Y = 0 }, // Start X is negative, off grid
                Goal = new TestPosition { X = 9, Y = 9 },
                Obstacles = new List<TestPosition>
                {
                    new TestPosition { X = 1, Y = 1 },
                    new TestPosition { X = 2, Y = 2 },
                    new TestPosition { X = 3, Y = 3 }
                }
            };

            var response = await client.PostAsJsonAsync("/api/maps", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Is.EqualTo(expectedMessage));
        }

        [Test]
        public async Task TestCreateMap_GoalOutsideGrid_Fail()
        {
            var expectedMessage = "{\"message\":\"Goal must be inside the grid.\"}";

            TestMapModel request = new()
            {
                Width = 10,
                Height = 50,
                Start = new TestPosition { X = 0, Y = 0 },
                Goal = new TestPosition { X = 19, Y = 9 }, // Goal X exceeds width of 10
                Obstacles = new List<TestPosition>
                {
                    new TestPosition { X = 1, Y = 1 },
                    new TestPosition { X = 2, Y = 2 },
                    new TestPosition { X = 3, Y = 3 }
                }
            };

            var response = await client.PostAsJsonAsync("/api/maps", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Is.EqualTo(expectedMessage));
        }

        [Test]
        public async Task TestCreateMap_StartCannotBeAnObstacle_Fail()
        {
            var expectedMessage = "{\"message\":\"Start cannot be an obstacle.\"}";

            TestMapModel request = new()
            {
                Width = 10,
                Height = 50,
                Start = new TestPosition { X = 1, Y = 1 }, // Start on obstacle
                Goal = new TestPosition { X = 9, Y = 9 },
                Obstacles = new List<TestPosition>
                {
                    new TestPosition { X = 1, Y = 1 },
                    new TestPosition { X = 2, Y = 2 },
                    new TestPosition { X = 3, Y = 3 }
                }
            };

            var response = await client.PostAsJsonAsync("/api/maps", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Is.EqualTo(expectedMessage));
        }

        [Test]
        public async Task TestCreateMap_GoalCannotBeAnObstacle_Fail()
        {
            var expectedMessage = "{\"message\":\"Goal cannot be an obstacle.\"}";

            TestMapModel request = new()
            {
                Width = 10,
                Height = 50,
                Start = new TestPosition { X = 0, Y = 0 },
                Goal = new TestPosition { X = 2, Y = 2 }, // Goal on obstacle
                Obstacles = new List<TestPosition>
                {
                    new TestPosition { X = 1, Y = 1 },
                    new TestPosition { X = 2, Y = 2 },
                    new TestPosition { X = 3, Y = 3 }
                }
            };

            var response = await client.PostAsJsonAsync("/api/maps", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Is.EqualTo(expectedMessage));
        }

        [Test]
        public async Task TestCreateMap_ObstacleOutsideBounds_Fail()
        {
            var expectedMessage = "{\"message\":\"Obstacle outside bounds.\"}";

            TestMapModel request = new()
            {
                Width = 10,
                Height = 50,
                Start = new TestPosition { X = 0, Y = 0 },
                Goal = new TestPosition { X = 9, Y = 9 },
                Obstacles = new List<TestPosition>
                {
                    new TestPosition { X = 1, Y = 1 },
                    new TestPosition { X = 12, Y = 2 }, // Obstacle X exceeds width of 10
                    new TestPosition { X = 3, Y = 3 }
                }
            };

            var response = await client.PostAsJsonAsync("/api/maps", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var json = await response.Content.ReadAsStringAsync();
            Assert.That(json, Is.EqualTo(expectedMessage));
        }
    }
}
