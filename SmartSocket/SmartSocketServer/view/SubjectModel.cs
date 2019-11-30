using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSocketServer.view
{
    class SubjectModel : Subject
    {
        private Observer observer;
        private string requestInfo;

        public void registerObserver(Observer o)
        {
            observer = o;
        }

        public void remove(Observer o)
        {
            observer = null;
        }

        public void notifyObserver()
        {
            observer.update(requestInfo);
        }

        public void setModel(string requestInfo)
        {
            this.requestInfo = requestInfo;
            changedModel();
        }

        private void changedModel()
        {
            notifyObserver();
        }
    }
}
