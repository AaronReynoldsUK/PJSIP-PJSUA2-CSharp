using System;

namespace PJSIP_PJSUA2_CSharp
{
    #region Endpoint Event Args

    public class NatCheckStunServersCompleteEventArgs : EventArgs
    {
        public NatCheckStunServersCompleteEventArgs(OnNatCheckStunServersCompleteParam p) : base()
        {
            NatCheckStunServersCompleteParam = p;
        }

        public OnNatCheckStunServersCompleteParam NatCheckStunServersCompleteParam { get; set; }
    }

    public class NatDetectionCompleteEventArgs : EventArgs
    {
        public NatDetectionCompleteEventArgs(OnNatDetectionCompleteParam p) : base()
        {
            NatDetectionCompleteParam = p;
        }

        public OnNatDetectionCompleteParam NatDetectionCompleteParam { get; set; }
    }

    public class SelectAccountEventArgs : EventArgs
    {
        public SelectAccountEventArgs(OnSelectAccountParam p) : base()
        {
            SelectAccountParam = p;
        }

        public OnSelectAccountParam SelectAccountParam { get; set; }
    }

    public class TimerParamEventArgs : EventArgs
    {
        public TimerParamEventArgs(OnTimerParam p) : base()
        {
            TimerParam = p;
        }

        public OnTimerParam TimerParam { get; set; }
    }

    public class TransportStateEventArgs : EventArgs
    {
        public TransportStateEventArgs(OnTransportStateParam p) : base()
        {
            TransportStateParam = p;
        }

        public OnTransportStateParam TransportStateParam { get; set; }
    }

    #endregion
}
