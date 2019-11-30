using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.SocketBase.Protocol;

using SmartSocketData;

namespace SmartSocketServer.Command
{
    interface ICommand
    {
        void execute(MainSession session, SocketJsonData requestInfo);
        void execute(SocketJsonData requestInfo);
    }
}
