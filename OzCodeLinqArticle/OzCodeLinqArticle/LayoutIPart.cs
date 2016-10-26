using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeLinqArticle
{
    public class LayoutPart
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int BitCount { get; set; }

        public LayoutPart(string name, int index, int bitCount)
        {
            Name = name;
            Index = index;
            BitCount = bitCount;
        }
    }
}