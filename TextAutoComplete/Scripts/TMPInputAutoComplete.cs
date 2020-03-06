using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using Debug = UnityEngine.Debug;

namespace dgd
{
    public class TMPInputAutoComplete : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Select the appropriate text mesh pro ugui input field. This is required")]
        public TMP_InputField tmpInputField;

        [Tooltip("Manually add a word list here. You may also leave this blank and load from text file. If both text file and this word list is used, they will be combined on load")]
        public string[] manualWordArray;

        [Tooltip("Load a word list from a plain text file on load. Leave this field blank to not load a word list from text file.")]
        public string loadWordListPath;

        [Header("Text Input")]
        [Tooltip("Adjust input case from input field as it is typed. This is invisible to the user")]
        public TextAutoCompleteEnums.TextCasting castInputText = TextAutoCompleteEnums.TextCasting.Lowercase;

        [Tooltip("Remove spaces before and after each word. Also removes more than one space between multiple words")]
        public bool cleanInputText = true;

        [Tooltip("Remove special characters from input words.")]
        public bool removeSpecialCharacters = true;

        [Header("Word List")]

        // normally set to lower
        [Tooltip("Adjust input case for word list on load. This only occurs once.")]
        public TextAutoCompleteEnums.TextCasting castWordList = TextAutoCompleteEnums.TextCasting.Lowercase;

        [Tooltip("Remove spaces before and after each word. Also removes more than one space between multiple words. This only occurs once on load.")]
        public bool cleanWordList = true;

        [Tooltip("Remove special characters from word list on load.")]
        public bool removeSpecialCharactersWordList = true;

        [Header("Output")]
        [Tooltip("Adjust output case for word list. This is visible to the user, unlike input and word list case casting.")]
        public TextAutoCompleteEnums.TextCasting castOutputText = TextAutoCompleteEnums.TextCasting.Orginal;

        [Tooltip("Remove spaces before and after each word. Also removes more than one space between multiple words.")]
        public bool cleanOutputText = false;

        [Tooltip("Remove special characters from word list on load.")]
        public bool removeSpecialCharactersOutput = false;

        [Header("Debug")]
        public bool enableDebug;

        public string inputText;

        [Header("Temp")]
        private List<string> loadTermList;

        private string[] loadWordListArray;

        private string[] wordList;

        public string[] outArray;

        // events

        // no input in the input field
        public delegate void EventNoInput();

        public event EventNoInput eventNoInput;

        // event when word list first loads
        public delegate void EventLoaded(string[] wordList);

        public event EventLoaded eventLoaded;

        // new input has been entered into the field
        public delegate void EventNewInput(string wordInput);

        public event EventNewInput eventNewInput;

        // begin auto complete dropdown, passes auto complete word suggestion list
        public delegate void EventAutoComplete(String[] autoCompleteList);

        public event EventAutoComplete eventAutoComplete;

        /// <summary>
        /// Subscribe to input field changes
        /// </summary>
        private void OnEnable()
        {
            if (tmpInputField != null)
            {
                tmpInputField.onValueChanged.AddListener(ValueChanged);
                tmpInputField.onSubmit.AddListener(OnSubmit);
            }
        }

        /// <summary>
        /// Unsubscribe to input field changes to prevent memory leaks.
        /// </summary>
        private void OnDisable()
        {
            if (tmpInputField != null)
            {
                tmpInputField.onValueChanged.RemoveListener(ValueChanged);
                tmpInputField.onSubmit.RemoveListener(OnSubmit);
            }
        }

        /// <summary>
        /// Unity start method.
        /// </summary>
        private void Start()
        {
            LoadWordSources();
            CombineWordLists();
            ManageWordList();
        }

        /// <summary>
        /// Load word sources (lists)
        /// </summary>
        private void LoadWordSources()
        {
            // Load manual word list
            loadWordListArray = new string[0];
            if (manualWordArray == null)
            {
                manualWordArray = new string[0];
            }

            // Load word list from file
            if (loadWordListPath != "")
            {
                // Load the word list if it exists.
                if (loadWordListPath != null)
                {
                    loadTermList = new List<string>();

                    if (File.Exists(loadWordListPath))
                    {
                        var stream = new StreamReader(loadWordListPath);
                        while (!stream.EndOfStream)
                            loadTermList.Add(stream.ReadLine());
                        if (enableDebug) Debug.Log("Characters added from file");

                        if (loadTermList.Count > 0)
                        {
                            loadWordListArray = loadTermList.ToArray();
                        }
                        else
                        {
                            if (enableDebug) Debug.Log("No characters were loaded from file. File may be empty or not txt extension");
                        }
                    }
                    else
                    {
                        if (enableDebug) Debug.Log("File does not exist for: " + loadWordListPath);
                    }
                }
            }
        }

