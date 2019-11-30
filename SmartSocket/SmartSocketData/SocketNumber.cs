using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSocketData
{
    class SocketNumber
    {
        public bool isNumber(string value)
        {
            if((value == null)
                || (string.IsNullOrEmpty(value))
                || (value == ""))
            {
                return false;
            }

            int index = 0;

            foreach(char cData in value)
            {
                if(Char.IsNumber(cData) == false)
                {
                    if((index == 0) && (cData != '-'))
                        return false;
                }

                ++index;
            }

            return true;
        }

        public int StringToInt(string data)
        {
            int result = 0;

            if (this.isNumber(data) == false)
                result = 0;
            else
                result = Convert.ToInt32(data);

            return result;
        }
    }
}
