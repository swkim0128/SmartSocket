using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using SmartSocketServer.Command;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine;

using SmartSocketMongoDB;
using SmartSocketMongoDB.Model;
using SmartSocketServer.view;

namespace SmartSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            /*
            Console.WriteLine("start the server!");
            Console.ReadKey();
            Console.WriteLine();

            var bootstrap = BootstrapFactory.CreateBootstrap();

            if (!bootstrap.Initialize())
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadKey();
                return;
            }

            var result = bootstrap.Start();

            Console.WriteLine("Start result: {0}", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            Console.WriteLine();

            // Stop the appserver
            bootstrap.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
            */
        }
    }
}
