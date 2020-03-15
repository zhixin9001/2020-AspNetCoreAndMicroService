using LocationService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TeamService.Entities;
using Xunit;

namespace TeamService.Tests.Integration
{
    public class TeamAndLocationIntegrationTests
    {
        private readonly TestServer teamServer;
        private readonly TestServer locationServer;
        private readonly HttpClient teamClient;
        private readonly HttpClient locationClient;

        public TeamAndLocationIntegrationTests()
        {
            teamServer = new TestServer(new WebHostBuilder()
                .UseStartup<TeamService.Startup>());
            teamClient = teamServer.CreateClient();

            locationServer = new TestServer(new WebHostBuilder()
             .UseStartup<LocationService.Startup>());
            locationClient = locationServer.CreateClient();
        }

        [Fact]
        public async void TestMemberAndLocationPostAndGet()
        {
            Guid memberId = Guid.NewGuid();
            Guid teamId = Guid.NewGuid();
            //location
            LocationService.Models.LocationRecord locationRecord = new LocationService.Models.LocationRecord
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
            HttpResponseMessage postResponse = await locationClient.PostAsync(
            $"/locations/{memberId}",
            stringContent);
            postResponse.EnsureSuccessStatusCode();

            locationRecord.Timestamp = 12;
            stringContent = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(locationRecord),
            UnicodeEncoding.UTF8,
            "application/json");
            await locationClient.PostAsync($"/locations/{memberId}", stringContent);

            var getResponse = await locationClient.GetAsync($"/locations/{memberId}");
            getResponse.EnsureSuccessStatusCode();
            string raw = await getResponse.Content.ReadAsStringAsync();
            List<LocationService.Models.LocationRecord> locations = JsonConvert.DeserializeObject<List<LocationService.Models.LocationRecord>>(raw);
            Assert.Equal(2, locations.Count);

            //team
            var teamZombie = new Team()
            {
                ID = teamId,
                Name = "Zombie"
            };

            StringContent teamStringContent = new StringContent(
                  System.Text.Json.JsonSerializer.Serialize(teamZombie),
                  UnicodeEncoding.UTF8,
                  "application/json");
            HttpResponseMessage teamPostResponse = await teamClient.PostAsync(
            "/teams",
            teamStringContent);
            teamPostResponse.EnsureSuccessStatusCode();

            //member
            var member = new Member
            {
                ID = memberId,
                FirstName = "zhi",
                LastName = "xin"
            };
            StringContent memberStringContent = new StringContent(
                  System.Text.Json.JsonSerializer.Serialize(member),
                  UnicodeEncoding.UTF8,
                  "application/json");
            HttpResponseMessage memeberPostResponse = await teamClient.PostAsync(
           $"/teams/{teamId}/members",
           memberStringContent);
            memeberPostResponse.EnsureSuccessStatusCode();

            //assert
            HttpResponseMessage memeberGetResponse = await teamClient.GetAsync(
                  $"/teams/{teamId}/members/{memberId}");
            string rawMemner = await memeberGetResponse.Content.ReadAsStringAsync();
            LocatedMember located = JsonConvert.DeserializeObject<LocatedMember>(rawMemner);
            
            Assert.Equal(memberId, located.ID);
            Assert.Equal(12, located.LastLocation.Timestamp);

        }
    }
}
