using PJSIP_PJSUA2_CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoftPhone
{
    public partial class SoftPhoneState
    {
        public int MaxLines { get; private set; } = 3;

        private bool stopWorkerThread { get; set; } = false;
        private static IGuiForm frmReference = null;

        public EndpointSC endpoint { get; private set; }
        public List<AccountSC> accounts { get; private set; } = new List<AccountSC>();

        public LineSet[] Lines = null;
        //public int ActiveCallId { get; private set; } = -1;
        //public Dictionary<int, CallSC> calls { get; private set; } = new Dictionary<int, CallSC>();
        //public Dictionary<int, CallInfo> callInfo { get; private set; } = new Dictionary<int, CallInfo>();

        public Queue<QueuedEvent> EventQueue = new Queue<QueuedEvent>();
        public Queue<Action> ActionQueue = new Queue<Action>();

        public AudioMediaPlayer audioMediaPlayer = null;

        public void SetFormReference(IGuiForm guiForm)
        {
            SoftPhoneState.frmReference = guiForm;
        }

        public void WorkerOperation()
        {
            if (frmReference == null)
                throw new Exception("You must specify the GUI form");

            EventQueue.Clear();

            // Check for account info and conf first...

            // Then try to establish connection...
            frmReference.CanClose = false;
            Initialise();

            HandleQueue();

            Lines = new LineSet[MaxLines + 1];
            for (uint i = 1; i <= MaxLines; i++)
                Lines[i] = new LineSet(this, i) { CallState = SimpleCallState.Available };

            while (true)
            {
                Thread.Sleep(50);

                HandleQueue();

                if (stopWorkerThread)
                    break;
            }

            Finalise();
            frmReference.CanClose = true;
            InvokeGUIThread(() => { frmReference.CloseNow(); });
        }

        public void StopWorkerThread()
        {
            stopWorkerThread = true;
        }

        public void Initialise()
        {
            // Create endpoint
            endpoint = new EndpointSC();
            endpoint.libCreate();

            endpoint.OnNatCheckStunServersComplete += Endpoint_OnNatCheckStunServersComplete;
            endpoint.OnNatDetectionComplete += Endpoint_OnNatDetectionComplete;
            endpoint.OnSelectAccount += Endpoint_OnSelectAccount;
            endpoint.OnTimer += Endpoint_OnTimer;
            endpoint.OnTransportState += Endpoint_OnTransportState;

            try
            {
                // Initialise
                EpConfig epConfig = new EpConfig();
                epConfig.logConfig.consoleLevel = 5;
                epConfig.uaConfig.maxCalls = 5;
                endpoint.libInit(epConfig);

                // Create SIP Transport
                TransportConfig transportConfig = new TransportConfig();
                transportConfig.port = 5060;
                endpoint.transportCreate(pjsip_transport_type_e.PJSIP_TRANSPORT_UDP, transportConfig);

                // Start library
                endpoint.libStart();

                // Create Account
                AccountConfig accountConfig = new AccountConfig();
                accountConfig.idUri = "sip:5505@192.168.11.28";
                accountConfig.regConfig.registrarUri = "sip:192.168.11.28";
                accountConfig.presConfig.publishEnabled = true;
                //accountConfig.sipConfig.
                AuthCredInfo authCredInfo = new AuthCredInfo("digest", "*", "5505", 0, "5505");
                accountConfig.sipConfig.authCreds.Add(authCredInfo);

                // Create Account
                var account = new AccountSC();
                account.OnIncomingCall += Account_OnIncomingCall;
                account.OnIncomingSubscribe += Account_OnIncomingSubscribe;
                account.OnInstantMessage += Account_OnInstantMessage;
                account.OnInstantMessageStatus += Account_OnInstantMessageStatus;
                account.OnMwiInfo += Account_OnMwiInfo;
                account.OnRegStarted += Account_OnRegStarted;
                account.OnRegState += Account_OnRegState;
                account.OnTypingIndication += Account_OnTypingIndication;

                account.create(accountConfig);
                accounts.Add(account);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Finalise()
        {
            try
            {
                foreach (var line in Lines)
                {
                    if (line == null)
                        continue;

                    switch (line.CallState)
                    {
                        case SimpleCallState.Active:
                            // Hangup
                            this.HangupLine(line.LineNumber);
                            break;

                        case SimpleCallState.Available:
                            break;

                        case SimpleCallState.DiallingOut:
                            // Hangup
                            this.HangupLine(line.LineNumber);
                            break;

                        case SimpleCallState.None:
                            break;

                        case SimpleCallState.OnHold:
                            // Hangup
                            this.HangupLine(line.LineNumber);
                            break;

                        case SimpleCallState.Open:
                            if (this.audioMediaPlayer != null)
                                audioMediaPlayer.stopTransmit(endpoint.audDevManager().getPlaybackDevMedia());
                            break;

                        case SimpleCallState.RingingIn:
                            // Ignore
                            break;
                    }
                }

                DisposeOfAudio();


                HandleQueue();

                //// Queue Dialtone
                //softPhoneState.ActionQueue.Enqueue(() =>
                //{
                //    try
                //    {
                //        AudioMediaPlayer audioMediaPlayer = new AudioMediaPlayer();
                //        AudioMedia audioMedia = softPhoneState.endpoint.audDevManager().getPlaybackDevMedia();

                //        audioMediaPlayer.createPlayer("Media/dial_tone.wav");
                //        audioMediaPlayer.startTransmit(audioMedia);
                //    }
                //    catch (Exception ex)
                //    {
                //        System.Diagnostics.Debug.WriteLine(ex.Message);
                //    }
                //});

                while (ActionQueue.Count > 0)
                    Thread.Sleep(50);

                foreach (var account in accounts.ToArray())
                {
                    if (account != null)
                    {
                        account.setRegistration(false);

                        Thread.Sleep(100);

                        HandleQueue();

                        account.OnIncomingCall -= Account_OnIncomingCall;
                        account.OnIncomingSubscribe -= Account_OnIncomingSubscribe;
                        account.OnInstantMessage -= Account_OnInstantMessage;
                        account.OnInstantMessageStatus -= Account_OnInstantMessageStatus;
                        account.OnMwiInfo -= Account_OnMwiInfo;
                        account.OnRegStarted -= Account_OnRegStarted;
                        account.OnRegState -= Account_OnRegState;
                        account.OnTypingIndication -= Account_OnTypingIndication;

                        account.Dispose();
                    }
                }
                accounts.Clear();

                if (endpoint != null)
                {
                    endpoint.OnNatCheckStunServersComplete -= Endpoint_OnNatCheckStunServersComplete;
                    endpoint.OnNatDetectionComplete -= Endpoint_OnNatDetectionComplete;
                    endpoint.OnSelectAccount -= Endpoint_OnSelectAccount;
                    endpoint.OnTimer -= Endpoint_OnTimer;
                    endpoint.OnTransportState -= Endpoint_OnTransportState;

                    endpoint.libDestroy();
                    endpoint.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DisposeOfAudio()
        {
            ActionQueue.Enqueue(() =>
            {
                if (this.audioMediaPlayer != null)
                    this.audioMediaPlayer.Dispose();
            });
        }

        private void HandleQueue()
        {
            if (EventQueue.Count == 0 && ActionQueue.Count == 0)
                return;

            while (EventQueue.Count > 0)
            {
                var queuedEvent = EventQueue.Dequeue();

                var queuedEventSenderName = queuedEvent.Sender.GetType().Name;
                var queuedEventName = queuedEvent.EventArgs.GetType().Name;

                switch (queuedEventSenderName)
                {
                    case "Account":
                        HandleQueueItem(queuedEvent.Sender as Account, queuedEvent.EventArgs);
                        break;
                    case "AccountSC":
                        HandleQueueItem(queuedEvent.Sender as AccountSC, queuedEvent.EventArgs);
                        break;

                    case "Call":
                        HandleQueueItem(queuedEvent.Sender as Call, queuedEvent.EventArgs);
                        break;
                    case "CallSC":
                        HandleQueueItem(queuedEvent.Sender as CallSC, queuedEvent.EventArgs);
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown Sender: " + queuedEventSenderName);
                        break;
                }
            }

            if (ActionQueue.Count == 0)
                return;

            while (ActionQueue.Count > 0)
            {
                var queuedAction = ActionQueue.Dequeue();

                try
                {
                    queuedAction();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void InvokeGUIThread(Action action)
        {
            if (frmReference != null)
                frmReference.InvokeGUIThread(action);
        }

        #region Line Methods

        public uint GetNextAvailableLineNumber()
        {
            if (this.Lines == null || this.Lines.Count() == 0)
                return 0;

            return this.Lines.Skip(1).FirstOrDefault(l => l.CallState == SimpleCallState.Available)?.LineNumber ?? 0;
        }

        public LineSet GetLineByNumber(uint lineNumber)
        {
            if (this.Lines == null || this.Lines.Count() == 0)
                return null;

            return this.Lines.Skip(1).FirstOrDefault(l => l.LineNumber == lineNumber);
        }

        public LineSet GetLineByCallId(int callId)
        {
            if (this.Lines == null || this.Lines.Count() == 0)
                return null;

            return this.Lines.Skip(1).FirstOrDefault(l => l.CallId == callId);
        }

        public LineSet GetActiveLine()
        {
            if (this.Lines == null || this.Lines.Count() == 0)
                return null;

            return this.Lines.Skip(1).FirstOrDefault(l => l.IsActiveLine);
        }

        public bool SetAsActiveLine(uint lineNumber)
        {
            bool? ChangeSucceded = default(bool?);

            var __wantedLine = GetLineByNumber(lineNumber);
            var __activeLine = GetActiveLine();

            if (__wantedLine == null)
                return false;

            if (__wantedLine.IsActiveLine)
                ChangeSucceded = true;
            else
            {
                if (__activeLine != null)
                {
                    // Deal with Active Line
                    switch (__activeLine.CallState)
                    {
                        case SimpleCallState.Active:
                            this.HoldCurrentLine();
                            ChangeSucceded = true;
                            break;
                        case SimpleCallState.Available:
                            ChangeSucceded = true;
                            break;
                        case SimpleCallState.DiallingOut:
                            // WHOA!!
                            ChangeSucceded = false;
                            break;
                        case SimpleCallState.None:
                            // Nothing to do - shouldn't occur
                            break;
                        case SimpleCallState.OnHold:
                            // Nothing to do
                            ChangeSucceded = true;
                            break;
                        case SimpleCallState.Open:
                            // Discard
                            __activeLine.DiscardOpenLine();
                            ChangeSucceded = true;
                            break;
                        case SimpleCallState.RingingIn:
                            // Nothing to do
                            ChangeSucceded = true;
                            break;
                    }
                }

                if (ChangeSucceded != false)
                {
                    __wantedLine.IsActiveLine = true;
                    // Do we need to Open it?

                    switch (__wantedLine.CallState)
                    {
                        case SimpleCallState.Active:
                            ChangeSucceded = true;
                            break;
                        case SimpleCallState.Available:
                            ChangeSucceded = true;
                            break;
                        case SimpleCallState.DiallingOut:
                            // shouldn't get here
                            ChangeSucceded = false;
                            break;
                        case SimpleCallState.None:
                            // Nothing to do - shouldn't occur
                            break;
                        case SimpleCallState.OnHold:
                            ChangeSucceded = true;
                            // Nothing to do
                            break;
                        case SimpleCallState.Open:
                            // Shouldn't happen
                            ChangeSucceded = true;
                            break;
                        case SimpleCallState.RingingIn:
                            // Nothing to do - shouldn't occur - or should it? if we answer a call
                            ChangeSucceded = true;
                            break;
                    }
                }
            }

            return ChangeSucceded ?? false;
        }

        public void NoLinesAvailable()
        {
            // Queue Dialtone
            this.ActionQueue.Enqueue(() =>
            {
                try
                {
                    if (this.audioMediaPlayer == null)
                        this.audioMediaPlayer = new AudioMediaPlayer();

                    AudioMedia audioMedia = this.endpoint.audDevManager().getPlaybackDevMedia();

                    this.audioMediaPlayer.createPlayer("hangup.wav");
                    this.audioMediaPlayer.startTransmit(audioMedia);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }


        #endregion

        #region Call Methods

        public void SetAsOnline(AccountSC account)
        {
            //PresenceStatus presenceStatus = new PresenceStatus();
            //presenceStatus.status = pjsua_buddy_status.PJSUA_BUDDY_STATUS_ONLINE;

            //account.setOnlineStatus(presenceStatus);

            ActionQueue.Enqueue(() =>
            {
                try
                {
                    PresenceStatus presenceStatus = new PresenceStatus();
                    presenceStatus.status = pjsua_buddy_status.PJSUA_BUDDY_STATUS_ONLINE;

                    account.setOnlineStatus(presenceStatus);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void DialOut(AccountSC account, LineSet activeLine, string destinationUri)
        {
            ActionQueue.Enqueue(() =>
            {
                CallSC call = new CallSC(account);
                CallOpParam callOpParam = new CallOpParam(true);

                SetupCall(call);

                try
                {
                    if (this.audioMediaPlayer != null)
                        audioMediaPlayer.stopTransmit(endpoint.audDevManager().getPlaybackDevMedia());

                    call.makeCall(destinationUri, callOpParam);
                    activeLine.SetCall(call);
                    //calls.Add(call.getId(), call);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void AnswerCall(uint lineNumber)
        {
            var __callLine = GetLineByNumber(lineNumber);

            if (__callLine == null)
                return;

            if (!__callLine.IsActiveLine)
            {
                if (!SetAsActiveLine(lineNumber))
                {
                    // Unable to Answer
                    return;
                }
            }

            ActionQueue.Enqueue(() =>
            {
                try
                {
                    CallOpParam callOpParam = new CallOpParam();
                    callOpParam.statusCode = pjsip_status_code.PJSIP_SC_OK;

                    __callLine.Call.answer(callOpParam);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void HoldCurrentLine()
        {
            var __activeLine = GetActiveLine();

            if (__activeLine == null)
                return;

            HoldLine(__activeLine.LineNumber);
        }

        public void HoldLine(uint lineNumber)
        {
            var __callLine = GetLineByNumber(lineNumber);

            if (__callLine == null)
                return;

            ActionQueue.Enqueue(() =>
            {
                try
                {
                    CallOpParam callOpParam = new CallOpParam(true);
                    //callOpParam.options = (uint)pjsua_call_flag.PJSUA_CALL_UPDATE_CONTACT;

                    __callLine.Call.setHold(callOpParam);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void UnHoldCurrentLine()
        {
            var __activeLine = GetActiveLine();

            if (__activeLine == null)
                return;

            UnHoldCall(__activeLine.LineNumber);
        }

        public void UnHoldCall(uint lineNumber)
        {
            var __callLine = GetLineByNumber(lineNumber);

            if (__callLine == null)
                return;

            ActionQueue.Enqueue(() =>
            {
                try
                {
                    CallOpParam callOpParam = new CallOpParam(true);                    
                    //callOpParam.options = (uint)pjsua_call_flag.PJSUA_CALL_UNHOLD;
                    callOpParam.options = 1;

                    callOpParam.opt.audioCount = 1;
                    __callLine.Call.reinvite(callOpParam);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void HangupCurrentLine()
        {
            var __activeLine = GetActiveLine();

            if (__activeLine == null)
                return;

            HangupLine(__activeLine.LineNumber);
        }

        public void HangupLine(uint lineNumber)
        {
            var __callLine = GetLineByNumber(lineNumber);

            if (__callLine == null)
                return;

            ActionQueue.Enqueue(() =>
            {
                try
                {
                    CallOpParam callOpParam = new CallOpParam();

                    __callLine.Call.hangup(callOpParam);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void HangupCall(int callId)
        {
            var __callLine = GetLineByCallId(callId);

            if (__callLine == null)
                return;

            ActionQueue.Enqueue(() =>
            {
                try
                {
                    CallOpParam callOpParam = new CallOpParam();

                    __callLine.Call.hangup(callOpParam);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }


        private void SetupCall(CallSC call)
        {
            call.OnCallMediaEvent += Call_OnCallMediaEvent;
            call.OnCallMediaState += Call_OnCallMediaState;
            call.OnCallMediaTransportState += Call_OnCallMediaTransportState;
            call.OnCallRedirected += Call_OnCallRedirected;
            call.OnCallReplaced += Call_OnCallReplaced;
            call.OnCallReplaceRequest += Call_OnCallReplaceRequest;
            call.OnCallRxOffer += Call_OnCallRxOffer;
            call.OnCallRxReinvite += Call_OnCallRxReinvite;
            call.OnCallSdpCreated += Call_OnCallSdpCreated;
            call.OnCallState += Call_OnCallState;
            call.OnCallTransferRequest += Call_OnCallTransferRequest;
            call.OnCallTransferStatus += Call_OnCallTransferStatus;
            call.OnCallTsxState += Call_OnCallTsxState;
            call.OnCallTxOffer += Call_OnCallTxOffer;
            call.OnCreateMediaTransport += Call_OnCreateMediaTransport;
            call.OnCreateMediaTransportSrtp += Call_OnCreateMediaTransportSrtp;
            call.OnDtmfDigit += Call_OnDtmfDigit;
            call.OnInstantMessage += Call_OnInstantMessage;
            call.OnInstantMessageStatus += Call_OnInstantMessageStatus;
            call.OnStreamCreated += Call_OnStreamCreated;
            call.OnStreamDestroyed += Call_OnStreamDestroyed;
            call.OnTypingIndication += Call_OnTypingIndication;
        }

        private void StripDownCall(int callId)
        {
            var __line = Lines.Skip(1).FirstOrDefault(l => l.CallId == callId);

            if (__line != null)
            {
                StripDownCall(__line.Call);
                __line.ResetLine();
            }

            //if (calls.Count(c => c.Key.ToString() == callId) == 1)
            //    StripDownCall(calls.First(c => c.Key.ToString() == callId).Value);

            //calls.Remove(Convert.ToInt32(callId));// calls.First(c => c.Key.ToString() == callId)
        }

        private void StripDownCall(CallSC call)
        {
            call.OnCallMediaEvent -= Call_OnCallMediaEvent;
            call.OnCallMediaState -= Call_OnCallMediaState;
            call.OnCallMediaTransportState -= Call_OnCallMediaTransportState;
            call.OnCallRedirected -= Call_OnCallRedirected;
            call.OnCallReplaced -= Call_OnCallReplaced;
            call.OnCallReplaceRequest -= Call_OnCallReplaceRequest;
            call.OnCallRxOffer -= Call_OnCallRxOffer;
            call.OnCallRxReinvite -= Call_OnCallRxReinvite;
            call.OnCallSdpCreated -= Call_OnCallSdpCreated;
            call.OnCallState -= Call_OnCallState;
            call.OnCallTransferRequest -= Call_OnCallTransferRequest;
            call.OnCallTransferStatus -= Call_OnCallTransferStatus;
            call.OnCallTsxState -= Call_OnCallTsxState;
            call.OnCallTxOffer -= Call_OnCallTxOffer;
            call.OnCreateMediaTransport -= Call_OnCreateMediaTransport;
            call.OnCreateMediaTransportSrtp -= Call_OnCreateMediaTransportSrtp;
            call.OnDtmfDigit -= Call_OnDtmfDigit;
            call.OnInstantMessage -= Call_OnInstantMessage;
            call.OnInstantMessageStatus -= Call_OnInstantMessageStatus;
            call.OnStreamCreated -= Call_OnStreamCreated;
            call.OnStreamDestroyed -= Call_OnStreamDestroyed;
            call.OnTypingIndication -= Call_OnTypingIndication;
        }

        #endregion

        

        
    }
}
