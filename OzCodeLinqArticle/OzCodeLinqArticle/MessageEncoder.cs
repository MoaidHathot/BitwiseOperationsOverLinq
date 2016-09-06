using System;
using System.Collections.Generic;
using System.Linq;

namespace OzCodeLinqArticle
{
    public class MessageEncoder : IMessageEncoder
    {
        private IEnumerable<MessageSection> _messageSections;

        public MessageEncoder(IEnumerable<MessageSection> messageSections)
        {
            _messageSections = messageSections.OrderBy(section => section.Index);
        }

        public byte[] Encode(IEnumerable<KeyValuePair<string, int>> message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, int>> Decode(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}