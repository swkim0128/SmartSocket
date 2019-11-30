using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using SmartSocketServer.Command;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine;

using SmartSocketData;
using SmartSocketServer.view;
using SuperSocket.SocketBase.Config;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmartSocketServer
{
    public partial class Form1 : Form, Observer
    {
        Thread thread;

        private Invoker invoker;
        private SubjectModel model;
        private SocketJsonData jsonData;

        public Form1()
        {
            InitializeComponent();
            initSettingPanel();
            initCurrentChart();

            thread = new Thread(new ThreadStart(serverInit));
            thread.Start();

            model = new SubjectModel();
            model.registerObserver(this);

            invoker = new Invoker(model);

            jsonData = new SocketJsonData();
        }

        private void initSettingPanel()
        {
            ContractComboBox.SelectedIndex = 0;
            ContractTypeComboBox.SelectedIndex = 0;
            WelfareComboBox.SelectedIndex = 0;
            ReceivingComboBox.SelectedIndex = 0;
        }

        private void initCurrentChart()
        {
            CurrentPowerChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "HH:mm:ss";
            CurrentPowerChart.Series["Series1"].XValueType = ChartValueType.DateTime;
        }

        private void serverInit()
        {
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
        }

        public void update(string requestInfo)
        {
            SocketJsonData jData = new SocketJsonData();

            string[] data = requestInfo.Split(';');
            int command = Convert.ToInt32(data[0]);
            jData.setJObj(data[1]);

            switch(command)
            {
                case (int)SocketCommand.ElectPowerInquiry:
                    {
                        CurrentElectricPowerLabel.Text = jData.getJsonKeyValue("usagePower") + "kWh";
                        CurrentPowerLabel.Text = jData.getJsonKeyValue("usagePower") + "kWh";
                        StandbyPowerLabel.Text = jData.getJsonKeyValue("standbyPower") + "kWh";
                        GradualAdvanceLabel.Text = jData.getJsonKeyValue("progressiveTax") + "단계";

                        DateTime contractDate = DateTime.Parse(jData.getJsonKeyValue("startDate"));
                        DateTime endDate = DateTime.Parse(jData.getJsonKeyValue("endDate"));

                        string duringDate = Convert.ToString(contractDate.Year) + "." + Convert.ToString(contractDate.Month) +
                            Convert.ToString(contractDate.Day) + " ~ " + Convert.ToString(endDate.Year) + "." +
                            Convert.ToString(contractDate.Month) + "." + Convert.ToString(endDate.Day);
                        label6.Text = duringDate;
                        label8.Text = duringDate;

                        string[] powerValues = jData.getJsonArrayValues("currentPower");
                        List<DateTime> timeList = new List<DateTime>();
                        for (int i = 0; i < 1440; i++)
                        {
                            timeList.Add(DateTime.Today.AddMinutes(i));
                        }
                        for(int i = 0; i < powerValues.Length; i++)
                        {
                            CurrentPowerChart.Series[0].Points.AddXY(timeList[i], powerValues[i]);
                        }
                        break;
                    }
                case (int)SocketCommand.ElectChargeInquiry:
                    {
                        CurrentPowerChargeLabel.Text = jData.getJsonKeyValue("allElectCharge") + "원";
                        BasicChargeLabel.Text = jData.getJsonKeyValue("basicCharge") + "원";
                        ElectricQuantityChargeLabel.Text = jData.getJsonKeyValue("tariff") + "원";
                        DiscountLabel.Text = jData.getJsonKeyValue("discount") + "원";
                        ValueAddedTaxLabel.Text = jData.getJsonKeyValue("vat") + "원";
                        PowerFundLabel.Text = jData.getJsonKeyValue("powerFund") + "원";
                        break;
                    }
                case (int)SocketCommand.TaxStandardSave:
                    {
                        bool result = jData.getJsonBoolValue("result");
                        if (result)
                            MessageBox.Show("저장 완료");
                        else
                            MessageBox.Show("저장 실패");
                        break;
                    }
                case (int)SocketCommand.TaxStandardInquiry:
                    {
                        if(jData.getJsonBoolValue("result"))
                        {
                            string contractDate = jData.getJsonKeyValue("contractDate");
                            string contract = jData.getJsonKeyValue("contract");
                            string family = jData.getJsonKeyValue("family");
                            string welfare = jData.getJsonKeyValue("welfare");
                            string contractPower = jData.getJsonKeyValue("contractPower");
                            string recevingVoltage = jData.getJsonKeyValue("receivingVoltage");

                            ContractComboBox.SelectedIndex = ContractComboBox.Items.IndexOf(contractDate);
                            ContractTypeComboBox.SelectedIndex = ContractTypeComboBox.Items.IndexOf(contract);
                            checkedRadioBtn(family);
                            WelfareComboBox.SelectedIndex = WelfareComboBox.Items.IndexOf(welfare);
                            ContractTextBox.Text = contractPower;
                            if (recevingVoltage == "해당없음")
                                ReceivingComboBox.SelectedIndex = 0;
                            else
                                ReceivingComboBox.SelectedIndex = ReceivingComboBox.Items.IndexOf(recevingVoltage);
                        }
                        break;
                    }
            }
        }

        private void checkedRadioBtn(string family)
        {
            FiveFamilyRadioBtn.Checked = false;
            ChildRadioBtn.Checked = false;
            LifeRadioBtn.Checked = false;
            BirthRadioBtn.Checked = false;
            NotRadioBtn.Checked = false;

            switch(family)
            {
                case "5인이상 가구":
                    FiveFamilyRadioBtn.Checked = true;
                    break;
                case "3자녀이상 가구":
                    ChildRadioBtn.Checked = true;
                    break;
                case "생명유지 장치":
                    LifeRadioBtn.Checked = true;
                    break;
                case "출산 가구":
                    BirthRadioBtn.Checked = true;
                    break;
                case "해당없음":
                    NotRadioBtn.Checked = true;
                    break;
            }
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            jsonData.reset();
            jsonData.addElement("measureProduct_id", "01");
            jsonData.addElement("contractDate", ContractComboBox.SelectedItem.ToString());

            invoker.executeCmd((int)SocketCommand.ElectPowerInquiry, jsonData);
            setPanelVisible(0);
        }

        private void ElectricChargeBtn_Click(object sender, EventArgs e)
        {
            jsonData.reset();
            jsonData.addElement("_id", "sw");
            jsonData.addElement("measureProduct_id", "01");

            invoker.executeCmd((int)SocketCommand.ElectChargeInquiry, jsonData);
            setPanelVisible(1);
        }

        private void AnalysisBtn_Click(object sender, EventArgs e)
        {
            jsonData.reset();

            int command = Convert.ToInt32(0);
            invoker.executeCmd(command, jsonData);
            setPanelVisible(2);
        }

        private void SettingBtn_Click(object sender, EventArgs e)
        {
            jsonData.reset();
            jsonData.addElement("_id", "sw");
            invoker.executeCmd((int)SocketCommand.TaxStandardInquiry, jsonData);
            setPanelVisible(3);
        }
        
        private void setPanelVisible(int n)
        {
            HomePanel.Visible = false;
            ElectricChargePanel.Visible = false;
            ConsumePatternPanel.Visible = false;
            SettingPanel.Visible = false;

            switch (n)
            {
                case 0:
                    HomePanel.Visible = true;
                    break;
                case 1:
                    ElectricChargePanel.Visible = true;
                    break;
                case 2:
                    ConsumePatternPanel.Visible = true;
                    break;
                case 3:
                    SettingPanel.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void ContractSaveBtn_Click(object sender, EventArgs e)
        {
            jsonData.reset();
            jsonData.addElement("_id", "sw");
            jsonData.addElement("contractDate", ContractComboBox.SelectedItem.ToString());
            jsonData.addElement("contract", ContractTypeComboBox.SelectedItem.ToString());
            jsonData.addElement("family", checkRadioBtn());
            jsonData.addElement("welfare", WelfareComboBox.SelectedItem.ToString());
            if(ContractTextBox.Text == "")
                jsonData.addElement("contractPower", 0);
            else
                jsonData.addElement("contractPower", ContractTextBox.Text);
            if(ReceivingComboBox.SelectedItem.ToString() == "-전력구분선택-")
                jsonData.addElement("receivingVoltage", "해당없음");
            else
                jsonData.addElement("receivingVoltage", ReceivingComboBox.SelectedItem.ToString());
            jsonData.addElement("measureProduct_id", "01");

            int command = Convert.ToInt32(SocketCommand.TaxStandardSave);
            invoker.executeCmd(command, jsonData);
        }

        private string checkRadioBtn()
        {
            string familyStr = "";

            if (FiveFamilyRadioBtn.Checked)
                familyStr = "5인이상 가구";
            else if (BirthRadioBtn.Checked)
                familyStr = "출산 가구";
            else if (ChildRadioBtn.Checked)
                familyStr = "3자녀이상 가구";
            else if (LifeRadioBtn.Checked)
                familyStr = "생명유지장치";
            else if (NotRadioBtn.Checked)
                familyStr = "해당없음";

            return familyStr;
        }

        private void ReceivingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void HomePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ConsumePatternPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ElectricChargePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SettingPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ContractTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(ContractTypeComboBox.SelectedIndex)
            {
                case 0:
                case 1:
                    {
                        FamilyPanel.Enabled = true;
                        WelfareComboBox.Enabled = true;
                        ContractTextBox.Enabled = false;
                        ReceivingComboBox.Enabled = false;
                        break;
                    }
                default:
                    {
                        FamilyPanel.Enabled = false;
                        WelfareComboBox.Enabled = false;
                        ContractTextBox.Enabled = true;
                        ReceivingComboBox.Enabled = true;
                        break;
                    }
            }
        }
    }
}
