using System.Collections.Generic;

namespace webapp
{
    public class Session
    {
        public string Id { get; set; }
        private readonly Dictionary<string, List<string>> _flashMessageLists = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, object> _flash = new Dictionary<string, object>();

        
        public Dictionary<string, List<string>> FlashMessageLists
        {
            get
            {
                Dictionary<string, List<string>> flashMessagesClone = new Dictionary<string, List<string>>(_flashMessageLists);
                _flashMessageLists.Clear();
                return flashMessagesClone;
            }
        }
        
        public void AddFlashMessageToList(string listKey, string message)
        {
            _flashMessageLists.TryGetValue(listKey, out List<string> messagesForKey);
            if (messagesForKey == null)
            {
                messagesForKey = new List<string>();
                _flashMessageLists[listKey] = messagesForKey;
            }
            messagesForKey.Add(message);
        }
        
        public void AddFlashMessagesToList(string listKey, List<string> messages)
        {
            _flashMessageLists.TryGetValue(listKey, out List<string> messagesForKey);
            if (messagesForKey == null)
            {
                messagesForKey = messages;
                _flashMessageLists[listKey] = messagesForKey;
            }
            else
            {
                messagesForKey.AddRange(messages);
            }
        }       
        
        public Dictionary<string, object> Flash
        {
            get
            {
                Dictionary<string, object> flashMessagesClone = new Dictionary<string, object>(_flash);
                _flash.Clear();
                return flashMessagesClone;
            }
        }
                       
        public void AddFlash(string key, object flash)
        {
            _flash[key] = flash;
        }
    }
}