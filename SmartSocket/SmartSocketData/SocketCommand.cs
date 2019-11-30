using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSocketData
{
    public enum SocketCommand
    {
        None = 0,
        ElectPowerSave,
        ElectPowerInquiry,
        ElectChargeInquiry,
        TaxStandardSave,
        TaxStandardInquiry,
        ElectPowerNoHave
    }
}
