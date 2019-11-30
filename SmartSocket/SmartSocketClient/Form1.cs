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

using SuperSocket.ClientEngine;

using SmartSocketData;

namespace SmartSocketClient
{
    public partial class Form1 : Form
    {
        private AsyncTcpSession clientSession;

        public Form1()
        {
            InitializeComponent();

            IPEndPoint ipServer = 
                new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2020);
            EndPoint epTemp = ipServer as EndPoint;

            clientSession = new AsyncTcpSession();
            clientSession.Connected += clientSessionConnected;
            clientSession.Closed += clientSessionClosed;
            clientSession.DataReceived += clientSessionDataReceived;
            clientSession.Error += clientSessionError;

            clientSession.Connect(epTemp);
        }

        private void clientSessionConnected(object sender, EventArgs e)
        {
            if(clientSession.IsConnected)
            {
                Console.WriteLine("Connected...");
            }
            else
            {
                disconnect();
            }
        }

        private void clientSessionClosed(object sender, EventArgs e)
        {
            disconnect();
        }

        private void clientSessionDataReceived(object sender, DataEventArgs e)
        {
            if(clientSession.IsConnected)
            {
                string data = Encoding.UTF8.GetString(e.Data);
                Console.WriteLine(data);
            }
            else
            {
                disconnect();
            }
        }

        private void clientSessionError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void disconnect()
        {
            clientSession.Close();
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            SocketJsonData jsonData = new SocketJsonData();
            jsonData.addElement("measureProduct_id", "01");
            sendData(SocketCommand.ElectPowerInquiry, jsonData.getJObject());
            setPanelVisible(0);
        }

        private void ElectricChargeBtn_Click(object sender, EventArgs e)
        {
            setPanelVisible(1);
        }

        private void AnalysisBtn_Click(object sender, EventArgs e)
        {
            setPanelVisible(2);
        }

        private void SettingBtn_Click(object sender, EventArgs e)
        {
            setPanelVisible(3);
        }
        
        private void sendData(SocketCommand socketCommand, string requestInfo)
        {
            if(clientSession.IsConnected)
            {
                string data = Convert.ToInt32(socketCommand) + ":" + requestInfo;
                byte[] byteData = Encoding.UTF8.GetBytes(data);
                clientSession.Send(byteData, 0, byteData.Length);
            }
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
    }
}
