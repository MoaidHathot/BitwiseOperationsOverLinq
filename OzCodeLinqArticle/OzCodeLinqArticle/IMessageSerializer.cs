using System.Collections.Generic;

namespace OzCodeLinqArticle
{
    public interface IMessageSerializer
    {
        IEnumerable<MessagePart> Deserialize(IEnumerable<LayoutPart> layout, byte[] bytes);
        byte[] Serialize(IEnumerable<MessagePart> messageParts);
    }
}