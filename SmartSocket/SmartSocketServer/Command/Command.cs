using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartSocketServer.view;
using SmartSocketData;

namespace SmartSocketServer.Command
{
    abstract class Command : ICommand
    {
        protected SubjectModel subjectModel;

        public Command(SubjectModel subject)
        {
            this.subjectModel = subject;
        }

        public abstract void execute(MainSession session, SocketJsonData requestInfo);
        public abstract void execute(SocketJsonData requestInfo);
    }
}
