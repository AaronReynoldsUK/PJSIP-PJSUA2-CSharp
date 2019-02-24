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
        private void HandleQueueItem(Account sender, EventArgs eventArgs)
        {
            AccountSC accountSC = (AccountSC)sender;
            // OR
            //AccountSC accountSC = sender as AccountSC;

            HandleQueueItem(accountSC, eventArgs);
        }

        private void HandleQueueItem(AccountSC sender, EventArgs eventArgs)
        {
            var queuedEventName = eventArgs.GetType().Name;

            AccountInfo accountInfo = null;
            try
            {
                accountInfo = sender.getInfo();
            }
            catch { accountInfo = null; }

            switch (queuedEventName)
            {
                case "RegStartedEventArgs":
                    var __RegStartedEventArgs = eventArgs as RegStartedEventArgs;

                    if (__RegStartedEventArgs.RegStartedParam.renew)
                        InvokeGUIThread(() => { frmReference.UpdateRegState("Registering..."); });
                    else
                        InvokeGUIThread(() => { frmReference.UpdateRegState("Unregistering..."); });

                    break;

                case "RegStateEventArgs":
                    var __RegStateEventArgs = eventArgs as RegStateEventArgs;

                    if (accountInfo == null)
                        InvokeGUIThread(() => { frmReference.UpdateRegState("Unknown."); frmReference.UpdatePrescence("Unknown"); });
                    else if (accountInfo.regIsActive)
                        InvokeGUIThread(() => { frmReference.UpdateRegState("Registered."); frmReference.UpdatePrescence(accountInfo.onlineStatusText); });
                    else
                        InvokeGUIThread(() => { frmReference.UpdateRegState("Unregistered."); frmReference.UpdatePrescence(accountInfo.onlineStatusText); });
                    break;
            }
        }

        private void HandleQueueItem(Call sender, EventArgs eventArgs)
        {
            Call call = (CallSC)sender;
            // OR
            //CallSC call = sender as CallSC;

            HandleQueueItem(call, eventArgs);
        }

        private void HandleQueueItem(CallSC sender, EventArgs eventArgs)
        {
            var queuedEventName = eventArgs.GetType().Name;

            CallInfo callInfo = null;
            try
            {
                callInfo = sender.getInfo();
            }
            catch { callInfo = null; }


            switch (queuedEventName)
            {
                case "CallMediaEventEventArgs":
                    break;

                case "CallMediaStateEventArgs":
                    var __call = sender as CallSC;
                    //CallInfo callInfo = __call.getInfo();
                    if (callInfo == null)
                        break;

                    var __lineSet_CallMediaStateEventArgs = GetLineByCallId(sender.getId());
                    callInfo = __lineSet_CallMediaStateEventArgs.CallInfo;

                    for (uint i = 0; i < callInfo.media.Count; i++)
                    {
                        int ii = Convert.ToInt32(i);

                        System.Diagnostics.Debug.WriteLine("Status: " + callInfo.media[ii].status);

                        switch (callInfo.media[ii].status)
                        {
                            case pjsua_call_media_status.PJSUA_CALL_MEDIA_ACTIVE:
                                __lineSet_CallMediaStateEventArgs.CallState = SimpleCallState.Active;
                                break;
                            case pjsua_call_media_status.PJSUA_CALL_MEDIA_ERROR:
                                break;
                            case pjsua_call_media_status.PJSUA_CALL_MEDIA_LOCAL_HOLD:
                                __lineSet_CallMediaStateEventArgs.CallState = SimpleCallState.OnHold;
                                break;
                            case pjsua_call_media_status.PJSUA_CALL_MEDIA_NONE:
                                break;
                            case pjsua_call_media_status.PJSUA_CALL_MEDIA_REMOTE_HOLD:
                                break;
                        }

                        if ((callInfo.media[ii].type == pjmedia_type.PJMEDIA_TYPE_AUDIO) && (__call.getMedia(i) != null))
                        {
                            //AudioMedia audioMedia = (AudioMedia)__call.getMedia(i);
                            AudioMedia audioMedia = AudioMedia.typecastFromMedia(__call.getMedia(i));

                            AudDevManager audDevManager = endpoint.audDevManager();
                            audioMedia.startTransmit(audDevManager.getPlaybackDevMedia());
                            audDevManager.getCaptureDevMedia().startTransmit(audioMedia);
                        }
                    }
                    break;

                case "CallMediaTransportStateEventArgs":
                    var __CallMediaTransportStateEventArgs = eventArgs as CallMediaTransportStateEventArgs;
                    // e.g.: state: PJSUA_MED_TP_CREATING or PJSUA_MED_TP_IDLE
                    break;


                case "CallRedirectedEventArgs":
                    break;

                case "CallReplacedEventArgs":
                    break;

                case "CallReplaceRequestEventArgs":
                    break;

                case "CallRxOfferEventArgs":
                    break;

                case "CallRxReinviteEventArgs":
                    break;

                case "CallSdpCreatedEventArgs":
                    break;

                case "CallStateEventArgs":
                    //CallInfo callInfo = (sender as CallSC).getInfo();

                    var __lineSet_CallStateEventArgs = GetLineByCallId(sender.getId());

                    if (__lineSet_CallStateEventArgs != null)
                    {
                        if (callInfo == null)
                            callInfo = __lineSet_CallStateEventArgs.CallInfo;

                        if (callInfo == null)
                        {
                            // Treat as DISCONNECTED

                            StripDownCall(sender);
                            __lineSet_CallStateEventArgs.ResetLine();
                        }
                        else
                        {
                            __lineSet_CallStateEventArgs.CallInfo = callInfo;

                            switch (callInfo.state)
                            {
                                case pjsip_inv_state.PJSIP_INV_STATE_CALLING:
                                    __lineSet_CallStateEventArgs.CallState = SimpleCallState.DiallingOut;
                                    break;

                                case pjsip_inv_state.PJSIP_INV_STATE_CONFIRMED:
                                    __lineSet_CallStateEventArgs.CallState = SimpleCallState.Active;
                                    break;

                                case pjsip_inv_state.PJSIP_INV_STATE_CONNECTING:
                                    __lineSet_CallStateEventArgs.CallState = SimpleCallState.DiallingOut;
                                    break;

                                case pjsip_inv_state.PJSIP_INV_STATE_DISCONNECTED:
                                    StripDownCall(sender);
                                    __lineSet_CallStateEventArgs.CallState = SimpleCallState.Available;
                                    __lineSet_CallStateEventArgs.ResetLine();
                                    break;

                                case pjsip_inv_state.PJSIP_INV_STATE_EARLY:
                                    __lineSet_CallStateEventArgs.CallState = SimpleCallState.RingingIn;
                                    break;

                                case pjsip_inv_state.PJSIP_INV_STATE_INCOMING:
                                    __lineSet_CallStateEventArgs.CallState = SimpleCallState.RingingIn;
                                    break;

                                case pjsip_inv_state.PJSIP_INV_STATE_NULL:
                                    __lineSet_CallStateEventArgs.CallState = SimpleCallState.None;
                                    break;
                            }
                        }
                    }
                    break;

                case "CallTransferRequestEventArgs":
                    break;

                case "CallTransferStatusEventArgs":
                    break;

                case "CallTsxStateEventArgs":
                    break;

                case "CallTxOfferEventArgs":
                    break;

                case "CreateMediaTransportEventArgs":
                    var __CreateMediaTransportEventArgs = eventArgs as CreateMediaTransportEventArgs;
                    break;

                case "CreateMediaTransportSrtpEventArgs":
                    break;

                case "DtmfDigitEventArgs":
                    break;

                case "InstantMessageEventArgs":
                    break;

                case "InstantMessageStatusEventArgs":
                    break;

                case "StreamCreatedEventArgs":
                    break;

                case "StreamDestroyedEventArgs":
                    break;

                case "TypingIndicationEventArgs":
                    break;

                default:
                    System.Diagnostics.Debug.WriteLine("Unknown Event: " + queuedEventName);
                    break;
            }
        }
    }
}
