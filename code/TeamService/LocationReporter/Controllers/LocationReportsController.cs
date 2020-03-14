using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocationReporter.Events;
using LocationReporter.Models;
using LocationReporter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocationReporter.Controllers
{
    [Route("/api/members/{memberId}/locationreports")]
    public class LocationReportsController : Controller
    {
        private ICommandEventConverter converter;
        private IEventEmitter eventEmitter;
        private ITeamServiceClient teamServiceClient;


        public LocationReportsController(ICommandEventConverter converter,
            IEventEmitter eventEmitter,
            ITeamServiceClient teamServiceClient)
        {
            this.converter = converter;
            this.eventEmitter = eventEmitter;
            this.teamServiceClient = teamServiceClient;
        }

        [HttpPost]
        public ActionResult PostLocationReport(Guid memberId, [FromBody]LocationReport locationReport)
        {
            MemberLocationRecordedEvent locationRecordedEvent = converter.CommandToEvent(locationReport);
            locationRecordedEvent.TeamID = teamServiceClient.GetTeamForMember(locationReport.MemberID);
            eventEmitter.EmitLocationRecordedEvent(locationRecordedEvent);

            return this.Created($"/api/members/{memberId}/locationreports/{locationReport.ReportID}", locationReport);
        }
    }
}