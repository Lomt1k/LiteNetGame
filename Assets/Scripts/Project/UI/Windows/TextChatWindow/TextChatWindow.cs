using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Project.UI.Windows.TextChatWindow
{
    public class TextChatWindow : Window
    {
        public const string prefabPath = "Prefabs/UI/Windows/TextChatWindow/Text Chat Window";
        public const int maxMessages = 100;

        public static TextChatWindow instance;
        
        [SerializeField] private Transform _textChatContent;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private TextMessage _messagePrefab;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private CanvasGroup _inputFieldGroup;
        [SerializeField] private CanvasGroup _scrollGroup;

        private Queue<TextMessage> _messages = new Queue<TextMessage>(maxMessages);
        private Coroutine _scrollCoroutine;
        private bool _isInputFieldActive;

        public override void OnCreated()
        {
            base.OnCreated();
            instance = this;
            ToggleInputField(false);
            ToggleScrollVisibility(false);
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

            ScrollToLastMessage();
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
            if (enterPressed && !string.IsNullOrWhiteSpace(inputText))
            {
                Networking.Client.Sending.ClientSending_TextChat.SendTextChatMessage(inputText);
            }
            
            ToggleInputField(false);
        }

        private void ScrollToLastMessage()
        {
            if (_scrollCoroutine != null)
            {
                StopCoroutine(_scrollCoroutine);
            }
            _scrollCoroutine = StartCoroutine(ScrollToLastMessageCoroutine());
        }

        private IEnumerator ScrollToLastMessageCoroutine()
        {
            for (int i = 0; i < 2; i++) //wait 2 frames after adds last message to correct scroll
            {
                yield return new WaitForEndOfFrame();
            }
            _scrollCoroutine = null;
            _scrollbar.value = 0;
        }

        private void ToggleScrollVisibility(bool state)
        {
            _scrollGroup.alpha = state ? 1f : 0f;
            _scrollGroup.interactable = state;
            _scrollGroup.blocksRaycasts = state;
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
    }
}

