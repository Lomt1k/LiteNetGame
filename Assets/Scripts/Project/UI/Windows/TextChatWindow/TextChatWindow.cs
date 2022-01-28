using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.Windows.TextChatWindow
{
    public class TextChatWindow : Window
    {
        public const string prefabPath = "Prefabs/UI/Windows/TextChatWindow/Text Chat Window";

        public const int maxMessages = 100;
        
        [SerializeField] private Transform _textChatContent;
        [SerializeField] private TextMessage _messagePrefab;

        private Queue<TextMessage> _messages = new Queue<TextMessage>(maxMessages);

        public void AddMessage(string text)
        {
            var message = _messages.Count < maxMessages 
                ? Instantiate(_messagePrefab, _textChatContent) 
                : _messages.Dequeue();
            
            message.SetText(text);
            message.transform.SetAsLastSibling();
            _messages.Enqueue(message);
        }
        
        
    }
}

