using System.Collections.Generic;

namespace OzCodeLinqArticle
{
    public interface IMessageEncoder
    {
        IEnumerable<CommandElement> Decode(byte[] bytes);
        byte[] Encode(IEnumerable<CommandElement> commandValues);
    }
}