        /// <summary>
        /// Combine loaded and manually entered word lists as necessary
        /// </summary>
        private void CombineWordLists()
        {
            if (loadWordListArray.Length > 0 && manualWordArray.Length > 0)
            {
                wordList = loadWordListArray.Concat(manualWordArray).ToArray();
                if (enableDebug) Debug.Log("Loaded word list from file and manual word list have been combined");
            }
            else if (loadWordListArray.Length > 0 && manualWordArray.Length <= 0)
            {
                wordList = loadWordListArray;
                if (enableDebug) Debug.Log("Loaded word list from file. No manual word list found");
            }
            else if (loadWordListArray.Length <= 0 && manualWordArray.Length > 0)
            {
                wordList = manualWordArray;
                if (enableDebug) Debug.Log("No Loaded word list from file found. Manual word list has been loaded");
            }
            else
            {
                wordList = new[] {"Error"};
                Debug.LogError("No manual or loaded word list were for found TMP Auto Completed component on " + gameObject);
            }
        }

        /// <summary>
        /// Call to re-initialize word list, if the word list has been updated
        /// </summary>
        public void ManageWordList()
        {
            if (cleanWordList || removeSpecialCharactersWordList)
            {
                wordList = TextHelper.CleanStringArray(wordList, removeSpecialCharactersWordList, cleanWordList);
                if (enableDebug) Debug.Log("Word list has special characters removed: " + removeSpecialCharactersWordList);
                if (enableDebug) Debug.Log("Word list has extra spaces removed: " + cleanWordList);
            }

            if (castWordList != TextAutoCompleteEnums.TextCasting.Orginal)
            {
                wordList = TextHelper.SetTextCastingArray(wordList, castWordList);
                if (enableDebug) Debug.Log("Word list case has been cast to : " + castWordList.ToString());
            }

            if (eventLoaded != null) eventLoaded(wordList);
        }

        /// <summary>
        /// Text input string has been submitted (usually with enter key, etc)
        /// </summary>
        /// <param name="arg0"></param>
        private void OnSubmit(string arg0)
        {
            CheckValue(true, arg0);
        }

        /// <summary>
        /// Text input value has been changed (usually by typing less or more characters).
        /// </summary>
        /// <param name="arg0"></param>
        private void ValueChanged(string arg0)
        {
            CheckValue(false, arg0);
        }

        /// <summary>
        /// Change changes
        /// </summary>
        /// <param name="isSubmission"></param>
        private void CheckValue(bool isSubmission, string textString)
        {
            if (wordList.Length <= 0) return;

            // text input is empty.
            if (textString.Length <= 0)
            {
                if (eventNoInput != null) eventNoInput();
                return;
            }

            inputText = textString;
            if (enableDebug) Debug.Log("Raw Input field text : " + textString);

            // clean input text
            if (cleanInputText || removeSpecialCharacters)
            {
                inputText = TextHelper.ModifyString(textString, removeSpecialCharacters, cleanInputText);
                if (enableDebug) Debug.Log("Sanitized Input field text: " + inputText);
            }

            // cast text input
            if (castInputText != TextAutoCompleteEnums.TextCasting.Orginal)
            {
                inputText = TextHelper.SetTextCasting(inputText, castInputText);
                if (enableDebug) Debug.Log("Case adjusted Input field text: " + inputText);
            }

            if (eventNewInput != null) eventNewInput(inputText);
        }

        /// <summary>
        /// Method called to start auto complete display
        /// </summary>
        public void CallAutoComplete(List<string> autoCompleteList)
        {
            outArray = autoCompleteList.ToArray();

            if (cleanOutputText || removeSpecialCharactersOutput)
            {
                outArray = TextHelper.CleanStringArray(outArray, removeSpecialCharactersOutput, cleanOutputText);
                if (enableDebug) Debug.Log("Out list has special characters removed: " + removeSpecialCharactersOutput);
                if (enableDebug) Debug.Log("Out list has extra spaces removed: " + cleanOutputText);
            }

            if (castOutputText != TextAutoCompleteEnums.TextCasting.Orginal)
            {
                outArray = TextHelper.SetTextCastingArray(outArray, castOutputText);
                if (enableDebug) Debug.Log("Word list case has been cast to : " + castOutputText.ToString());
            }

            if (eventAutoComplete != null) eventAutoComplete(outArray);
        }

        public void TermSelected(string selectedTerm)
        {
        }
    }
}