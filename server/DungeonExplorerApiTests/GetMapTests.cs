using DungeonExplorerApiTests.Base;
using System.Net;
using System.Net.Http.Json;

namespace DungeonExplorerApiTests
{
    internal class GetMapTests : BaseTestWrapper
    {
        [Test]
        public async Task TestGetMap_Success()
        {
            CreateMap();

            var id = 1;

            var response = await client.GetAsync($"/api/maps/{id}");
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<TestMapModel>();

            Assert.That(result.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task TestGetMap_WithMultipleMapsReturnsExpected_Success()
        {
            CreateMap();
            CreateMap();

            var id = 2;

            var response = await client.GetAsync($"/api/maps/{id}");
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<TestMapModel>();

            Assert.That(result.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task TestGetMap_NotFound_Fails()
        {
            var id = 20;

            var response = await client.GetAsync($"/api/maps/{id}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}