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
            var valueMap = message.ToDictionary(pair => pair.Key, pair => pair.Value);



            //var result = _layoutItems
            //    .Select(Item => new LayoutItemValue<string>(map[Item.Name], Item.BitCount))
            //    .Select(
            //        Item => new LayoutItemValue<int>(Item.Value.IsHexNumber() ? int.Parse(Item.Value.Substring(2), NumberStyles.HexNumber) : int.Parse(Item.Value), Item.BitCount))
            //    .Select(Item => new LayoutItemValue<byte[]>(BitConverter.GetBytes(Item.Value), Item.BitCount))
            //    .Select(Item => new LayoutItemValue<IEnumerable<Bit>>(Item.Value.ToBits(), Item.BitCount))
            //    .Select(Item => new LayoutItemValue<IEnumerable<Bit>>(Item.Value.Take(Item.BitCount), Item.BitCount));


            //Bug: Remove pad before batch, the length is not dividable by 8.


            foreach (var item in _layoutItems)
            {
                valueMap[item.Name].ToByteArray().ToBits().Pad(item.BitCount, Bit.Off).Take(item.BitCount);
            }

            var bits = _layoutItems.Select(item => new {Item = item, value = valueMap[item.Name]})
                .SelectMany(pair => pair.value
                    .ToByteArray()
                    .ToBits()
                    .Take(pair.Item.BitCount))
                .Pad(32, Bit.Off)
                .Batch(8)
                .ToBytes();
                //.ToArray();


            //bits.ToArray();
            throw new NotImplementedException();
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