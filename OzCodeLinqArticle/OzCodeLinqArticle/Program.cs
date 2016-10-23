using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MoreLinq;

namespace OzCodeLinqArticle
{
    class Program
    {
        static void Main(string[] args)
        {
            var layout = new[]
            {
                new LayoutPart("Part 0", 0, 3),
                new LayoutPart("Part 1", 1, 4),
                new LayoutPart("Part 2", 2, 11),
            };

            var message = new[]
            {
                new MessagePart("Part 0", 1),
                new MessagePart("Part 1", 0xA),
                new MessagePart("Part 2", 0x1F0),
            };

            var serializer = new MessageSerializer();

            var serializedBytes = serializer.Serialize(layout, message);
            var deserializedBytes = serializer.Deserialize(layout, serializedBytes);

            var converted = BitConverter.ToString(serializedBytes).Replace("-", "");
            Console.WriteLine(converted);

            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();

        }
    }
}
