using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Config;

using SmartSocketServer.Command;
using SmartSocketServer.Session;
using SmartSocketServer.view;
using SmartSocketData;

namespace SmartSocketServer
{
    class MainServer : AppServer<MainSession, StringRequestInfo>
    {
        private Invoker invoker;

        public MainServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", "")))
        {
            invoker = new Invoker();
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }

        protected override void OnNewSessionConnected(MainSession session)
        {
            base.OnNewSessionConnected(session);
        }

        protected override void ExecuteCommand(MainSession session, StringRequestInfo requestInfo)
        {
            int key = Convert.ToInt32(requestInfo.Key);
            SocketJsonData jsonData = new SocketJsonData();
            jsonData.setJObj(requestInfo.Body);

            Console.Write("ExecuteCommand : " + key + " requestInfo : " + requestInfo.Body);
            invoker.executeCmd(key, session, jsonData);
        }
    }
}
