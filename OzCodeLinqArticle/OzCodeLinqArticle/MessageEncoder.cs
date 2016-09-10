using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using MoreLinq;

namespace OzCodeLinqArticle
{
    public class MessageEncoder : IMessageEncoder
    {
        private readonly IEnumerable<LayoutItem> _layoutItems;

        public MessageEncoder(IEnumerable<LayoutItem> messageSections)
        {
            _layoutItems = messageSections.OrderBy(section => section.Index).ToArray();
        }

        public byte[] Encode(IEnumerable<KeyValuePair<string, string>> message)
        {
            var map = message.ToDictionary(pair => pair.Key, pair => pair.Value);



            var result = _layoutItems
                .Select(item => new LayoutItemValue<string>(map[item.Name], item.BitCount))
                .Select(
                    item => new LayoutItemValue<int>(item.Value.IsHexNumber() ? int.Parse(item.Value.Substring(2), NumberStyles.HexNumber) : int.Parse(item.Value), item.BitCount))
                .Select(item => new LayoutItemValue<byte[]>(BitConverter.GetBytes(item.Value), item.BitCount))
                .Select(item => new LayoutItemValue<IEnumerable<Bit>>(item.Value.ToBits(), item.BitCount))
                .Select(item => new LayoutItemValue<IEnumerable<Bit>>(item.Value.Take(item.BitCount), item.BitCount));




            //throw new NotImplementedException();
            return _layoutItems
                .SelectMany(item => map[item.Name]
                    .ToByteArray()
                    .ToBits()
                    .Pad(item.BitCount, Bit.Off)
                    .Take(item.BitCount))
                .ToByteArray();
        }

        public class LayoutItemValue<T>
        {
            public T Value { get; set; }
            public int BitCount { get; set; }

            public LayoutItemValue(T value, int bitCount)
            {
                Value = value;
                BitCount = bitCount;
            }

            public override string ToString() => $"Value: {Value}, Bitcount: {BitCount}";
    }

        public IEnumerable<KeyValuePair<string, int>> Decode(byte[] bytes)
        {
            var bits = bytes.ToBits();

            return _layoutItems.Select(item =>
            {
                var value = bits
                .Take(item.BitCount)
                .ToByteArray()
                .ToInt();

                bits = bits.Skip(item.BitCount);

                return new KeyValuePair<string, int>(item.Name, value);
            });
        }
    }
}