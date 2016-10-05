using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Maybe2
{
    public static class StringExtensions
    {
        public const string NewLine = "\r\n";

        public static string Pack(this string s, int? maxlen = null)
        {
            if (s != null && maxlen.HasValue && maxlen > 0 && s.Length > maxlen.Value)
            {
                int l = s.Length - maxlen.Value;
                s = s.Remove(maxlen.Value, l);
            }
            return (s == null) ? null : s.Trim();
        }

        public static string PackWhiteSpaces(this string s, string packSpaces = " ")
        {
            if (s == null) return null;
            return Regex.Replace(s.Trim(), @"\s+", packSpaces);
        }

        public static string PackToNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            return Regex.Replace(input, @"[^\d]", "");
        }

        public static string EnsureTrailingSlash(this string input)
        {
            var s = input.PackToNull() ?? "";
            if (!s.EndsWith("/"))
                return s + "/";
            return s;
        }

        public static string TrySubstring(this string value, int startIndex, int length = 0)
        {
            if (value == null) return null;

            if (length == 0 || value.Length - startIndex < length)
                return value.Substring(startIndex);
            else
                return value.Substring(startIndex, length);
        }

        public static string PackToNull(this string o, int? maxlen = null)
        {
            var s = o.Pack(maxlen);
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }

        public static string format(this string o, params object[] args)
        {
            return string.Format(o ?? "{0}", args);
        }


        public static string Ellipsize(this string text, int characterCount, string ellipsis = "…")
        {
            if (text == null) return null;

            if (characterCount <= 0 || text.Length <= characterCount)
                return text;

            return text.Substring(0, characterCount) + ellipsis;
        }


        /// <summary>
        /// Whether the char is a letter between A and Z or not
        /// </summary>
        public static bool IsLetter(this char c)
        {
            return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z') || ('А' <= c && c <= 'Я') || ('а' <= c && c <= 'я');
        }

        public static bool IsSpace(this char c)
        {
            return (c == '\r' || c == '\n' || c == '\t' || c == '\f' || c == ' ');
        }

        public static bool IsDigit(this char c)
        {
            return char.IsDigit(c);
        }

        public static string[] GetLines(this string source)
        {
            return source.NoNull(c => c.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None), new string[] { });
        }


        private static Regex regFileCleaner = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()))));

        public static string CleanFileName(this string fileName)
        {
            if (fileName == null) return null;
            return regFileCleaner.Replace(fileName, string.Empty);
        }


        public static string RemoveTags(this string html)
        {
            return string.IsNullOrEmpty(html)
                ? ""
                : Regex.Replace(html, "<[^<>]*>", "", RegexOptions.Singleline);
        }

        public static bool isCyrillic(this string o, bool isAll = false)
        {
            if (string.IsNullOrWhiteSpace(o)) return false;
            return (isAll) ? Regex.IsMatch(o, @"\P{IsCyrillic}") : Regex.IsMatch(o, @"\p{IsCyrillic}");
        }

        public static int StrToInt(this string o, int def = 0)
        {
            int r;
            if (int.TryParse(o.Pack() ?? "", out r))
                return r;
            return def;
        }

        public static int? StrToIntNullable(this string o, int? def = null)
        {
            int r;
            if (int.TryParse(o.Pack() ?? "", out r))
                return r;
            return def;
        }

        public static bool StrToBool(this string o)
        {
            if (o.IsNullOrWhiteSpace()) return false;

            o = o.Trim().ToLower();
            switch (o)
            {
                case "true": return true;
                case "верно": return true;
                case "vrai": return true;
                case "on": return true;
                case "+": return true;
                case "*": return true;
                case "1": return true;
            }

            return Boolean.TrueString.ToLower() == o;
        }



        /// <summary>
        /// Convert string value to decimal ignore the culture.
        ///     string v1 = "123.4";
        ///     string v2 = "123,4";
        ///     string v3 = "1,234.5";
        ///     string v4 = "1.234,5";
        ///     string v5 = "123";
        ///     string v6 = "1,234,567.89";
        ///     string v7 = "1.234.567,89";
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Decimal value.</returns>
        public static decimal StrToDecimal(this string value, decimal def = 0)
        {

            var r = StrToDecimalNullable(value);
            return (r.HasValue) ? r.Value : def;
        }


        public static decimal? StrToDecimalNullable(this string value, decimal? def = null)
        {
            try
            {
                decimal number;
                string tempValue = value;
                var punctuation = value.Where(x => char.IsPunctuation(x)).Distinct();
                int count = punctuation.Count();
                NumberFormatInfo format = CultureInfo.InvariantCulture.NumberFormat;
                switch (count)
                {
                    case 0:
                        break;
                    case 1:
                        tempValue = value.Replace(",", ".");
                        break;
                    case 2:
                        if (punctuation.ElementAt(0) == '.')
                            tempValue = value.SwapChar('.', ',');
                        break;
                    default:
                        throw new InvalidCastException();
                }

                number = decimal.Parse(tempValue, format);
                return number;
            }
            catch
            {
                return def;
            }
        }
        /// <summary>
        /// Swaps the char.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        static string SwapChar(this string value, char from, char to)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            StringBuilder builder = new StringBuilder();

            foreach (var item in value)
            {
                char c = item;
                if (c == from)
                    c = to;
                else if (c == to)
                    c = from;

                builder.Append(c);
            }
            return builder.ToString();
        }

        public static Guid StrToGuid(this string o, Guid? def = null)
        {
            Guid r;
            if (Guid.TryParse(o.Pack() ?? "", out r))
                return r;
            return def.GetValueOrDefault();
        }

        public static Guid? StrToGuidNullable(this string o, Guid? def = null)
        {
            Guid r;
            if (Guid.TryParse(o.Pack() ?? "", out r))
                return r;
            return def;
        }

        static string[] dtFrmts = new string[] { "dd.MM.yyyy", "yyyy-MM-dd", "dd.MM.yy", "yyyy-MM-dd HH:mm", "yyyy-MM-dd HH:mm:ss", "dd.MM.yyyy HH:mm", "yyyy-MM-ddTHH:mm:ss", "dd.MM.yyyy HH:mm:ss" };

        public static DateTime StrToDate(this string o, DateTime? def = null)
        {
            DateTime d;
            if (DateTime.TryParseExact(o.Pack() ?? "", dtFrmts, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out d))
                return d;
            return def ?? DateTime.Now;
        }

        public static DateTime? StrToDateNullable(this string o, DateTime? def = null)
        {
            DateTime d;
            if (DateTime.TryParseExact(o.Pack() ?? "", dtFrmts, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out d))
                return d;
            return def;
        }


        public static bool IsNullOrEmpty(this string o)
        {
            return string.IsNullOrEmpty(o);
        }

        public static bool IsNullOrWhiteSpace(this string o)
        {
            return string.IsNullOrWhiteSpace(o);
        }

        public static string TrimStart(this string target, string trimString)
        {
            string result = target;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }

        public static string TrimEnd(this string target, string trimString)
        {
            string result = target;
            while (result.EndsWith(trimString))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
        }
    }
}
