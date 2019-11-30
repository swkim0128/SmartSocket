using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSocketData
{
    class SocketData
    {
        private const int COMMAND_SIZE = 4;
        /*
        //public SocketCommand.Command command { get; set; }
        public string stringData { get; set; }

        private byte[] data;
        private int dataLength;
        public byte[] Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
                this.dataLength = data.Length;
            }
        }
        public int Length
        {
            get
            {
                return this.dataLength;
            }
            set
            {
                this.dataLength = value;
                this.data = new byte[this.dataLength];
            }
        }

        public SocketData()
        {
            //this.command = SocketCommand.Command.None;
            this.stringData = "";
        }

        public SocketData(SocketCommand.Command typeCommand, string stringData)
        {
            this.command = typeCommand;
            this.stringData = stringData;
        }

        public void createByteData()
        {
            byte[] commandByte = convertByteToCommand();
            byte[] stringByte = convertByteToString();

            data = new byte[COMMAND_SIZE + stringByte.Length];

            Buffer.BlockCopy(commandByte, 0, data, 0, COMMAND_SIZE);
            Buffer.BlockCopy(stringByte, 0, data, COMMAND_SIZE, stringByte.Length);
        }

        private byte[] convertByteToString()
        {
            return Encoding.UTF8.GetBytes(this.stringData);
        }

        private byte[] convertByteToCommand()
        {
            return Encoding.UTF8.GetBytes(string.Format("{0:D4}", this.command.GetHashCode()));
        }

        public void createCmdAndStrData()
        {
            byte[] commandTemp = new byte[COMMAND_SIZE];

            Buffer.BlockCopy(data, 0, commandTemp, 0, COMMAND_SIZE);
            this.command = (SocketCommand.Command)convertIntToByte(commandTemp);

            byte[] stringTemp = new byte[data.Length - COMMAND_SIZE];
            Buffer.BlockCopy(data, COMMAND_SIZE, stringTemp, 0, data.Length - COMMAND_SIZE);

            this.stringData = convertStringToByte(stringTemp);
        }

        private int convertIntToByte(byte[] data)
        {
            string str = Encoding.UTF8.GetString(data);
            return Convert.ToInt32(str);
        }

        private string convertStringToByte(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
        */
    }
}
