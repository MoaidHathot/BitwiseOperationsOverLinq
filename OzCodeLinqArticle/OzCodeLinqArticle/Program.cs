using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace OzCodeLinqArticle
{
    class Program
    {
        static void Main(string[] args)
        {
            var bytes = "0xffff".ToByteArray();

            var partitions = bytes.Partition(8).ToArray();
            partitions.Consume();


            
        }
    }
}
