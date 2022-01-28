using TMPro;
using UnityEngine;

namespace Project.UI.Windows.TextChatWindow
{
    public class TextMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        public void SetText(string text)
        {
            _text.text = text;
        }
        
        
    }
}

