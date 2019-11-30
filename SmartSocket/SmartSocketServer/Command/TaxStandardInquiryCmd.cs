using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSocketData;

using SmartSocketMongoDB;
using SmartSocketMongoDB.Model;
using SmartSocketServer.view;

namespace SmartSocketServer.Command
{
    class TaxStandardInquiryCmd : Command
    {

        public TaxStandardInquiryCmd()
            : base(null)
        {

        }
        public TaxStandardInquiryCmd(SubjectModel subject)
            : base(subject)
        {

        }

        public override void execute(MainSession session, SocketJsonData requestInfo)
        {
            
        }

        public override void execute(SocketJsonData requestInfo)
        {
            string socketData = inquiryStandard(requestInfo);
            subjectModel.setModel(socketData);
        }

        public string inquiryStandard(SocketJsonData requestInfo)
        {
            string _id = requestInfo.getJsonKeyValue("_id");

            UserRepository userRepository = new UserRepository();
            User user = userRepository.Find("_id", _id).Result;

            SocketJsonData jsonData = new SocketJsonData();
            if(user == null)
            {
                jsonData.addElement("result", false);
            }
            else
            {
                jsonData.setJObj(user.standard);
                jsonData.addElement("result", true);
            }

            string socketData = (int)SocketCommand.TaxStandardInquiry + ";" +
                jsonData.getJObject();
            return socketData;
        }

    }
}
