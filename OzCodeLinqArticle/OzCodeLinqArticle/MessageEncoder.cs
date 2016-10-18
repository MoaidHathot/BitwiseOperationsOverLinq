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
        private readonly IEnumerable<LayoutElement> _layout;

        public MessageEncoder(IEnumerable<LayoutElement> messageSections)
        {
            _layout = messageSections.OrderBy(section => section.Index).ToArray();
        }

        public byte[] Encode(IEnumerable<CommandElement> commandValues)
        {
            var valueMap = commandValues.ToDictionary(element => element.Name, element => element.Value);

            return _layout
                .Select(item => new {Item = item, Value = valueMap[item.Name]})
                .SelectMany(pair =>
                    BitConverter.GetBytes(pair.Value)
                    .ToBits()
                    .Pad(pair.Item.BitCount, Bit.Off)
                    .Take(pair.Item.BitCount))
                .Batch(8)
                .ToBytes()
                .ToArray();
            //var valueMap = message.ToDictionary(pair => pair.Key, pair => pair.Value);



            //var result = _layout
            //    .Select(Item => new LayoutItemValue<string>(map[Item.Name], Item.BitCount))
            //    .Select(
            //        Item => new LayoutItemValue<int>(Item.Value.IsHexNumber() ? int.Parse(Item.Value.Substring(2), NumberStyles.HexNumber) : int.Parse(Item.Value), Item.BitCount))
            //    .Select(Item => new LayoutItemValue<byte[]>(BitConverter.GetBytes(Item.Value), Item.BitCount))
            //    .Select(Item => new LayoutItemValue<IEnumerable<Bit>>(Item.Value.ToBits(), Item.BitCount))
            //    .Select(Item => new LayoutItemValue<IEnumerable<Bit>>(Item.Value.Take(Item.BitCount), Item.BitCount));


            //Bug: Remove pad before batch, the length is not dividable by 8.



            //foreach (var item in _layout)
            //{
            //    valueMap[item.Name].ToByteArray().ToBits().Pad(item.BitCount, Bit.Off).Take(item.BitCount);
            //}

            //var bits = _layout.Select(item => new {Item = item, value = valueMap[item.Name]})
            //    .SelectMany(pair => pair.value
            //        .ToByteArray()
            //        .ToBits()
            //        .Take(pair.Item.BitCount))
            //    .Pad(32, Bit.Off)
            //    .Batch(8)
            //    .ToBytes();
            //    //.ToArray();


            ////bits.ToArray();
            //throw new NotImplementedException();
        }

        public IEnumerable<CommandElement> Decode(byte[] bytes)
        {
            var bits = bytes.ToBits();

            return _layout.Select(layoutElement =>
            {
                var slice = bits
                    .Take(layoutElement.BitCount)
                    .Batch(8)
                    .ToBytes()
                    .Pad(sizeof(int), (byte) 0)
                    .Take(sizeof(int))
                    .ToArray();

                bits = bits.Skip(layoutElement.BitCount);

                return new CommandElement(layoutElement.Name, BitConverter.ToInt32(slice, 0));
            });
        }

        //public class LayoutItemValue<T>
        //{
        //    public T Value { get; set; }
        //    public int BitCount { get; set; }

        //    public LayoutItemValue(T value, int bitCount)
        //    {
        //        Value = value;
        //        BitCount = bitCount;
        //    }

        //    public override string ToString() => $"Value: {Value}, Bitcount: {BitCount}";
        //}

        //public IEnumerable<KeyValuePair<string, int>> Decode(byte[] bytes)
        //{
        //    var bits = bytes.ToBits();

        //    return _layout.Select(item =>
        //    {
        //        var value = bits
        //        .Take(item.BitCount)
        //        .ToByteArray()
        //        .ToInt();

        //        bits = bits.Skip(item.BitCount);

        //        return new KeyValuePair<string, int>(item.Name, value);
        //    });
        //}
    }
}