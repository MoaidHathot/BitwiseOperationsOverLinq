﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeLinqArticle
{
    public class LayoutItem
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int BitCount { get; set; }

        public LayoutItem(string name, int index, int bitCount)
        {
            Name = name;
            Index = index;
            BitCount = bitCount;
        }

        public override string ToString() => $"Name: {Name}, Index: {Index}, BitCount: {BitCount}";
    }
}