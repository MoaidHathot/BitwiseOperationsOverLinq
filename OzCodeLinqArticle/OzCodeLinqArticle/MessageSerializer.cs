using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using MoreLinq;

namespace OzCodeLinqArticle
{
    public class MessageSerializer : IMessageSerializer
    {
        public byte[] Serialize(IEnumerable<LayoutPart> layout, IEnumerable<MessagePart> messageParts)
        {
            var partsMap = messageParts.ToDictionary(part => part.Name, part => part.Value);

            return layout.OrderBy(part => part.Index).Select(part =>
                    new { Part = part, Value = partsMap[part.Name] })
                .SelectMany(pair =>
                    BitConverter.GetBytes(pair.Value)
                    .ToBits()
                    .Pad(pair.Part.BitCount, Bit.Off)
                    .Take(pair.Part.BitCount))
                .Batch(8)
                .Select(slice => slice.ToByte())
                .ToArray();
        }

        public IEnumerable<MessagePart> Deserialize(IEnumerable<LayoutPart> layout, byte[] bytes)
        {
            var bits = bytes.ToBits();

            return layout.OrderBy(part => part.Index).Select(part =>
            {
                var slice =
                    bits
                        .Take(part.BitCount)
                        .Batch(8)
                        .ToBytes()
                        .Pad(sizeof(int), (byte) 0)
                        .Take(sizeof(int))
                .ToArray()
                .ToInt();

                bits = bits.Skip(part.BitCount);

                return new MessagePart(part.Name, slice);

            }).ToArray();
        }
    }
}