using PJSIP_PJSUA2_CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone
{
    public partial class SoftPhoneState
    {

        #region Endpoint Events

        private void Endpoint_OnNatCheckStunServersComplete(object sender, NatCheckStunServersCompleteEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Endpoint_OnNatDetectionComplete(object sender, NatDetectionCompleteEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Endpoint_OnSelectAccount(object sender, SelectAccountEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();

        }

        private void Endpoint_OnTimer(object sender, TimerParamEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Endpoint_OnTransportState(object sender, TransportStateEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        #endregion

        #region Account Events

        private void Account_OnIncomingCall(object sender, IncomingCallEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));

            var __account = sender as AccountSC;
            CallSC call = new CallSC(__account, e.IncomingCallParam.callId);
            CallOpParam callOpParam = new CallOpParam();

            var __nextAvailableLineNumber = GetNextAvailableLineNumber();
            if (__nextAvailableLineNumber == 0)
            {
                // No available lines
                callOpParam.statusCode = pjsip_status_code.PJSIP_SC_DECLINE;
                call.hangup(callOpParam);
            }
            else
            {
                var __availableLine = GetLineByNumber(__nextAvailableLineNumber);

                __availableLine.ResetLine();
                SetupCall(call);
                __availableLine.SetCall(call);

                // possibly updated by call state changes:
                __availableLine.CallState = SimpleCallState.RingingIn;
            }

            //int __activeLineNo = LineSet.GetActiveLine(this, out LineSet __activeLine);


            //calls.Add(e.IncomingCallParam.callId, call);


            ////CallOpParam callOpParam = new CallOpParam();
            ////callOpParam.statusCode = pjsip_status_code.PJSIP_SC_DECLINE;

            ////call.hangup(callOpParam);



            //CallOpParam callOpParam = new CallOpParam();
            //callOpParam.statusCode = pjsip_status_code.PJSIP_SC_OK;

            //call.answer(callOpParam);
            //
        }

        private void Account_OnIncomingSubscribe(object sender, IncomingSubscribeEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Account_OnInstantMessage(object sender, InstantMessageEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Account_OnInstantMessageStatus(object sender, InstantMessageStatusEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Account_OnMwiInfo(object sender, MwiInfoEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Account_OnRegStarted(object sender, RegStartedEventArgs e)
        {
            //throw new NotImplementedException();
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //bgQueue.Enqueue(e);
        }

        private void Account_OnRegState(object sender, RegStateEventArgs e)
        {
            //throw new NotImplementedException();
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //bgQueue.Enqueue(e);
        }

        private void Account_OnTypingIndication(object sender, TypingIndicationEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        #endregion

        #region  Call Events

        private void Call_OnCallMediaEvent(object sender, CallMediaEventEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallMediaState(object sender, CallMediaStateEventArgs e)
        {
            CallInfo callInfo = (sender as CallSC).getInfo();
            var __callLine = GetLineByCallId(callInfo.id);

            if (__callLine != null)
                __callLine.CallInfo = callInfo;

            EventQueue.Enqueue(new QueuedEvent(sender, e));

            //var __call = sender as CallSC;
            //CallInfo callInfo = __call.getInfo();

            //for (uint i = 0; i < callInfo.media.Count; i++)
            //{
            //    int ii = Convert.ToInt32(i);
            //    if ((callInfo.media[ii].type == pjmedia_type.PJMEDIA_TYPE_AUDIO) && (__call.getMedia(i) != null))
            //    {
            //        AudioMedia audioMedia = (AudioMedia)__call.getMedia(i);

            //        AudDevManager audDevManager = endpoint.audDevManager();
            //        audioMedia.startTransmit(audDevManager.getPlaybackDevMedia());
            //        audDevManager.getCaptureDevMedia().startTransmit(audioMedia);
            //    }
            //}
        }

        private void Call_OnCallMediaTransportState(object sender, CallMediaTransportStateEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private pjsip_redirect_op Call_OnCallRedirected(object sender, CallRedirectedEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));

            return (sender as CallSC).onCallRedirected(e.CallRedirectedParam);
        }

        private void Call_OnCallReplaced(object sender, CallReplacedEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallReplaceRequest(object sender, CallReplaceRequestEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallRxOffer(object sender, CallRxOfferEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallRxReinvite(object sender, CallRxReinviteEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallSdpCreated(object sender, CallSdpCreatedEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallState(object sender, CallStateEventArgs e)
        {
            CallInfo callInfo = (sender as CallSC).getInfo();
            var __callLine = GetLineByCallId(callInfo.id);

            if (__callLine != null)
                __callLine.CallInfo = callInfo;

            EventQueue.Enqueue(new QueuedEvent(sender, e));

            ////throw new NotImplementedException();
            //CallInfo callInfo = (sender as CallSC).getInfo();

            //if (callInfo.state == pjsip_inv_state.PJSIP_INV_STATE_DISCONNECTED)
            //    bgQueue.Enqueue(callInfo);
        }

        private void Call_OnCallTransferRequest(object sender, CallTransferRequestEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallTransferStatus(object sender, CallTransferStatusEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallTsxState(object sender, CallTsxStateEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCallTxOffer(object sender, CallTxOfferEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCreateMediaTransport(object sender, CreateMediaTransportEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnCreateMediaTransportSrtp(object sender, CreateMediaTransportSrtpEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnDtmfDigit(object sender, DtmfDigitEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnInstantMessage(object sender, InstantMessageEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnInstantMessageStatus(object sender, InstantMessageStatusEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnStreamCreated(object sender, StreamCreatedEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnStreamDestroyed(object sender, StreamDestroyedEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        private void Call_OnTypingIndication(object sender, TypingIndicationEventArgs e)
        {
            EventQueue.Enqueue(new QueuedEvent(sender, e));
            //throw new NotImplementedException();
        }

        #endregion
    }
}
