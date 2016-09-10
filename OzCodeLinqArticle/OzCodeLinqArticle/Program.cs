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
            var configuration = ReadMessageConfiguration("Messages.xml");

            var encoder = new MessageEncoder(configuration.LayoutItems);

            var encodedMessage = encoder.Encode(configuration.Messages[0]);
            Console.WriteLine(encodedMessage.ToHexNumber());
            Console.WriteLine(encodedMessage.ToBits().Reverse().Select(b => b.ToString()).Aggregate((a, b) => $"{a}{b}"));
            var decodedMessage = encoder.Decode(encodedMessage);

            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();

        }

        static MessageRepository ReadMessageConfiguration(string configuratinPath)
        {
            var document = XDocument.Load(configuratinPath);

            var layout = document.Descendants("Layout")
                .Descendants("LayoutElement")
                .Select(item => new LayoutItem(item.Attribute("Name").Value, int.Parse(item.Attribute("Index").Value), int.Parse(item.Attribute("BitCount").Value)));

            var messages = document
                .Descendants("Message")
                .Select(msg =>  msg.Attributes()
                .Select(attribute => new KeyValuePair<string, string>(attribute.Name.ToString(), attribute.Value)).ToArray())
                .ToArray();

            return new MessageRepository()
            {
                LayoutItems = layout,
                Messages = messages
            };
        }
    }

    class MessageRepository
    {
        public IEnumerable<LayoutItem> LayoutItems { get; set; }
        public IEnumerable<KeyValuePair<string, string>>[] Messages { get; set; }
    }
}
