using System;

namespace SoftPhone
{
    public class QueuedEvent
    {
        public QueuedEvent() { }

        public QueuedEvent(object sender, EventArgs eventArgs)
        {
            this.Sender = sender;
            this.EventArgs = eventArgs;
        }

        public object Sender { get; set; }
        public EventArgs EventArgs { get; set; }
    }
}
