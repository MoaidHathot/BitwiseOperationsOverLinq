using System.Collections.Generic;

namespace OzCodeLinqArticle
{
    public interface IMessageEncoder
    {
        IEnumerable<KeyValuePair<string, int>> Decode(byte[] bytes);
        byte[] Encode(IEnumerable<KeyValuePair<string, int>> message);
    }
}