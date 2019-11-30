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
    class ElectChargeInquiryCmd : Command
    {
        private string family;
        private string welfare;
        private int contractPower = 0;

        private double usagePower = 0;

        private int basicCharge = 0;
        private int charge = 0;
        private int tariff = 0;
        private int guarantee = 0;
        private int familyDiscountAmount = 0;
        private int welfareDiscountAmount = 0;
        private int vat = 0;
        private int powerFund = 0;
        private int allElectCharge = 0;

        public ElectChargeInquiryCmd() 
            : base(null)
        {

        }
        public ElectChargeInquiryCmd(SubjectModel subject)
            : base(subject)
        {

        }

        public override void execute(MainSession session, SocketJsonData requestInfo)
        {
            string socketData = inquiryElectCharge(requestInfo);
        }

        public override void execute(SocketJsonData requestInfo)
        {
            string socketData = inquiryElectCharge(requestInfo);
            subjectModel.setModel(socketData);
        }

        private string inquiryElectCharge(SocketJsonData requestInfo)
        {
            initData();

            UserRepository userRepository = new UserRepository();
            User user = userRepository.Find("_id", requestInfo.getJsonKeyValue("_id")).Result;

            string measureId = requestInfo.getJsonKeyValue("measureProduct_id");
            int contractDay = user.standard.contractDate;
            family = user.standard.family;
            welfare = user.standard.welfare;
            contractPower = user.standard.contractPower;

            DateTime now = DateTime.Today;
            DateTime contractDate = new DateTime(now.Year, now.Month, contractDay);

            int result = DateTime.Compare(contractDate, now);

            if (0 < result)
            {
                if (now.Month == 1)
                    contractDate = new DateTime(now.Year - 1, 12, contractDay);
                else
                    contractDate = new DateTime(now.Year, now.Month - 1, contractDay);
            }

            DayPowerRepository dayPowerRepository = new DayPowerRepository();

            // 기준 날짜로부터 현재까지의 DayPower 리스트 불러오기
            List<DayPower> dayPowers = dayPowerRepository.FindListDayPower(contractDate, measureId).Result;

            foreach (DayPower power in dayPowers)
            {
                usagePower += power.usagePower;
            }

            ContractStandardRepository standardRepository = new ContractStandardRepository();
            ContractStandard standard = new ContractStandard();
            standard.contract = user.standard.contract;
            standard.receiving = user.standard.receivingVoltage;

            standard = standardRepository.Find(standard).Result;

            calculateElectricPower(standard);
            allElectCharge = basicCharge + tariff + vat + powerFund;

            SocketJsonData jsonData = new SocketJsonData();
            jsonData.addElement("basicCharge", Convert.ToString(basicCharge));
            jsonData.addElement("charge", Convert.ToString(charge));
            jsonData.addElement("tariff", Convert.ToString(tariff));
            jsonData.addElement("guarantee", Convert.ToString(guarantee));
            jsonData.addElement("familyDiscountAmount", Convert.ToString(familyDiscountAmount));
            jsonData.addElement("welfareDiscountAmount", Convert.ToString(welfareDiscountAmount));
            jsonData.addElement("discount", Convert.ToString(guarantee + familyDiscountAmount + welfareDiscountAmount));
            jsonData.addElement("vat", Convert.ToString(vat));
            jsonData.addElement("powerFund", Convert.ToString(powerFund));
            jsonData.addElement("allElectCharge", Convert.ToString(allElectCharge));

            string electCharge = (int)SocketCommand.ElectChargeInquiry + ";" +
                jsonData.getJObject();

            return electCharge;
        }

        private void initData()
        {
            contractPower = 0;
            usagePower = 0;
            basicCharge = 0;
            charge = 0;
            tariff = 0;
            guarantee = 0;
            familyDiscountAmount = 0;
            welfareDiscountAmount = 0;
            vat = 0;
            powerFund = 0;
            allElectCharge = 0;
        }

        private void calculateElectricPower(ContractStandard standard)
        {
            switch(standard.contract)
            {
                case "주택용(고압)":
                case "주택용(저압)":
                    calHomePower(standard);
                    break;
                case "일반용(갑)1":
                case "산업용(갑)1":
                case "교육용(갑)1":
                    calOther1(standard);
                    break;
                case "일반용(갑)2":
                case "산업용(갑)2":
                case "일반용(을)":
                case "교육용(을)":
                    calOther2(standard);
                    break;
            }
        }

        private void calHomePower(ContractStandard standard)
        {
            if(usagePower <= 200)
            {
                basicCharge = standard.getBasic_charge(0);
                charge = (int)(standard.charge.getBasic(0) * usagePower);
            }
            else if(usagePower <= 400)
            {
                basicCharge = standard.getBasic_charge(1);
                charge = (int)(standard.charge.getBasic(0) * 200 +
                    standard.charge.getBasic(1) * (usagePower - 200));
            }
            else
            {
                basicCharge = standard.getBasic_charge(1);
                charge = (int)(standard.charge.getBasic(0) * 200 +
                    standard.charge.getBasic(1) * 200 + 
                    standard.charge.getBasic(2) * (usagePower - 400));
            }

            discount();
            calVatAndPowFund();
        }

        private void discount()
        {
            int sum = basicCharge + charge;

            if (usagePower < 200)
                guarantee = 4000;
            else
                guarantee = 0;

            discountWelfare(sum);
        }

        private void discountWelfare(int sum)
        {
            switch(welfare)
            {
                case "해당없음":
                    welfareDiscountAmount = 0;
                    discountFamily(sum - guarantee);
                    tariff = charge - guarantee - familyDiscountAmount;
                    break;
                case "기초생활(생계, 의료)":
                    {
                        if (sum < 16000)
                            welfareDiscountAmount = sum;
                        else
                            welfareDiscountAmount = 16000;

                        discountFamily(sum - welfareDiscountAmount);
                        tariff = charge - welfareDiscountAmount - familyDiscountAmount;
                        break;
                    }
                case "기초생활(주거, 교육)":
                    {
                        if (sum < 10000)
                            welfareDiscountAmount = sum;
                        else
                            welfareDiscountAmount = 10000;

                        discountFamily(sum - welfareDiscountAmount);
                        tariff = charge - welfareDiscountAmount - familyDiscountAmount;
                        break;
                    }
                case "차상위계층":
                    {
                        if (sum < 8000)
                            welfareDiscountAmount = sum;
                        else
                            welfareDiscountAmount = 8000;

                        discountFamily(sum - welfareDiscountAmount);
                        tariff = charge - welfareDiscountAmount - familyDiscountAmount;
                        break;
                    }
                default:
                    {
                        if (sum < 16000)
                            welfareDiscountAmount = sum;
                        else
                            welfareDiscountAmount = 16000;

                        discountFamily(sum);
                    
                        if (welfareDiscountAmount >= familyDiscountAmount)
                            tariff = charge - welfareDiscountAmount;
                        else
                            tariff = charge - familyDiscountAmount;
                        break;
                    }
            }
        }

        private void discountFamily(int sum)
        {
            switch(family)
            {
                case "해당없음":
                    familyDiscountAmount = 0;
                    break;
                case "생명유지장치":
                    {
                        familyDiscountAmount = (int)(sum * 0.3);
                        break;
                    }
                default:
                    {
                        familyDiscountAmount = (int)(sum * 0.3);
                        if (familyDiscountAmount > 16000)
                            familyDiscountAmount = 16000;
                        break;
                    }
            }
        }

        private void calOther1(ContractStandard standard)
        {
            basicCharge = standard.getBasic_charge(0) * contractPower;
            charge = (int)(standard.charge.getBasic(0) * usagePower);
            calVatAndPowFund();
        }

        private void calOther2(ContractStandard standard)
        {
            basicCharge = standard.getBasic_charge(0) * contractPower;
            charge = (int)(standard.charge.getLightload(0) * usagePower);
            calVatAndPowFund();
        }

        private void calVatAndPowFund()
        {
            vat = (int)(tariff * 0.1);
            powerFund = (int)(tariff * 0.037);
        }

    }
}
