using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeLinqArticle
{
    public class MessagePart
    {
        public string Name { get; }
        public int Value { get; }

        public MessagePart(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}

//public override string ToString() => $"Name: {Name}, Value: {Value}";