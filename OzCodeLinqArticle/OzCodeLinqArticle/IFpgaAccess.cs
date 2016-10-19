using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeLinqArticle
{
    public interface IFpgaAccess
    {
        Task Send(IEnumerable<CommandPart> command);
        Task<IEnumerable<CommandPart>> Receive();
    }
}
