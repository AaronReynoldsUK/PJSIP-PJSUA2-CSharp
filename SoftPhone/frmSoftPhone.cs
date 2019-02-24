using PJSIP_PJSUA2_CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftPhone
{
    public partial class frmSoftPhone : Form, IGuiForm
    {
        public frmSoftPhone()
        {
            InitializeComponent();

            this.FormClosing += FrmSoftPhone_FormClosing;
        }


        private static SoftPhoneState SoftPhoneState = null;
        private Thread workerThread = null;
        private bool stopBgThread = false;
        BackgroundWorker bgThread;

        private void frmSoftPhone_Load(object sender, EventArgs e)
        {
            SoftPhoneState = new SoftPhoneState();
            SoftPhoneState.SetFormReference(this);

            stopBgThread = false;
            workerThread = new Thread(new ThreadStart(SoftPhoneState.WorkerOperation));
            workerThread.Start();

            bgThread = new BackgroundWorker();
            bgThread.DoWork += BgThread_DoWork;
            //bgThread.ProgressChanged += BgThread_ProgressChanged;
            bgThread.RunWorkerCompleted += BgThread_RunWorkerCompleted;
            //bgThread.WorkerReportsProgress = true;
            bgThread.RunWorkerAsync(this);

            //Initialise();
        }

        #region Form Events

        private void FrmSoftPhone_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopBgThread = true;
            SoftPhoneState?.StopWorkerThread();

            e.Cancel = !CanClose;
        }

        #endregion

        private void BgThread_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!stopBgThread)
            {
                Thread.Sleep(250);

                InvokeGUIThread(() => { UpdateGUI(); });
            }
        }

        private void BgThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BgThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #region Keypad

        private void frmSoftPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter
                var __keyPressEventArgs = new KeyPressEventArgs((char)10);

                Digits_KeyPress(sender, __keyPressEventArgs);
                e.Handled = true;
            }
        }

        private void frmSoftPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                Digits_KeyPress(sender, e);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)8)
            {
                // Backspace
                Digits_KeyPress(sender, e);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27)
            {
                // Escape
                Digits_KeyPress(sender, e);
                e.Handled = true;
            }
        }

        // Add digit pressed to the DialString
        private void Digits_KeyPress(object sender, KeyPressEventArgs e)
        {
            var __activeLine = SoftPhoneState.GetActiveLine();
            var __ignoreUnlessOpen = new char[] { (char)8, (char)10, (char)27 };
            uint __lineNumber = 0;

            if (__activeLine == null)
            {
                __lineNumber = SoftPhoneState.GetNextAvailableLineNumber();

                if (__lineNumber == -0)
                {
                    SoftPhoneState.NoLinesAvailable();
                    return;
                }

                if (!SoftPhoneState.SetAsActiveLine(__lineNumber))
                {
                    // some kind of issue
                    return;
                }

                __activeLine = SoftPhoneState.GetLineByNumber(__lineNumber);
            }

            if (__activeLine == null)
            {
                // shouldn't happen
                return;
            }

            if (__activeLine.CallState == SimpleCallState.Available)
            {
                if (__ignoreUnlessOpen.Contains(e.KeyChar))
                { }
                else
                {
                    __activeLine.OpenLine();

                    // Wait for the line to open
                    while (__activeLine.CallState != SimpleCallState.Open)
                        Thread.Sleep(50);
                }                
            }

            if (__activeLine.CallState == SimpleCallState.Open)
            {
                // if pre-call, add to dial string
                if (e.KeyChar == (char)8)   // del / bs
                {
                    if (__activeLine.DialString.Length > 0)
                        __activeLine.DialString = __activeLine.DialString.Substring(0, __activeLine.DialString.Length - 1);
                }
                else if (e.KeyChar == (char)27)
                {
                    // Close
                    __activeLine.DiscardOpenLine();
                }
                else if (e.KeyChar == (char)10)
                {
                    // Dial
                    if (!String.IsNullOrEmpty(__activeLine.DialString))
                        InitiateCall();
                }
                else
                {
                    __activeLine.DialString += e.KeyChar;
                    __activeLine.PlayDigit(e.KeyChar);
                }
            }
            else if (__activeLine.CallState == SimpleCallState.Active)
            {
                // if on call, send DTMF
                __activeLine.SendDTMF(e.KeyChar.ToString());
            }
            else
            {
                // if inactive, ignore
            }
            e.Handled = true;
        }

        private void Digits_Click(object sender, EventArgs e)
        {
            var __tag = (sender as Button).Tag?.ToString() ?? " ";

            char __key = (__tag == "BS" || __tag == "DEL")
                ? (char)8
                : __tag.First();

            var __keyPressEventArgs = new KeyPressEventArgs(__key);
            
            Digits_KeyPress(sender, __keyPressEventArgs);
        }

        #endregion

        #region Active Call Control

        private void btnCall_Click(object sender, EventArgs e)
        {
            InitiateCall();
        }

        private void InitiateCall()
        {
            var __account = SoftPhoneState.accounts.FirstOrDefault();

            if (__account != null)
            {
                var __activeLine = SoftPhoneState.GetActiveLine();
                //var __activeLine = SoftPhoneState.Lines.FirstOrDefault(c => c.IsActiveLine);

                if (__activeLine == null)
                {
                    // No active line - do nothing
                    return;
                }

                var __sipDialString = String.Format("sip:{0}@192.168.11.28", __activeLine.DialString);

                SoftPhoneState.DialOut(__account, __activeLine, __sipDialString);

                //if (__activeLine != null)
                //    SoftPhoneState.DialOut(__account, String.Format("sip:{0}@192.168.11.28", __activeLine.DialString));
            }
        }

        private void btnHOLD_Click(object sender, EventArgs e)
        {
            var __account = SoftPhoneState.accounts.FirstOrDefault();

            if (__account != null)
            {
                var __activeLine = SoftPhoneState.GetActiveLine();

                if (__activeLine == null)
                {
                    // No active line - do nothing
                    return;
                }

                if (__activeLine.CallState == SimpleCallState.Active)
                {
                    // Place on HOLD
                    SoftPhoneState.HoldCurrentLine();
                    //SoftPhoneState.HoldCall(__activeLine.CallId);
                }
                else
                {
                    // Resume from HOLD
                    SoftPhoneState.UnHoldCurrentLine();
                    //SoftPhoneState.UnHoldCall(__activeLine.CallId);
                }
            }
        }

        private void btnHANGUP_Click(object sender, EventArgs e)
        {
            var __account = SoftPhoneState.accounts.FirstOrDefault();

            if (__account != null)
            {
                var __activeLine = SoftPhoneState.GetActiveLine();
                
                if (__activeLine == null)
                {
                    // No active line - do nothing
                    return;
                }

                if (__activeLine != null)
                {
                    if (__activeLine.CallId != -1)
                        SoftPhoneState.HangupCall(__activeLine.CallId);
                    else
                    {
                        // Discard
                        __activeLine.ResetLine();   // is this the right option here?
                    }
                }
            }
        }

        #endregion

        #region Line Buttons

        private void btnLine1_Click(object sender, EventArgs e)
        {
            LineButton_Click(1);
        }

        private void btnLine2_Click(object sender, EventArgs e)
        {
            LineButton_Click(2);
        }

        private void btnLine3_Click(object sender, EventArgs e)
        {
            LineButton_Click(3);
        }

        private void LineButton_Click(uint lineNumber)
        {
            // Action depends upon state
            var __line = SoftPhoneState.GetLineByNumber(lineNumber);

            if (!__line.IsActiveLine)
            {
                var __activeLine = SoftPhoneState.GetActiveLine();
                if (__activeLine != null)
                {
                    switch (__activeLine.CallState)
                    {
                        case SimpleCallState.Active:
                            SoftPhoneState.HoldCurrentLine();// HoldCall(__activeLine.CallId);
                            break;
                        case SimpleCallState.Available:
                            break;
                        case SimpleCallState.DiallingOut:
                            // WHOA!!
                            return;
                            break;
                        case SimpleCallState.None:
                            // Nothing to do - shouldn't occur
                            break;
                        case SimpleCallState.OnHold:
                            // Nothing to do
                            break;
                        case SimpleCallState.Open:
                            // Discard
                            __activeLine.DiscardOpenLine();
                            break;
                        case SimpleCallState.RingingIn:
                            // Nothing to do - shouldn't occur
                            break;
                    }
                }
                SoftPhoneState.SetAsActiveLine(lineNumber);
            }
            else
            {
                // switch between HOLD and UNHOLD
                if (__line.CallState == SimpleCallState.Active)
                    SoftPhoneState.HoldLine(lineNumber);
                else if (__line.CallState == SimpleCallState.OnHold)
                    SoftPhoneState.UnHoldCall(lineNumber);
            }
        }

        #endregion

        public void InvokeGUIThread(Action action)
        {
            Invoke(action);
        }

        public void UpdateRegState(string state)
        {
            InvokeGUIThread(() => { lblRegState.Text = state; });
        }

        public void UpdatePrescence(string status)
        {
            InvokeGUIThread(() => { lblPresence.Text = status; });
        }

        public bool CanClose { get; set; } = false;

        public void CloseNow()
        {
            InvokeGUIThread(() => { this.Close(); });
        }

        public void UpdateGUI()
        {
            // Update Lines
            if (SoftPhoneState != null)
            {
                if (SoftPhoneState.Lines != null)
                {
                    string __activeTime = "N/A";
                    for (uint __lineNo = 1; __lineNo <= SoftPhoneState.MaxLines; __lineNo++)
                    {
                        var __line = SoftPhoneState.GetLineByNumber(__lineNo);

                        if (__line == null)
                            continue;

                        var __ctrlLineKey = this.Controls.Find(String.Format("btnLine{0}", __lineNo), true).FirstOrDefault();
                        var __ctrlLineTB = this.Controls.Find(String.Format("txtLine{0}", __lineNo), true).FirstOrDefault();

                        if (__ctrlLineKey == null || __ctrlLineTB == null)
                            continue;

                        var __lineKeyBtn = (__ctrlLineKey as Button);
                        var __lineTextbox = (__ctrlLineTB as TextBox);

                        if (__line.IsActiveLine)
                            __lineKeyBtn.FlatAppearance.BorderColor = Color.Black;
                        else
                            __lineKeyBtn.FlatAppearance.BorderColor = Color.Silver;

                        switch (__line.CallState)
                        {
                            case SimpleCallState.Active:
                                __lineKeyBtn.Enabled = true;
                                __lineKeyBtn.BackColor = System.Drawing.Color.Green;
                                __activeTime = TimeSpan.FromSeconds(__line.CallInfo.totalDuration.sec).ToString();

                                if (__line.IsActiveLine)
                                {
                                    btnHOLD.Enabled = true;
                                    btnHOLD.Text = "HOLD";
                                    btnHANGUP.Enabled = true;
                                }
                                break;

                            case SimpleCallState.Available:
                                __lineKeyBtn.Enabled = true;
                                __lineKeyBtn.BackColor = Color.White;
                                __lineTextbox.Text = __line.DialString;

                                if (__line.IsActiveLine)
                                {
                                    btnHOLD.Enabled = false;
                                    btnHOLD.Text = "HOLD";
                                    btnHANGUP.Enabled = false;
                                }
                                break;

                            case SimpleCallState.DiallingOut:
                                break;

                            case SimpleCallState.None:
                                __lineKeyBtn.Enabled = false;
                                __lineKeyBtn.BackColor = Color.White;
                                break;

                            case SimpleCallState.OnHold:
                                __lineKeyBtn.Enabled = true;
                                __lineKeyBtn.BackColor = Color.Yellow;

                                if (__line.IsActiveLine)
                                {
                                    btnHOLD.Enabled = true;
                                    btnHOLD.Text = "RESUME";
                                    btnHANGUP.Enabled = true;
                                }
                                break;

                            case SimpleCallState.Open:
                                __lineTextbox.Text = __line.DialString;

                                if (__line.IsActiveLine)
                                {
                                    btnHOLD.Enabled = false;
                                    btnHOLD.Text = "HOLD";
                                    btnHANGUP.Enabled = true;
                                }
                                break;

                            case SimpleCallState.RingingIn:
                                __lineKeyBtn.Enabled = true;
                                __lineKeyBtn.BackColor = Color.Orange;
                                break;
                        }

                    }

                    lblActiveTime.Text = __activeTime;

                    var __activeLine = SoftPhoneState.GetActiveLine();
                    if (__activeLine == null)
                    {
                        btnCall.Enabled = false;
                        lblActiveLine.Text = "Active Line: NONE";
                    }
                    else
                    {
                        btnCall.Enabled = !String.IsNullOrEmpty(__activeLine.DialString);
                        lblActiveLine.Text = String.Format("Active Line: {0}", __activeLine.LineNumber);
                    }
                }
                else
                {
                    // Disable buttons
                    //btnLine1.Enabled = false;
                    //btnLine2.Enabled = false;
                    //btnLine3.Enabled = false;
                }
                //if (SoftPhoneState.calls != null && SoftPhoneState.callInfo != null)
                //{
                //    foreach (var __callSet in SoftPhoneState.calls.ToArray())
                //    {
                //        var __callId = __callSet.Key;
                //        var __callInfo = SoftPhoneState.callInfo.ContainsKey(__callId) ? SoftPhoneState.callInfo[__callId] : null;



                //    }

                //}

                //btnCall.Focus();
            }
        }

        public void UpdateCallStatus(int callId, pjsip_inv_state callStatus, string callStatusText)
        {
            
        }

        
    }
}
