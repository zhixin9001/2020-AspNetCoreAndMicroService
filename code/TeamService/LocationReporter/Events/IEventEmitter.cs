using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationReporter.Events
{
    public interface IEventEmitter
    {
        void EmitLocationRecordedEvent(MemberLocationRecordedEvent locationRecordedEvent);
    }
}
