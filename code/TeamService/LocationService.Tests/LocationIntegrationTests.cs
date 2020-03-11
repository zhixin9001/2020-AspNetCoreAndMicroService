using LocationService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace LocationService.Tests
{
    public class LocationIntegrationTests
    {
        private readonly TestServer testServer;
        private readonly HttpClient testClient;
        public LocationIntegrationTests()
        {
            testServer = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>());
            testClient = testServer.CreateClient();
        }
        [Fact]
        public async void TestLocationsPostAndGet()
        {
            Guid memberId = Guid.NewGuid();
            LocationRecord locationRecord = new LocationRecord
            {
                ID = Guid.NewGuid(),
                Latitude = 1.3f,
                Altitude = 22f,
                Longitude = 4.4f,
                Timestamp = 11,
                MemberID = memberId
            };
            StringContent stringContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(locationRecord),
                UnicodeEncoding.UTF8,
                "application/json");
            // Act
            HttpResponseMessage postResponse = await testClient.PostAsync(
            $"/locations/{memberId}",
            stringContent);
            postResponse.EnsureSuccessStatusCode();

            locationRecord.Timestamp = 12;
            stringContent = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(locationRecord),
            UnicodeEncoding.UTF8,
            "application/json");
            // Act
            await testClient.PostAsync($"/locations/{memberId}", stringContent);

            var getResponse = await testClient.GetAsync($"/locations/{memberId}");
            getResponse.EnsureSuccessStatusCode();
            string raw = await getResponse.Content.ReadAsStringAsync();
            List<LocationRecord> locations = JsonConvert.DeserializeObject<List<LocationRecord>>(raw);
            Assert.Equal(2, locations.Count);

            var getLatestResponse = await testClient.GetAsync($"/locations/{memberId}/latest");
            getLatestResponse.EnsureSuccessStatusCode();
            string raw1 = await getLatestResponse.Content.ReadAsStringAsync();
            LocationRecord location = JsonConvert.DeserializeObject<LocationRecord>(raw1);
            Assert.Equal(12, location.Timestamp);
        }
    }
}