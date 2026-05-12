using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class StringTools
    {
        public static string GetColored(this string inputString, Color color) =>
            $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{inputString}</color>";

        public static bool IsNullOrEmptyOrWhiteSpace(this string inputString) =>
            string.IsNullOrWhiteSpace(inputString) || string.IsNullOrEmpty(inputString);

        public static string JoinString<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        public static string JoinString<T>(this IEnumerable<T> enumerable)
        {
            return $"[{string.Join(", ", enumerable)}]";
        }

        /// <summary>
        /// Преобразует строку в lowerCamelCase.
        /// Примеры:
        /// "User" -> "user"
        /// "XMLHttpRequest" -> "xmlHttpRequest"
        /// "hello_world" -> "helloWorld"
        /// "some-multi word" -> "someMultiWord"
        /// </summary>
        public static string ToLowerCamelCase(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;
            var s = inputString.Trim();
            if (s.Length == 0) return s;

            var separators = new[] { ' ', '\t', '-', '_' };
            var parts = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            string combined;
            if (parts.Length > 1)
            {
                var sb = new StringBuilder();
                foreach (var p in parts)
                {
                    if (p.Length == 0) continue;
                    if (p.Length == 1)
                    {
                        sb.Append(char.ToUpperInvariant(p[0]));
                    }
                    else
                    {
                        sb.Append(char.ToUpperInvariant(p[0]));
                        sb.Append(p.Substring(1).ToLowerInvariant());
                    }
                }

                combined = sb.ToString();
            }
            else
            {
                combined = s;
            }

            if (combined.Length == 1)
                return combined.ToLowerInvariant();

            var upperRun = 0;
            while (upperRun < combined.Length && char.IsUpper(combined[upperRun]))
                upperRun++;

            switch (upperRun)
            {
                case 0:
                    return combined;
                case 1:
                    return char.ToLowerInvariant(combined[0]) + combined.Substring(1);
                default:
                {
                    var prefix = combined.Substring(0, upperRun).ToLowerInvariant();
                    return prefix + combined.Substring(upperRun);
                }
            }
        }
    }
}