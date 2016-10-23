using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeLinqArticle
{
    public interface IFpgaAccess
    {
        Task Send(IEnumerable<MessagePart> command);
        Task<IEnumerable<MessagePart>> Receive();
    }
}
