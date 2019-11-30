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
    class ElectPowerInquiryCmd : Command
    {
        public ElectPowerInquiryCmd()
            : base(null)
        {

        }
        public ElectPowerInquiryCmd(SubjectModel subject)
            : base(subject)
        {

        }

        public override void execute(MainSession session, SocketJsonData requestInfo)
        {
            string socketData = saveElectPower(requestInfo);
            session.Send(socketData);
        }

        public override void execute(SocketJsonData requestInfo)
        {
            string socketData = saveElectPower(requestInfo);
            subjectModel.setModel(socketData);
        }

        private string saveElectPower(SocketJsonData requestInfo)
        {
            DayPowerRepository dayPowerRepository = new DayPowerRepository();
            string measureId = requestInfo.getJsonKeyValue("measureProduct_id");
            string date = requestInfo.getJsonKeyValue("contractDate");
            int contractDay = Convert.ToInt32(date);
            
            DateTime today = DateTime.Today;
            DateTime contractDate = new DateTime(today.Year, today.Month, contractDay);

            int result = DateTime.Compare(contractDate, today);

            if (0 < result)
            {
                if (today.Month == 1)
                    contractDate = new DateTime(today.Year - 1, 12, contractDay);
                else
                    contractDate = new DateTime(today.Year, today.Month - 1, contractDay);
            }

            DayPower currentPower = dayPowerRepository.Find(today, measureId);
            List<DayPower> dayPowers = dayPowerRepository.FindListDayPower(contractDate, measureId).Result;
            double usagePower = 0;
            double standbyPower = 0;

            foreach (DayPower power in dayPowers)
            {
                usagePower += power.usagePower;
                standbyPower += power.standbyPower;
            }

            int progressiveTax = 0;
            if (usagePower < 200)
                progressiveTax = 1;
            else if (usagePower < 400)
                progressiveTax = 2;
            else
                progressiveTax = 3;

            string electData = "";

            SocketJsonData jsonData = new SocketJsonData();
            DateTime endDate;
            if (contractDate.Day == 1)
            {
                switch (contractDate.Month)
                {
                    case 2:
                        endDate = new DateTime(contractDate.Year, contractDate.Month, 28);
                        break;
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        endDate = new DateTime(contractDate.Year, contractDate.Month, 31);
                        break;
                    default:
                        endDate = new DateTime(contractDate.Year, contractDate.Month, 30);
                        break;
                }
            }
            else
                endDate = new DateTime(contractDate.Year, contractDate.Month + 1, contractDate.Day - 1);

            jsonData.addElement("usagePower", Convert.ToString(usagePower));
            jsonData.addElement("standbyPower", Convert.ToString(standbyPower));
            jsonData.addElement("progressiveTax", Convert.ToString(progressiveTax));
            jsonData.addElement("startDate", contractDate.ToShortDateString());
            jsonData.addElement("endDate", endDate.ToShortDateString());

            if(currentPower == null)
            {
                jsonData.addElement("result", false);
            }
            else
            {
                jsonData.addElement("currentPower", currentPower.getPowerArray());
            }

            electData = (int)SocketCommand.ElectPowerInquiry + ";";
            electData += jsonData.getJObject();

            return electData;
        }
    }
}
