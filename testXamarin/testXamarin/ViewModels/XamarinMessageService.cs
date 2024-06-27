using System;


namespace Kts.Messaging
{
    public class XamarinMessageService : IMessageService
    {
        public void ShowMessage(string content, string caption, MessageType messageType)
        {
            return;
        }

        public bool? ShowYesNoCancelMessage(string content, string caption)
        {
            return true;
        }

        public bool ShowYesNoMessage(string content, string caption)
        {
            return true;
        }
    }
}
