using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pws.Shared.Hubs
{
    public interface IMyHub
    {
        Task SpreadAsync(string content);
    }
}
