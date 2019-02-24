using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJSIP_PJSUA2_CSharp
{
    public class AccountSC : Account
    {
        #region  Event Handlers

        public event EventHandler<IncomingCallEventArgs> OnIncomingCall;
        public event EventHandler<IncomingSubscribeEventArgs> OnIncomingSubscribe;
        public event EventHandler<InstantMessageEventArgs> OnInstantMessage;
        public event EventHandler<InstantMessageStatusEventArgs> OnInstantMessageStatus;
        public event EventHandler<MwiInfoEventArgs> OnMwiInfo;
        public event EventHandler<RegStartedEventArgs> OnRegStarted;
        public event EventHandler<TypingIndicationEventArgs> OnTypingIndication;
        public event EventHandler<RegStateEventArgs> OnRegState;

        #endregion

        #region Overrides

        public override void onIncomingCall(OnIncomingCallParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnIncomingCall?.Invoke(this, new IncomingCallEventArgs(prm));

            base.onIncomingCall(prm);
        }

        public override void onIncomingSubscribe(OnIncomingSubscribeParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnIncomingSubscribe?.Invoke(this, new IncomingSubscribeEventArgs(prm));

            base.onIncomingSubscribe(prm);
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

        public override void onMwiInfo(OnMwiInfoParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnMwiInfo?.Invoke(this, new MwiInfoEventArgs(prm));

            base.onMwiInfo(prm);
        }

        public override void onRegStarted(OnRegStartedParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnRegStarted?.Invoke(this, new RegStartedEventArgs(prm));

            base.onRegStarted(prm);
        }

        public override void onTypingIndication(OnTypingIndicationParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnTypingIndication?.Invoke(this, new TypingIndicationEventArgs(prm));

            base.onTypingIndication(prm);
        }

        public override void onRegState(OnRegStateParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnRegState?.Invoke(this, new RegStateEventArgs(prm));

            base.onRegState(prm);
        }

        #endregion
    }
}
