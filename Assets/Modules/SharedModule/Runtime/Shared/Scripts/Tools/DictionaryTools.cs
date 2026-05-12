using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class DictionaryTools
    {
        public static string AsString(this IDictionary dict, int indentSize = 2)
        {
            if (dict == null) return "null";

            var sb = new StringBuilder();
            var indent = new string(' ', indentSize);

            sb.AppendLine("{");
            var index = 0;
            var total = dict.Count;
            foreach (DictionaryEntry entry in dict)
            {
                index++;
                sb.Append(indent);
                sb.Append(FormatKey(entry.Key));
                sb.Append(": ");
                sb.Append(FormatValue(entry.Value));
                if (index < total) sb.Append(',');
                sb.AppendLine();
            }

            sb.Append("}");
            return sb.ToString();

            string FormatKey(object key)
            {
                if (key == null) return "null";
                if (key is string s) return "\"" + EscapeString(s) + "\"";
                return key.ToString();
            }

            string FormatValue(object value)
            {
                if (value == null) return "null";
                if (value is string s) return "\"" + EscapeString(s) + "\"";
                if (value is IFormattable formattable) return formattable.ToString(null, CultureInfo.InvariantCulture);
                return value.ToString();
            }

            string EscapeString(string s)
            {
                if (s == null) return null;
                // Базовое экранирование, достаточно для читаемости
                return s
                    .Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\r", "\\r")
                    .Replace("\n", "\\n")
                    .Replace("\t", "\\t");
            }
        }
    }
}