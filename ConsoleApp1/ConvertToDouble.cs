using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public static class ConvertToDouble
    {
        private const double MaxValue = 1.7976931348623157E+308;
        private static readonly Regex DoubleRegex = new(@"-?\d+(\.|,\d{1,x})?", RegexOptions.Compiled);

        public static double ToDouble(this string s)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)) throw new ArgumentNullException();
            if (DoubleRegex.IsMatch(s) == false) throw new ArgumentException();

            if (HasSeparator(s))
                return StringToNumberWithSeparator(s);

            return StringToNumber(s);
        }

        private static bool HasSign(string s)
        {
            return s.Any(c => c is '+' or '-');
        }

        private static bool IsNegative(string s)
        {
            return s.StartsWith('-');
        }

        private static bool IsBiggerThanMax(double d)
        {
            return d > MaxValue;
        }

        private static bool HasSeparator(string s)
        {
            return s.Any(c => c is '.' or ',');
        }

        private static char GetSeparator(string s)
        {
            return s.FirstOrDefault(c => c is '.' or ',');
        }

        private static int GetStartPosition(string s)
        {
            return HasSign(s) ? 1 : 0;
        }

        private static double GetDouble(string s, int startPosition, int separatorPosition, bool leftPart = true)
        {
            double result = 0;
            switch (leftPart){
                case true:
                    var rank = 1;
                    for (var i = separatorPosition - 1; i >= startPosition; i--, rank *= 10){
                        result += (s[i] - '0') * rank;
                        if (IsBiggerThanMax(result)) throw new OverflowException();
                    }

                    break;

                case false:
                    rank = 10;
                    for (var i = separatorPosition + 1; i < s.Length; i++, rank *= 10){
                        result += ((double)s[i] - '0') / rank;
                        if (IsBiggerThanMax(result)) throw new OverflowException();
                    }

                    break;
            }

            return result;
        }

        private static double StringToNumberWithSeparator(string s)
        {
            var startPosition = GetStartPosition(s);
            var separator = GetSeparator(s);

            var separatorPosition = s.IndexOf(separator);

            var leftPart = GetDouble(s, startPosition, separatorPosition);
            var rightPart = GetDouble(s, startPosition, separatorPosition, false);

            var result = leftPart + rightPart;

            if (IsNegative(s)) result = 0 - result;

            return result;
        }

        private static double StringToNumber(string s)
        {
            var startPosition = GetStartPosition(s);
            double result = 0;
            var rank = 1;
            for (var i = s.Length - 1; i >= startPosition; i--, rank *= 10){
                result += (s[i] - '0') * rank;
                if (IsBiggerThanMax(result)) throw new OverflowException();
            }

            if (IsNegative(s)) result = 0 - result;

            return result;
        }
    }
}