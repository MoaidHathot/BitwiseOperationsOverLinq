using System.Collections.Generic;

namespace OzCodeLinqArticle
{
    public interface IMessageEncoder
    {
        IEnumerable<CommandPart> Decode(byte[] bytes);
        byte[] Encode(IEnumerable<CommandPart> commandValues);
    }
}