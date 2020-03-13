using LocationReporter.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationReporter.Models
{
    public interface ICommandEventConverter
    {
        MemberLocationRecordedEvent CommandToEvent(LocationReport locationReport);
    }

    public class CommandEventConverter : ICommandEventConverter
    {
        public MemberLocationRecordedEvent CommandToEvent(LocationReport locationReport)
        {
            MemberLocationRecordedEvent locationRecordedEvent = new MemberLocationRecordedEvent
            {
                Latitude = locationReport.Latitude,
                Longitude = locationReport.Longitude,
                Origin = locationReport.Origin,
                MemberID = locationReport.MemberID,
                ReportID = locationReport.ReportID,
                RecordedTime = DateTime.Now.ToUniversalTime().Ticks
            };

            return locationRecordedEvent;
        }
    }
}
