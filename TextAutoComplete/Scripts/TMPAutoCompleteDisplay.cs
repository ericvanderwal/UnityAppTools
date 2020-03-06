using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace dgd
{
    [RequireComponent(typeof(TMPInputAutoComplete))]
    public class TMPAutoCompleteDisplay : MonoBehaviour
    {
        [Header("Pooling")]
        public GameObject prefab;

        public RectTransform autocompleteDropdownPanel;
        public int poolAmount;
        public float buttonHeight = 35;
        private Queue<GameObject> avaliableObjects = new Queue<GameObject>();
        private TMPInputAutoComplete _tmpInputAutoComplete;
        public List<GameObject> activeButtons;

        private void Awake()
        {
            GrowPool(poolAmount);
        }

        private void OnEnable()
        {
            _tmpInputAutoComplete = GetComponent<TMPInputAutoComplete>();
            if (_tmpInputAutoComplete == null)
            {
                Debug.LogError("No TMP Input Autocomplete component is missing on " + gameObject);
            }
            else
            {
                _tmpInputAutoComplete.eventNoInput += NoInput;
                _tmpInputAutoComplete.eventNewInput += NewInput;
                _tmpInputAutoComplete.eventAutoComplete += AutoComplete;
            }
        }

        private void OnDisable()
        {
            if (_tmpInputAutoComplete != null)
            {
                _tmpInputAutoComplete.eventNoInput -= NoInput;
                _tmpInputAutoComplete.eventNewInput -= NewInput;
                _tmpInputAutoComplete.eventAutoComplete -= AutoComplete;
            }
        }

        private void AutoComplete(string[] autocompletelist)
        {
            // reset first
            NoInput();

            // for auto complete create a button
            foreach (var word in autocompletelist)
            {
                var obj = GetFromPool();
                if (obj != null)
                {
                    activeButtons.Add(obj);
                    AutoCompleteClick autoCompleteClick = obj.GetComponent<AutoCompleteClick>();
                    if (autoCompleteClick != null) autoCompleteClick.SetText(word);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(autocompleteDropdownPanel);
        }

        private void NewInput(string wordinput)
        {
        }

        /// <summary>
        /// return buttons to the pool when no input is being used
        /// </summary>
        private void NoInput()
        {
            if (activeButtons.Count > 0)
            {
                foreach (var b in activeButtons)
                {
                    AddToPool(b);
                }
            }
        }

        #region Pooling  

        private void GrowPool(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var instanceAdd = Instantiate(prefab);
                instanceAdd.transform.SetParent(autocompleteDropdownPanel);
                RectTransform rectTransform = instanceAdd.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, buttonHeight);
                }

                AddToPool(instanceAdd);
            }
        }

        public void AddToPool(GameObject instance)
        {
            instance.SetActive(false);
            avaliableObjects.Enqueue(instance);
        }

        public GameObject GetFromPool()
        {
            if (avaliableObjects.Count == 0)
            {
                GrowPool(1);
            }

            var instance = avaliableObjects.Dequeue();
            instance.SetActive(true);
            return instance;
        }

        #endregion
    }
}