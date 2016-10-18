using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MoreLinq;

namespace OzCodeLinqArticle
{
    public static class BitExtensions
    {
        public static IEnumerable<Bit> ToBits(this byte @byte)
        {
            for (var index = 0; index < 8; ++index, @byte >>= 1)
            {
                yield return 1 == (@byte & 1);
            }
        }

        public static IEnumerable<Bit> ToBits(this byte[] bytes) => bytes.SelectMany(@byte => @byte.ToBits());

        public static IEnumerable<Bit> PadToFit(this IEnumerable<Bit> bits, Bit bitPadValue, int bitCount) => bits.Concat(Enumerable.Repeat(bitPadValue, bitCount)).Take(bitCount);

        public static IEnumerable<Bit> RightPadToFit(this IEnumerable<Bit> array, Bit padValue, int length) => PadToFit(array, padValue, length);

        public static IEnumerable<Bit> LeftPadToFit(this IEnumerable<Bit> array, Bit padValue, int length) => Enumerable.Empty<Bit>().PadToFit(padValue, length).Concat(array);

        public static IEnumerable<Bit> RemoveTrailingZeros(this IEnumerable<Bit> bits) => bits.Take(1).Concat(bits.Skip(1).Reverse().SkipWhile((b, i) => !b).Reverse());

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> items, int partitionSize) => items.Select((item, inx) => new { item, inx }).GroupBy(x => x.inx / partitionSize).Select(g => g.Select(x => x.item));

        public static byte[] ToByteArray(this IEnumerable<Bit> bits)
        {
            var bitsArray = new BitArray(bits.Select(b => (bool)b).ToArray());

            var bytes = new byte[(bitsArray.Length - 1) / 8 + 1];

            bitsArray.CopyTo(bytes, 0);

            return bytes;
        }

        public static byte ToByte(this IEnumerable<Bit> bits)
        {
            int result = 0;

            int index = 0;
            foreach (var bit in bits)
            {
                result = result | (byte)(bit << index++);

                if (index > 8)
                {
                    throw new ArgumentOutOfRangeException(nameof(bits));
                }
            }

            return (byte)result;
        }

        public static IEnumerable<byte> ToBytes(this IEnumerable<IEnumerable<Bit>> bits)
        {
            return bits.Select(b => b.ToByte());
        }

        public static byte[] ToByteArray(this string value) => value.IsHexNumber() ? value.HexToByteArray() : value.IntegerToByteArray();
        //{
        //    //var value = properties[Item.Name];

        //    if (string.IsNullOrWhiteSpace(value))
        //    {
        //        throw new ArgumentException($"Argument {nameof(value)} is either null, empty or contains only whitespace");
        //    }

        //    try
        //    {
        //        return value.IsHexNumber() ? value.HexToByteArray() : value.IntegerToByteArray();
        //    }
        //    catch (FormatException ex)
        //    {
        //        throw new FormatException($"Failed to parse argument {nameof(value)}", ex);
        //    }
        //}
    }
}