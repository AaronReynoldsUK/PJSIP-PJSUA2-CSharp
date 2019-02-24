using PJSIP_PJSUA2_CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone
{
    public class LineSet
    {
        public LineSet(SoftPhoneState softPhoneState, uint lineNumber )
        {
            softPhoneStateReference = softPhoneState;
            LineNumber = lineNumber;
        }

        public uint LineNumber { get; private set; } = 0;
        public int CallId { get; set; } = -1;
        public CallSC Call { get; private set; }
        public CallInfo CallInfo { get; set; }

        public bool IsActiveLine { get; set; } = false;

        public SimpleCallState CallState { get; set; } = SimpleCallState.None;

        public string DialString = String.Empty;

        private SoftPhoneState softPhoneStateReference = null;
        //public AudioMediaPlayer MediaPlayerAudio = null;

        public void ResetLine()
        {
            CallId = -1;
            Call = null;
            CallInfo = null;
            IsActiveLine = false;
            CallState = SimpleCallState.Available;
            DialString = String.Empty;
        }

        public void SetCall(CallSC call)
        {
            this.Call = call;

            this.CallId = this.Call.getId();
            this.CallInfo = this.Call.getInfo();
        }

        public void OpenLine()
        {
            this.IsActiveLine = true;
            this.CallState = SimpleCallState.Open;
            this.DialString = String.Empty;

            // Queue Dialtone
            softPhoneStateReference.ActionQueue.Enqueue(() =>
            {
                try
                {
                    if (softPhoneStateReference.audioMediaPlayer == null)
                        softPhoneStateReference.audioMediaPlayer = new AudioMediaPlayer();

                    AudioMedia audioMedia = softPhoneStateReference.endpoint.audDevManager().getPlaybackDevMedia();

                    softPhoneStateReference.audioMediaPlayer.createPlayer("Media/dial_tone.wav");
                    softPhoneStateReference.audioMediaPlayer.startTransmit(audioMedia);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void DiscardOpenLine()
        {
            softPhoneStateReference.ActionQueue.Enqueue(() =>
            {
                try
                {
                    if (softPhoneStateReference.audioMediaPlayer != null)
                        softPhoneStateReference.audioMediaPlayer.stopTransmit(softPhoneStateReference.endpoint.audDevManager().getPlaybackDevMedia());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
            ResetLine();
        }

        public void SendDTMF(string digits)
        {
            softPhoneStateReference.ActionQueue.Enqueue(() =>
            {
                try
                {
                    this.Call.dialDtmf(digits);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void PlayDigit(char dtmfChar)
        {
            // Queue Dialtone
            softPhoneStateReference.ActionQueue.Enqueue(() =>
            {
                try
                {
                    if (softPhoneStateReference.audioMediaPlayer == null)
                        softPhoneStateReference.audioMediaPlayer = new AudioMediaPlayer();

                    if (softPhoneStateReference.audioMediaPlayer != null)
                        softPhoneStateReference.audioMediaPlayer.stopTransmit(softPhoneStateReference.endpoint.audDevManager().getPlaybackDevMedia());

                    string __fileName = "";
                    if (char.IsDigit(dtmfChar))
                        __fileName = String.Format("Media/{0}.wav", dtmfChar.ToString());
                    else if (dtmfChar == '*')
                        __fileName = "Media/asterisk.wav";
                    else if (dtmfChar == '#')
                        __fileName = "Media/hash.wav";
                    else
                        return;

                    AudioMedia audioMedia = softPhoneStateReference.endpoint.audDevManager().getPlaybackDevMedia();

                    softPhoneStateReference.audioMediaPlayer.createPlayer(__fileName);
                    //softPhoneState.audioMediaPlayer.createPlayer(__fileName, (uint)pjmedia_file_player_option.PJMEDIA_FILE_NO_LOOP);
                    softPhoneStateReference.audioMediaPlayer.startTransmit(audioMedia);
                    //softPhoneState.audioMediaPlayer.stop
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

    }
}
