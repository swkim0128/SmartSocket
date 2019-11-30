using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartSocketData;
using SmartSocketServer.view;

namespace SmartSocketServer.Command
{
    class Invoker
    {
        ICommand[] command = new ICommand[8];

        public Invoker()
        {
            command[(int)SocketCommand.ElectPowerSave] = new ElectPowerSaveCmd();
            command[(int)SocketCommand.ElectPowerInquiry] = new ElectPowerInquiryCmd();
            command[(int)SocketCommand.ElectChargeInquiry] = new ElectChargeInquiryCmd();
            command[(int)SocketCommand.TaxStandardSave] = new TaxStandardSaveCmd();
        }

        public Invoker(SubjectModel subject)
        {
            command[(int)SocketCommand.ElectPowerSave] = new ElectPowerSaveCmd(subject);
            command[(int)SocketCommand.ElectPowerInquiry] = new ElectPowerInquiryCmd(subject);
            command[(int)SocketCommand.ElectChargeInquiry] = new ElectChargeInquiryCmd(subject);
            command[(int)SocketCommand.TaxStandardSave] = new TaxStandardSaveCmd(subject);
            command[(int)SocketCommand.TaxStandardInquiry] = new TaxStandardInquiryCmd(subject);
        }

        public void setCommand(int slot, ICommand command)
        {
            this.command[slot] = command;
        }

        public void executeCmd(int slot, MainSession session, SocketJsonData requestInfo)
        {
            this.command[slot].execute(session, requestInfo);
        }

        public void executeCmd(int slot, SocketJsonData requestInfo)
        {
            this.command[slot].execute(requestInfo);
        }
    }
}
