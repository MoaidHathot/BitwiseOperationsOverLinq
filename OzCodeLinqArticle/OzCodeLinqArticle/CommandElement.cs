using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeLinqArticle
{
    public class CommandElement
    {
        public string Name { get; }
        public int Value { get; }

        public CommandElement(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}

//public override string ToString() => $"Name: {Name}, Value: {Value}";