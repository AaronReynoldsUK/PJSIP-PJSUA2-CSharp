using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJSIP_PJSUA2_CSharp
{
    public class EndpointSC: Endpoint
    {
        #region  Event Handlers

        public event EventHandler<NatCheckStunServersCompleteEventArgs> OnNatCheckStunServersComplete;
        public event EventHandler<NatDetectionCompleteEventArgs> OnNatDetectionComplete;
        public event EventHandler<SelectAccountEventArgs> OnSelectAccount;
        public event EventHandler<TimerParamEventArgs> OnTimer;
        public event EventHandler<TransportStateEventArgs> OnTransportState;

        #endregion

        #region Overrides

        public override void onNatCheckStunServersComplete(OnNatCheckStunServersCompleteParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnNatCheckStunServersComplete?.Invoke(this, new NatCheckStunServersCompleteEventArgs(prm));

            base.onNatCheckStunServersComplete(prm);
        }

        public override void onNatDetectionComplete(OnNatDetectionCompleteParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnNatDetectionComplete?.Invoke(this, new NatDetectionCompleteEventArgs(prm));

            base.onNatDetectionComplete(prm);
        }

        public override void onSelectAccount(OnSelectAccountParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnSelectAccount?.Invoke(this, new SelectAccountEventArgs(prm));

            base.onSelectAccount(prm);
        }

        public override void onTimer(OnTimerParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnTimer?.Invoke(this, new TimerParamEventArgs(prm));

            base.onTimer(prm);
        }

        public override void onTransportState(OnTransportStateParam prm)
        {
#if DEBUG
            DebugLogger.LogEvent(prm);
#endif

            OnTransportState?.Invoke(this, new TransportStateEventArgs(prm));

            base.onTransportState(prm);
        }

        #endregion
    }
}
