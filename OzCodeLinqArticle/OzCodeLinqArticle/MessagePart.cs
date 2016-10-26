using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeLinqArticle
{
    public class MessagePart : LayoutPart
    {
        public int Value { get; }

        public MessagePart(string name, int index, int bitCount, int value)
            : base(name, index, bitCount)
        {
            Value = value;
        }
    }
}