using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJSIP_PJSUA2_CSharp
{
    public class CallSC : Call
    {
        public CallSC(Account account) : base(account) { }

        public CallSC(Account account, int callId) : base(account, callId) { }

        #region  Event Handlers

        public event EventHandler<CallMediaEventEventArgs> OnCallMediaEvent;
        public event EventHandler<CallMediaTransportStateEventArgs> OnCallMediaTransportState;
        public event EventHandler<CallMediaStateEventArgs> OnCallMediaState;
        public event OnCallRedirectedHandler OnCallRedirected;
        public event EventHandler<CallReplacedEventArgs> OnCallReplaced;
        public event EventHandler<CallReplaceRequestEventArgs> OnCallReplaceRequest;
        public event EventHandler<CallRxOfferEventArgs> OnCallRxOffer;
        public event EventHandler<CallRxReinviteEventArgs> OnCallRxReinvite;
        public event EventHandler<CallSdpCreatedEventArgs> OnCallSdpCreated;
        public event EventHandler<CallStateEventArgs> OnCallState;
        public event EventHandler<CallTransferRequestEventArgs> OnCallTransferRequest;
        public event EventHandler<CallTransferStatusEventArgs> OnCallTransferStatus;
        public event EventHandler<CallTsxStateEventArgs> OnCallTsxState;
        public event EventHandler<CallTxOfferEventArgs> OnCallTxOffer;
        public event EventHandler<CreateMediaTransportEventArgs> OnCreateMediaTransport;
        public event EventHandler<CreateMediaTransportSrtpEventArgs> OnCreateMediaTransportSrtp;
        public event EventHandler<DtmfDigitEventArgs> OnDtmfDigit;
        public event EventHandler<InstantMessageEventArgs> OnInstantMessage;
        public event EventHandler<InstantMessageStatusEventArgs> OnInstantMessageStatus;
        public event EventHandler<StreamCreatedEventArgs> OnStreamCreated;
        public event EventHandler<StreamDestroyedEventArgs> OnStreamDestroyed;
        public event EventHandler<TypingIndicationEventArgs> OnTypingIndication;

        #endregion

        #region Overrides

        public override void onCallMediaEvent(OnCallMediaEventParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallMediaEvent?.Invoke(this, new CallMediaEventEventArgs(prm));

            base.onCallMediaEvent(prm);
        }

        public override void onCallMediaTransportState(OnCallMediaTransportStateParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallMediaTransportState?.Invoke(this, new CallMediaTransportStateEventArgs(prm));

            base.onCallMediaTransportState(prm);
        }

        public override void onCallMediaState(OnCallMediaStateParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallMediaState?.Invoke(this, new CallMediaStateEventArgs(prm));

            base.onCallMediaState(prm);
        }

        public override pjsip_redirect_op onCallRedirected(OnCallRedirectedParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            return OnCallRedirected?.Invoke(this, new CallRedirectedEventArgs(prm))
                ?? base.onCallRedirected(prm);
        }

        public override void onCallReplaced(OnCallReplacedParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallReplaced?.Invoke(this, new CallReplacedEventArgs(prm));

            base.onCallReplaced(prm);
        }

        public override void onCallReplaceRequest(OnCallReplaceRequestParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallReplaceRequest?.Invoke(this, new CallReplaceRequestEventArgs(prm));

            base.onCallReplaceRequest(prm);
        }

        public override void onCallRxOffer(OnCallRxOfferParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallRxOffer?.Invoke(this, new CallRxOfferEventArgs(prm));

            base.onCallRxOffer(prm);
        }

        public override void onCallRxReinvite(OnCallRxReinviteParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallRxReinvite?.Invoke(this, new CallRxReinviteEventArgs(prm));

            base.onCallRxReinvite(prm);
        }

        public override void onCallSdpCreated(OnCallSdpCreatedParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallSdpCreated?.Invoke(this, new CallSdpCreatedEventArgs(prm));

            base.onCallSdpCreated(prm);
        }

        public override void onCallState(OnCallStateParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallState?.Invoke(this, new CallStateEventArgs(prm));

            base.onCallState(prm);
        }

        public override void onCallTransferRequest(OnCallTransferRequestParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallTransferRequest?.Invoke(this, new CallTransferRequestEventArgs(prm));

            base.onCallTransferRequest(prm);
        }

        public override void onCallTransferStatus(OnCallTransferStatusParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallTransferStatus?.Invoke(this, new CallTransferStatusEventArgs(prm));

            base.onCallTransferStatus(prm);
        }

        public override void onCallTsxState(OnCallTsxStateParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallTsxState?.Invoke(this, new CallTsxStateEventArgs(prm));

            base.onCallTsxState(prm);
        }

        public override void onCallTxOffer(OnCallTxOfferParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCallTxOffer?.Invoke(this, new CallTxOfferEventArgs(prm));

            base.onCallTxOffer(prm);
        }

        public override void onCreateMediaTransport(OnCreateMediaTransportParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCreateMediaTransport?.Invoke(this, new CreateMediaTransportEventArgs(prm));

            base.onCreateMediaTransport(prm);
        }

        public override void onCreateMediaTransportSrtp(OnCreateMediaTransportSrtpParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnCreateMediaTransportSrtp?.Invoke(this, new CreateMediaTransportSrtpEventArgs(prm));

            base.onCreateMediaTransportSrtp(prm);
        }

        public override void onDtmfDigit(OnDtmfDigitParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnDtmfDigit?.Invoke(this, new DtmfDigitEventArgs(prm));

            base.onDtmfDigit(prm);
        }

        public override void onInstantMessage(OnInstantMessageParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnInstantMessage?.Invoke(this, new InstantMessageEventArgs(prm));

            base.onInstantMessage(prm);
        }

        public override void onInstantMessageStatus(OnInstantMessageStatusParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnInstantMessageStatus?.Invoke(this, new InstantMessageStatusEventArgs(prm));

            base.onInstantMessageStatus(prm);
        }

        public override void onStreamCreated(OnStreamCreatedParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnStreamCreated?.Invoke(this, new StreamCreatedEventArgs(prm));

            base.onStreamCreated(prm);
        }

        public override void onStreamDestroyed(OnStreamDestroyedParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnStreamDestroyed?.Invoke(this, new StreamDestroyedEventArgs(prm));

            base.onStreamDestroyed(prm);
        }

        public override void onTypingIndication(OnTypingIndicationParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnTypingIndication?.Invoke(this, new TypingIndicationEventArgs(prm));

            base.onTypingIndication(prm);
        }

        #endregion

        //public bool Refreshing { get; set; } = false;
        //public int Refreshed_CallId { get; set; }
        //public CallInfo Refreshed_CallInfo { get; set; }

    }
}
