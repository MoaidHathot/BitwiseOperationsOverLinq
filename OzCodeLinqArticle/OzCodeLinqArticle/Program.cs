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
                new LayoutElement("SendAck", 0, 1), //Indicates whether to reply with an acknowledgement or not
                new LayoutElement("CommandId", 1, 5), //The command's id
                new LayoutElement("SenderId", 2, 4), //The sender's id
                new LayoutElement("Headers", 3, 6), //Headers about the command and the state
                new LayoutElement("Content", 4, 8), //Actual content
            };

            var command = new[]
            {
                new CommandElement("SendAck", 1), //Reply with an acknowledgement
                new CommandElement("CommandId", 0xf), //Command's id
                new CommandElement("SenderId", 10), //Sender's id
                new CommandElement("Headers", 0x10), //Predefined headers
                new CommandElement("Content", 0xff), //predefined content
            };

            var encoder = new MessageEncoder(layout);

            var encodedBytes = encoder.Encode(command);
            var decodedBytes = encoder.Decode(encodedBytes);
            //var configuration = ReadMessageConfiguration("Messages.xml");

            //var encoder = new MessageEncoder(configuration.LayoutItems);

            //var encodedMessage = encoder.Encode(configuration.Messages[0]);
            //Console.WriteLine(encodedMessage.ToHexNumber());
            //Console.WriteLine(encodedMessage.ToBits().Reverse().Select(b => b.ToString()).Aggregate((a, b) => $"{a}{b}"));
            //var decodedMessage = encoder.Decode(encodedMessage);

            var converted = BitConverter.ToString(encodedBytes).Replace("-", "");
            Console.WriteLine(converted);

            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();

        }

        static MessageRepository ReadMessageConfiguration(string configuratinPath)
        {
            var document = XDocument.Load(configuratinPath);

            var layout = document.Descendants("Layout")
                .Descendants("LayoutElement")
                .Select(item => new LayoutElement(item.Attribute("Name").Value, int.Parse(item.Attribute("Index").Value), int.Parse(item.Attribute("BitCount").Value)));

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
        public IEnumerable<LayoutElement> LayoutItems { get; set; }
        public IEnumerable<KeyValuePair<string, string>>[] Messages { get; set; }
    }
}
