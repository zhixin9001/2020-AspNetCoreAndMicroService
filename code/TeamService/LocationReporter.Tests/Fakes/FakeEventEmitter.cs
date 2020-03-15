﻿using LocationReporter.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationReporter.Tests.Fakes
{
    public class FakeEventEmitter : IEventEmitter
    {
        private List<MemberLocationRecordedEvent> memberLocationRecordedEvents;

        public FakeEventEmitter()
        {
            memberLocationRecordedEvents = new List<MemberLocationRecordedEvent>();
        }

        public void EmitLocationRecordedEvent(MemberLocationRecordedEvent locationRecordedEvent)
        {
            MemberLocationRecordedEvents.Add(locationRecordedEvent);
        }

        public List<MemberLocationRecordedEvent> MemberLocationRecordedEvents
        {
            get
            {
                return memberLocationRecordedEvents;
            }
        }
    }
}
