using LocationService.Models;
using LocationService.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LocationService.Controllers
{
    [Route("locations/{memberId}")]
    public class LocationRecordController : Controller
    {

        private ILocationRecordRepository locationRepository;

        public LocationRecordController(ILocationRecordRepository repository)
        {
            this.locationRepository = repository;
        }

        [HttpPost]
        public IActionResult AddLocation(Guid memberId, [FromBody]LocationRecord locationRecord)
        {
            //之所以无法解析Body中的locationRecord，是因为"MemberID"写成了半个单引号，timespan是long类型，写成带小数点也不行滴
            locationRepository.Add(locationRecord);
            return this.Created($"/locations/{memberId}/{locationRecord.ID}", locationRecord);
        }

        [HttpPost("p")]
        public IActionResult PostLocation([FromBody]Liqud locationRecord)
        {
            return this.Created($"/locations/{123}/", locationRecord);
        }

        [HttpGet]
        public IActionResult GetLocationsForMember(Guid memberId)
        {
            return this.Ok(locationRepository.AllForMember(memberId));
        }

        [HttpGet("latest")]
        public IActionResult GetLatestForMember(Guid memberId)
        {
            return this.Ok(locationRepository.GetLatestForMember(memberId));
        }
    }
}
