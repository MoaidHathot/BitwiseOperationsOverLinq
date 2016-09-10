using System;

namespace OzCodeLinqArticle
{
    public static class NumberFormattingExtensions
    {
        public static string ToHexNumber(this byte[] bytes) => BitConverter.ToString(bytes).Replace("-", "");

        public static byte[] IntegerToByteArray(this string number)
        {
            return BitConverter.GetBytes(ulong.Parse(number));
        }

        public static byte[] HexToByteArray(this string hex)
        {
            if (!hex.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new FormatException($"The input string '{hex}' wasn't in the correct format. It didn't start with '0x'");
            }

            //Expects the first two characters to be "0x"
            var integerValue = ulong.Parse(hex.Substring(2), System.Globalization.NumberStyles.HexNumber);

            return BitConverter.GetBytes(integerValue);
        }

        public static byte[] ToByteArray(this int number)
        {
            return BitConverter.GetBytes(number);
        }

        public static int ToInt(this byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        public static bool IsHexNumber(this string number)
        {
            if (null == number)
            {
                return false;
            }

            if (!number.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            number = number.Substring(2);

            long result;
            return long.TryParse(number, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out result);
        }

    }
}