using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartSocketMongoDB;
using SmartSocketMongoDB.Model;
using SmartSocketData;
using SmartSocketServer.view;

namespace SmartSocketServer.Command
{
    class ElectPowerSaveCmd : Command
    {
        public ElectPowerSaveCmd()
            : base(null)
        {

        }

        public ElectPowerSaveCmd(SubjectModel subject)
            : base(subject)
        {

        }

        public override void execute(MainSession session, SocketJsonData requestInfo)
        {
            string measureId = requestInfo.getJsonKeyValue("measureProduct_id");
            string power = requestInfo.getJsonKeyValue("power");

            MeasureProductRepository measureProductRepository = new MeasureProductRepository();

            if (measureProductRepository.Find("_id", measureId).Result == null)
            {
                MeasureProduct product = new MeasureProduct();
                product.id = measureId;
                measureProductRepository.Insert(product).Wait();
            }
            else
            {
                DayPowerRepository dayPowerRepository = new DayPowerRepository();
                DateTime date = DateTime.Today;

                DayPower dayPower = dayPowerRepository.Find(date, measureId);

                if (dayPower == null)
                {
                    DayPower day = new DayPower();
                    day.id.measureProduct_id = measureId;
                    day.id.date = date;
                    dayPowerRepository.Insert(day).Wait();
                    dayPowerRepository.Add(day.id, "power", power).Wait();
                }
                else
                {
                    DayPowerID id = new DayPowerID();
                    id.date = date;
                    id.measureProduct_id = measureId;

                    double usagePower = dayPower.usagePower;
                    usagePower += Convert.ToDouble(power);
                    dayPowerRepository.Add(id, "power", power).Wait();
                    dayPowerRepository.Update(id, "usagePower", Convert.ToString(usagePower)).Wait();
                }
            }
        }

        public override void execute(SocketJsonData requestInfo)
        {

        }
    }
}
