using System.Collections.Generic;
using UnityEngine;

namespace dgd
{
    [RequireComponent(typeof(TMPInputAutoComplete))]
    public class SimpleAutoComplete : MonoBehaviour
    {
        public enum MatchType
        {
            Start,
            Any
        }

        [Header("Settings")]
        public MatchType matchType = MatchType.Any;

        public int maxMatches = 5;

        [Header("Debug")]
        public bool enableDebug;

        public List<string> autoCompleteTerms;
        private TMPInputAutoComplete _tmpInputAutoComplete;
        public string[] wordList;
        public string inputString;

        private void OnEnable()
        {
            if (_tmpInputAutoComplete == null)
            {
                _tmpInputAutoComplete = GetComponent<TMPInputAutoComplete>();
            }

            _tmpInputAutoComplete.eventLoaded += TermListLoaded;
            _tmpInputAutoComplete.eventNewInput += NewInput;
            _tmpInputAutoComplete.eventNoInput += NoInput;
        }

        private void NoInput()
        {
            inputString = "";
        }

        private void NewInput(string wordinput)
        {
            inputString = wordinput;
            AutoComplete();
        }

        private void TermListLoaded(string[] wordlist)
        {
            wordList = wordlist;
        }

        private void OnDisable()
        {
            _tmpInputAutoComplete.eventLoaded -= TermListLoaded;
            _tmpInputAutoComplete.eventNewInput -= NewInput;
            _tmpInputAutoComplete.eventNoInput -= NoInput;
        }

        /// <summary>
        /// Find auto complete words.
        /// </summary>
        private void AutoComplete()
        {
            autoCompleteTerms = SearchTerms(wordList, inputString, maxMatches, matchType);
            _tmpInputAutoComplete.CallAutoComplete(autoCompleteTerms);
        }

        private List<string> SearchTerms(string[] wordList, string searchTerm, int matchAmount, MatchType myMatchType)
        {
            Debug.Log("Search term is " + searchTerm);

            List<string> matchTerms = new List<string>();
            int checkNum = 0;
            foreach (var word in wordList)
            {
                bool matchWord = false;

                if (myMatchType == MatchType.Start)
                {
                    matchWord = word.StartsWith(searchTerm);
                }
                else
                {
                    matchWord = word.Contains(searchTerm);
                }

                if (matchWord)
                {
                    checkNum++;
                    matchTerms.Add(word);
                }

                if (checkNum == matchAmount) break;
            }

            return matchTerms;
        }
    }
}