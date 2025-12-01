using DungeonExplorerApiTests.Base;
using System.Net;
using System.Net.Http.Json;

namespace DungeonExplorerApiTests
{
    internal class GetMapPathTests : BaseTestWrapper
    {
        [Test]
        public async Task TestGetMapPath_Success()
        {
            CreateMap();

            var id = 1;

            var response = await client.GetAsync($"/api/maps/{id}/path");
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<TestMapPathModel>();

            Assert.That(result.MapId, Is.EqualTo(id));
            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result.Steps.Count, Is.EqualTo(3));

            var step = result.Steps[0];
            Assert.That(step.X, Is.EqualTo(0));
            Assert.That(step.Y, Is.EqualTo(0));

            step = result.Steps[1];
            Assert.That(step.X, Is.EqualTo(1));
            Assert.That(step.Y, Is.EqualTo(0));

            step = result.Steps[2];
            Assert.That(step.X, Is.EqualTo(2));
            Assert.That(step.Y, Is.EqualTo(0));
        }

        [Test]
        public async Task TestGetMapPath_NotFound_Fails()
        {
            var id = 20;

            var response = await client.GetAsync($"/api/maps/{id}/path");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}