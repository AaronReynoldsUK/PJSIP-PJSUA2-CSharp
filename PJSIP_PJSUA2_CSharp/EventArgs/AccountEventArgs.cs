using System;

namespace PJSIP_PJSUA2_CSharp
{
    #region Account Event Args

    public class IncomingCallEventArgs : EventArgs
    {
        public IncomingCallEventArgs(OnIncomingCallParam p) : base()
        {
            IncomingCallParam = p;
        }

        public OnIncomingCallParam IncomingCallParam { get; set; }
    }

    public class IncomingSubscribeEventArgs : EventArgs
    {
        public IncomingSubscribeEventArgs(OnIncomingSubscribeParam p) : base()
        {
            IncomingSubscribeParam = p;
        }

        public OnIncomingSubscribeParam IncomingSubscribeParam { get; set; }
    }

    public class InstantMessageEventArgs : EventArgs
    {
        public InstantMessageEventArgs(OnInstantMessageParam p) : base()
        {
            InstantMessageParam = p;
        }

        public OnInstantMessageParam InstantMessageParam { get; set; }
    }

    public class InstantMessageStatusEventArgs : EventArgs
    {
        public InstantMessageStatusEventArgs(OnInstantMessageStatusParam p) : base()
        {
            InstantMessageStatusParam = p;
        }

        public OnInstantMessageStatusParam InstantMessageStatusParam { get; set; }
    }

    public class MwiInfoEventArgs : EventArgs
    {
        public MwiInfoEventArgs(OnMwiInfoParam p) : base()
        {
            MwiInfoParam = p;
        }

        public OnMwiInfoParam MwiInfoParam { get; set; }
    }

    public class RegStartedEventArgs : EventArgs
    {
        public RegStartedEventArgs(OnRegStartedParam p) : base()
        {
            RegStartedParam = p;
        }

        public OnRegStartedParam RegStartedParam { get; set; }
    }

    public class TypingIndicationEventArgs : EventArgs
    {
        public TypingIndicationEventArgs(OnTypingIndicationParam p) : base()
        {
            TypingIndicationParam = p;
        }

        public OnTypingIndicationParam TypingIndicationParam { get; set; }
    }

    public class RegStateEventArgs : EventArgs
    {
        public RegStateEventArgs(OnRegStateParam p) : base()
        {
            RegStateParam = p;
        }

        public OnRegStateParam RegStateParam { get; set; }
    }

    #endregion
}
