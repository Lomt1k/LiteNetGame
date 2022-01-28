using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Project.UI.Windows.TextChatWindow
{
    public class TextChatWindow : Window
    {
        public const string prefabPath = "Prefabs/UI/Windows/TextChatWindow/Text Chat Window";
        public const int maxMessages = 100;

        public static TextChatWindow instance;
        
        [SerializeField] private Transform _textChatContent;
        [SerializeField] private TextMessage _messagePrefab;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private CanvasGroup _inputFieldGroup;

        private Queue<TextMessage> _messages = new Queue<TextMessage>(maxMessages);
        private bool _isInputFieldActive;

        public override void OnCreated()
        {
            base.OnCreated();
            instance = this;
            ToggleInputField(false);
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _inputField.onDeselect.AddListener(OnInputEnd);
            _inputField.onEndEdit.AddListener(OnInputEnd);
        }
        
        private void UnsubscribeEvents()
        {
            _inputField.onDeselect.RemoveListener(OnInputEnd);
            _inputField.onEndEdit.RemoveListener(OnInputEnd);
        }

        public void AddMessage(string text)
        {
            var message = _messages.Count < maxMessages 
                ? Instantiate(_messagePrefab, _textChatContent) 
                : _messages.Dequeue();
            
            message.SetText(text);
            message.transform.SetAsLastSibling();
            _messages.Enqueue(message);
            
            Debug.Log(text);
        }

        private void ToggleInputField(bool state)
        {
            _inputFieldGroup.alpha = state ? 1f : 0f;
            _inputFieldGroup.interactable = state;
            _inputFieldGroup.blocksRaycasts = state;
            _isInputFieldActive = state;

            if (!state)
            {
                _inputField.text = string.Empty;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F6) || Input.GetKeyDown(KeyCode.T))
            {
                if (!_isInputFieldActive)
                {
                    ToggleInputField(true);
                    _inputField.ActivateInputField();
                }
                
            }
        }

        private void OnInputEnd(string inputText)
        {
            if (!_isInputFieldActive)
                return;

            bool enterPressed = Input.GetKeyDown(KeyCode.Return);
            if (enterPressed && inputText.Length > 1)
            {
                //TODO send message
            }
            
            ToggleInputField(false);
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
    }
}

