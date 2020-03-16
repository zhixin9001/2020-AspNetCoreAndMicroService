using EventProcessor.Events;

namespace EventProcessor.Queues
{
    public interface IEventEmitter
    {
        void EmitProximityDetectedEvent(ProximityDetectedEvent proximityDetectedEvent);
    }
}