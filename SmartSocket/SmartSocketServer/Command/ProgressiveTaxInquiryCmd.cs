﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.SocketBase.Protocol;
using SmartSocketData;

namespace SmartSocketServer.Command
{
    class ProgressiveTaxInquiryCmd : ICommand
    {
        public void execute(MainSession session, SocketJsonData requestInfo)
        {

        }

        public void execute(SocketJsonData requestInfo)
        {

        }

        public string getProgressivTax(double usagePower)
        {
            return null;
        }
    }
}
