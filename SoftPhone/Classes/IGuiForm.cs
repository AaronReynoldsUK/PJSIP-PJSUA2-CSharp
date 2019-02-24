using System;

namespace SoftPhone
{
    public interface IGuiForm
    {
        void InvokeGUIThread(Action action);

        void UpdateRegState(string state);
        void UpdatePrescence(string status);
        void UpdateCallStatus(int callId, pjsip_inv_state callStatus, string callStatusText);

        void CloseNow();
        bool CanClose { get; set; }
    }
}
