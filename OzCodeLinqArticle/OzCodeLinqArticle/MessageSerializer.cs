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
        public byte[] Serialize(IEnumerable<MessagePart> messageParts)
        {
            return messageParts.OrderBy(part => part.Index)
                .SelectMany(part =>  
                    BitConverter.GetBytes(part.Value)
                    .ToBits()
                    .Pad(part.BitCount, Bit.Off)
                    .Take(part.BitCount))
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
                        .Pad(sizeof(int), (byte)0)
                        .Take(sizeof(int))
                .ToArray()
                .ToInt();

                bits = bits.Skip(part.BitCount);

                return new MessagePart(part.Name, part.Index, part.BitCount, slice);

            }).ToArray();
        }
    }
}