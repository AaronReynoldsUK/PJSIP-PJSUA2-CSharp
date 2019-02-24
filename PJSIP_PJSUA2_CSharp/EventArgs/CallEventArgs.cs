using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJSIP_PJSUA2_CSharp
{
    public delegate pjsip_redirect_op OnCallRedirectedHandler(object sender, CallRedirectedEventArgs e);

    #region Call Event Args

    public class CallMediaEventEventArgs : EventArgs
    {
        public CallMediaEventEventArgs(OnCallMediaEventParam p) : base()
        {
            CallMediaEventParam = p;
        }

        public OnCallMediaEventParam CallMediaEventParam { get; set; }
    }

    public class CallMediaTransportStateEventArgs : EventArgs
    {
        public CallMediaTransportStateEventArgs(OnCallMediaTransportStateParam p) : base()
        {
            CallMediaTransportStateParam = p;
        }

        public OnCallMediaTransportStateParam CallMediaTransportStateParam { get; set; }
    }

    public class CallMediaStateEventArgs : EventArgs
    {
        public CallMediaStateEventArgs(OnCallMediaStateParam p) : base()
        {
            CallMediaStateParam = p;
        }

        public OnCallMediaStateParam CallMediaStateParam { get; set; }
    }

    public class CallRedirectedEventArgs : EventArgs
    {
        public CallRedirectedEventArgs(OnCallRedirectedParam p) : base()
        {
            CallRedirectedParam = p;
        }

        public OnCallRedirectedParam CallRedirectedParam { get; set; }
    }

    public class CallReplacedEventArgs : EventArgs
    {
        public CallReplacedEventArgs(OnCallReplacedParam p) : base()
        {
            CallReplacedParam = p;
        }

        public OnCallReplacedParam CallReplacedParam { get; set; }
    }

    public class CallReplaceRequestEventArgs : EventArgs
    {
        public CallReplaceRequestEventArgs(OnCallReplaceRequestParam p) : base()
        {
            CallReplaceRequestParam = p;
        }

        public OnCallReplaceRequestParam CallReplaceRequestParam { get; set; }
    }

    public class CallRxOfferEventArgs : EventArgs
    {
        public CallRxOfferEventArgs(OnCallRxOfferParam p) : base()
        {
            CallRxOfferParam = p;
        }

        public OnCallRxOfferParam CallRxOfferParam { get; set; }
    }

    public class CallRxReinviteEventArgs : EventArgs
    {
        public CallRxReinviteEventArgs(OnCallRxReinviteParam p) : base()
        {
            CallRxReinviteParam = p;
        }

        public OnCallRxReinviteParam CallRxReinviteParam { get; set; }
    }

    public class CallSdpCreatedEventArgs : EventArgs
    {
        public CallSdpCreatedEventArgs(OnCallSdpCreatedParam p) : base()
        {
            CallSdpCreatedParam = p;
        }

        public OnCallSdpCreatedParam CallSdpCreatedParam { get; set; }
    }

    public class CallStateEventArgs : EventArgs
    {
        public CallStateEventArgs(OnCallStateParam p) : base()
        {
            CallStateParam = p;
        }

        public OnCallStateParam CallStateParam { get; set; }
    }

    public class CallTransferRequestEventArgs : EventArgs
    {
        public CallTransferRequestEventArgs(OnCallTransferRequestParam p) : base()
        {
            CallTransferRequestParam = p;
        }

        public OnCallTransferRequestParam CallTransferRequestParam { get; set; }
    }

    public class CallTransferStatusEventArgs : EventArgs
    {
        public CallTransferStatusEventArgs(OnCallTransferStatusParam p) : base()
        {
            CallTransferStatusParam = p;
        }

        public OnCallTransferStatusParam CallTransferStatusParam { get; set; }
    }

    public class CallTsxStateEventArgs : EventArgs
    {
        public CallTsxStateEventArgs(OnCallTsxStateParam p) : base()
        {
            CallTsxStateParam = p;
        }

        public OnCallTsxStateParam CallTsxStateParam { get; set; }
    }

    public class CallTxOfferEventArgs : EventArgs
    {
        public CallTxOfferEventArgs(OnCallTxOfferParam p) : base()
        {
            CallTxOfferParam = p;
        }

        public OnCallTxOfferParam CallTxOfferParam { get; set; }
    }

    public class CreateMediaTransportEventArgs : EventArgs
    {
        public CreateMediaTransportEventArgs(OnCreateMediaTransportParam p) : base()
        {
            CreateMediaTransportParam = p;
        }

        public OnCreateMediaTransportParam CreateMediaTransportParam { get; set; }
    }

    public class CreateMediaTransportSrtpEventArgs : EventArgs
    {
        public CreateMediaTransportSrtpEventArgs(OnCreateMediaTransportSrtpParam p) : base()
        {
            CreateMediaTransportSrtpParam = p;
        }

        public OnCreateMediaTransportSrtpParam CreateMediaTransportSrtpParam { get; set; }
    }

    public class DtmfDigitEventArgs : EventArgs
    {
        public DtmfDigitEventArgs(OnDtmfDigitParam p) : base()
        {
            DtmfDigitParam = p;
        }

        public OnDtmfDigitParam DtmfDigitParam { get; set; }
    }

    //public class InstantMessageEventArgs : EventArgs
    //{
    //    public InstantMessageEventArgs(OnInstantMessageParam p) : base()
    //    {
    //        InstantMessageParam = p;
    //    }

    //    public OnInstantMessageParam InstantMessageParam { get; set; }
    //}

    //public class InstantMessageStatusEventArgs : EventArgs
    //{
    //    public InstantMessageStatusEventArgs(OnInstantMessageStatusParam p) : base()
    //    {
    //        InstantMessageStatusParam = p;
    //    }

    //    public OnInstantMessageStatusParam InstantMessageStatusParam { get; set; }
    //}

    public class StreamCreatedEventArgs : EventArgs
    {
        public StreamCreatedEventArgs(OnStreamCreatedParam p) : base()
        {
            StreamCreatedParam = p;
        }

        public OnStreamCreatedParam StreamCreatedParam { get; set; }
    }

    public class StreamDestroyedEventArgs : EventArgs
    {
        public StreamDestroyedEventArgs(OnStreamDestroyedParam p) : base()
        {
            StreamDestroyedParam = p;
        }

        public OnStreamDestroyedParam StreamDestroyedParam { get; set; }
    }

    //public class TypingIndicationEventArgs : EventArgs
    //{
    //    public TypingIndicationEventArgs(OnTypingIndicationParam p) : base()
    //    {
    //        TypingIndicationParam = p;
    //    }

    //    public OnTypingIndicationParam TypingIndicationParam { get; set; }
    //}

    #endregion
}
