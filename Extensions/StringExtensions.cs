using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace dgd
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove all spaces from a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveSpaces(this string value)
        {
            return value.Replace(" ", "");
        }

        /// <summary>
        /// Remove Special Characters
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Change string to title case. If Force option is set, change all letters to lowercase first, then title case. If set to false, any existing capital letters will not be changed to lower case.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value, bool force)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;

            if (force)
            {
                string s = value.ToLower();
                return cultInfo.ToTitleCase(s);
            }
            else
            {
                return cultInfo.ToTitleCase(value);
            }
        }

        /// <summary>
        /// Capitalize the first word of a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Capitalize(this string value)
        {
            return value.First().ToString().ToUpper() + value.Substring(1);
        }

        /// <summary>
        /// Create camel case from string with spaces. The first letter will be capitalized.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SpaceToUpperCamel(this string value)
        {
            value.ToUpper();
            return value.Replace(" ", "");
        }

        /// <summary>
        /// Create camel case from string with spaces. The first letter will be not be capitalized.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SpaceToLowerCamel(this string value)
        {
            value.ToUpper();
            value.Replace(" ", "");
            return value.First().ToString().ToLower() + value.Substring(1);
        }

        /// <summary>
        /// Create spaces between camel case (capital) characters.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CamelToSpace(this string value)
        {
            var newValue = value.First().ToString().ToLower() + value.Substring(1);

            for (int i = 0; i < newValue.Length; i++)
            {
                if (Char.IsUpper(newValue[i]))
                {
                    newValue.Replace(newValue[i].ToString(), " ");
                }
            }

            return newValue;
        }

        /// <summary>
        /// Create snake case from string with spaces. Each space will be replaced by an underscore and all letters to lowercase.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SpaceToSnakeCase(this string value)
        {
            value.ToLower();
            return value.Replace(" ", "_");
        }

        /// <summary>
        /// Create spaces between snake case (underscores).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SnakeToSpace(this string value)
        {
            return value.Replace("_", " ");
        }
    }
}