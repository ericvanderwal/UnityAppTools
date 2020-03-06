using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace dgd
{
    public static class TextHelper
    {
        // characters used to split string
        private static char[] delimiterChars = {' ', ',', '.', ':', '\t'};

        // characters that are considered special characters
        private static char[] specialCharacters = {'*', '?', '\'', '/', '&', '#', '$', '@'};

        #region CleanString

        /// <summary>
        /// Remove empty space before, after, and only 1 space between each word, and special characters
        /// </summary>
        /// <returns>Clean string</returns>
        public static string ModifyString(string myString, bool removeSpecialChars, bool removeSpaces)
        {
            string returnString = myString;

            var splitString = SplitString(returnString);

            if (splitString.Length == 1)
            {
                if (removeSpecialChars) returnString = RemoveSpecialCharacters(returnString);
                if (removeSpaces) returnString = returnString.Trim();
            }
            else
            {
                string[] allCleanString = new string[splitString.Length];
                for (var index = 0; index < splitString.Length; index++)
                {
                    var s = splitString[index];
                    string cleanString = s;
                    if (removeSpecialChars) cleanString = RemoveSpecialCharacters(s);
                    if (removeSpaces) cleanString = cleanString.Trim();
                    allCleanString[index] = cleanString;
                }

                returnString = String.Join(" ", allCleanString.Where(s => !String.IsNullOrEmpty(s)));
            }

            return returnString;
        }

        /// <summary>
        /// Split String
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        private static string[] SplitString(string myString)
        {
            string[] splitString = myString.Split(delimiterChars);
            return splitString;
        }

        /// <summary>
        /// Remove starting/ending special characters
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        private static string RemoveSpecialCharacters(string myString)
        {
            string returnString;
            returnString = myString.Trim(specialCharacters);
            return returnString;
        }

        /// <summary>
        /// Clean entire string array.
        /// </summary>
        /// <param name="myStringArray"></param>
        /// <returns>An array of strings</returns>
        public static string[] CleanStringArray(string[] myStringArray, bool removeSpecialChars, bool removeSpaces)
        {
            string[] returnArray = new string[myStringArray.Length];

            for (var index = 0; index < myStringArray.Length; index++)
            {
                var s = myStringArray[index];
                var cleanString = TextHelper.ModifyString(s, removeSpecialChars, removeSpaces);
                returnArray[index] = cleanString;
            }

            return returnArray;
        }

        #endregion

        #region TextCasting

        /// <summary>
        /// Determine the case of the words. Should never return original
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public static TextAutoCompleteEnums.TextCasting GetTextCasting(string myString)
        {
            //          Debug.Log("Text casting get started");

            var mySplitString = SplitString(myString);
            TextAutoCompleteEnums.TextCasting textCasting;

            // if first word is at least two letters, then use it to determine case, otherwise
            if (mySplitString[0].Length >= 2)
            {
//                Debug.Log("First word");
                return CaseFromCharacters(myString[0], myString[1]);
            }
            else
            {
                // check to see if any word has more than 1 character
                string word = " ";
                bool hasWord = false;

                foreach (var s in mySplitString)
                {
                    if (s.Length >= 2)
                    {
                        word = s;
                        hasWord = true;
                        break;
                    }
                }

                // one word has more than one character. Use this to determine case
                if (hasWord)
                {
                    return CaseFromCharacters(word[0], word[1]);
                }

                // no word has more than one character. So lets use the first to determine
                else
                {
                    if (Char.IsUpper(myString[0]))
                    {
                        return TextAutoCompleteEnums.TextCasting.Uppercase;
                    }
                    else
                    {
                        return TextAutoCompleteEnums.TextCasting.Lowercase;
                    }
                }
            }
        }

        /// <summary>
        /// Private helper method for GetTextCasting.
        /// </summary>
        /// <param name="char1"></param>
        /// <param name="char2"></param>
        /// <returns></returns>
        private static TextAutoCompleteEnums.TextCasting CaseFromCharacters(Char char1, Char char2)
        {
            TextAutoCompleteEnums.TextCasting textCasting = TextAutoCompleteEnums.TextCasting.Orginal;

            if (Char.IsUpper(char1) && Char.IsUpper(char2))
            {
                textCasting = TextAutoCompleteEnums.TextCasting.Uppercase;
            }

            if (Char.IsUpper(char1) && !Char.IsUpper(char2))
            {
                textCasting = TextAutoCompleteEnums.TextCasting.Titlecase;
            }

            if (!Char.IsUpper(char1))
            {
                textCasting = TextAutoCompleteEnums.TextCasting.Lowercase;
            }

            return textCasting;
        }

        /// <summary>
        /// Cast string to a new case. Ie upper, lower, title or original
        /// </summary>
        /// <param name="myString"></param>
        /// <param name="myCast"></param>
        /// <returns></returns>
        public static string SetTextCasting(string myString, TextAutoCompleteEnums.TextCasting myCast)
        {
            string resultString = "";
            switch (myCast)
            {
                case TextAutoCompleteEnums.TextCasting.Lowercase:
                    resultString = CultureInfo.CurrentCulture.TextInfo.ToLower(myString);
                    break;
                case TextAutoCompleteEnums.TextCasting.Titlecase:
                    resultString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(myString);
                    break;
                case TextAutoCompleteEnums.TextCasting.Uppercase:
                    resultString = CultureInfo.CurrentCulture.TextInfo.ToUpper(myString);
                    break;
                case TextAutoCompleteEnums.TextCasting.Orginal:
                    resultString = myString;
                    break;
            }

            return resultString;
        }

        /// <summary>
        /// Cast string array to a new case. Ie upper, lower, title or orginal
        /// </summary>
        /// <param name="myStringArray"></param>
        /// <param name="myCast"></param>
        /// <returns></returns>
        public static string[] SetTextCastingArray(string[] myStringArray, TextAutoCompleteEnums.TextCasting myCast)
        {
            string[] resultStringArray = new string[myStringArray.Length];

            for (var index = 0; index < myStringArray.Length; index++)
            {
                var s = myStringArray[index];
                var newString = SetTextCasting(s, myCast);
                resultStringArray[index] = newString;
            }

            return resultStringArray;
        }

        #endregion
    }
}