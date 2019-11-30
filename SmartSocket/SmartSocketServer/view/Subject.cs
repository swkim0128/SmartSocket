using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSocketServer.view
{
    interface Subject
    {
        void registerObserver(Observer o);
        void remove(Observer o);
        void notifyObserver();
    }
}
