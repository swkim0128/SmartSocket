using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.SocketBase.Protocol;

using SmartSocketMongoDB;
using SmartSocketMongoDB.Model;
using SmartSocketData;
using SmartSocketServer.view;

namespace SmartSocketServer.Command
{
    class TaxStandardSaveCmd : Command
    {
        public TaxStandardSaveCmd() 
            : base(null)
        {

        }
        public TaxStandardSaveCmd(SubjectModel subject)
            : base(subject)
        {

        }

        public override void execute(MainSession session, SocketJsonData requestInfo)
        {
            saveTaxStandard(requestInfo);
        }

        public override void execute(SocketJsonData requestInfo)
        {
            string saveData = saveTaxStandard(requestInfo);
            subjectModel.setModel(saveData);
        }       

        private string saveTaxStandard(SocketJsonData requestInfo)
        {
            UserRepository userRepository = new UserRepository();
            string userId = requestInfo.getJsonKeyValue("_id");
            int contractDate = Convert.ToInt32(requestInfo.getJsonKeyValue("contractDate"));
            string contract = requestInfo.getJsonKeyValue("contract");
            string family = requestInfo.getJsonKeyValue("family");
            string welfare = requestInfo.getJsonKeyValue("welfare");
            int contractPower = Convert.ToInt32(requestInfo.getJsonKeyValue("contractPower"));
            string receivingVoltage = requestInfo.getJsonKeyValue("receivingVoltage");
            string measureProduct_id = requestInfo.getJsonKeyValue("measureProduct_id");

            User user = userRepository.Find("_id", userId).Result;

            SocketJsonData jsonData = new SocketJsonData();
            if (user != null)
            {
                user.standard.contractDate = contractDate;
                user.standard.contract = contract;
                user.standard.family = family;
                user.standard.welfare = welfare;
                user.standard.contractPower = contractPower;
                user.standard.receivingVoltage = receivingVoltage;

                bool result = userRepository.Update(userId, user);

                if (result)
                {
                    jsonData.addElement("result", true);
                }
                else
                {
                    jsonData.addElement("result", false);
                }
            }
            else
            {
                user = new User
                {
                    ID = requestInfo.getJsonKeyValue("_id"),
                    standard = new Standard
                    {
                        contractDate = contractDate,
                        contract = contract,
                        family = family,
                        welfare = welfare,
                        contractPower = contractPower,
                        receivingVoltage = receivingVoltage,
                    },
                    measureProduct_id = measureProduct_id,
                };

                userRepository.Insert(user).Wait();
                jsonData.addElement("result", true);
            }

            string socketData = (int)SocketCommand.TaxStandardSave + ";" +
                jsonData.getJObject();

            return socketData;
        }
    }
}
