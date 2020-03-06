using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace dgd
{
    [RequireComponent(typeof(Button))]
    public class AutoCompleteClick : MonoBehaviour
    {
        private TMPInputAutoComplete _tmpInputAutoComplete;
        private Button _button;
        private TextMeshProUGUI _buttonText;
        private string _text;

        void Start()
        {
            _tmpInputAutoComplete = GetComponentInParent<TMPInputAutoComplete>();
            if (_tmpInputAutoComplete == null)
            {
                Debug.LogError("No Auto Complete component was found in the part of the auto complete click button");
            }

            _button = GetComponent<Button>();
            if (_button != null) _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            if (_button != null) _button.onClick.RemoveListener(OnClick);
        }

        /// <summary>
        /// Set the button text.
        /// </summary>
        public void SetText(string text)
        {
            if (_button == null)
            {
                _buttonText = GetComponentInChildren<TextMeshProUGUI>();
                if (_buttonText == null)
                {
                    Debug.LogError("Textmesh Pro uGui Text component is missing in the child of this button: " + gameObject);
                }
            }

            _buttonText.text = text;
        }

        /// <summary>
        /// This button has been clicked. Send an event to the TMP Autocomplete controller with the button text;
        /// </summary>
        private void OnClick()
        {
            _text = _buttonText.text;
            if (_tmpInputAutoComplete != null) _tmpInputAutoComplete.TermSelected(_text);
        }
    }
}