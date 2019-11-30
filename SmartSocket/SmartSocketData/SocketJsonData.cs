using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SmartSocketData
{
    public class SocketJsonData
    {
        private JObject jObject;
        
        public SocketJsonData()
        {
            jObject = new JObject();
        }

        public SocketJsonData(string str)
        {
            jObject = JObject.Parse(str);
        }

        public void setJObj(string str)
        {
            jObject = JObject.Parse(str);
        }

        public void setJObj(object obj)
        {
            jObject = JObject.FromObject(obj);
        }

        public string getJsonKeyValue(string key)
        {
            return jObject[key].ToString();
        }

        public string[] getJsonArrayValues(string key)
        {
            JArray array = (JArray)jObject[key];
            string[] values = new string[array.Count];
            for (int index = 0; index < array.Count; index++)
                values[index] = array[index].ToString();

            return values;
        }

        public bool getJsonBoolValue(string key)
        {
            return Convert.ToBoolean(jObject[key]);
        }

        public void addElement(string key, string value)
        {
            jObject.Add(key, value);
        }
        
        public void addElement(string key, string[] values)
        {
            JArray array = new JArray();

            foreach (string value in values)
                array.Add(value);

            jObject.Add(key, array);
        }

        public void addElement(string key, int value)
        {
            jObject.Add(key, value);
        }

        public void addElement(string key, bool value)
        {
            jObject.Add(key, value);
        }

        public void addElement(string key, object obj)
        {
            var jsonObj = JObject.FromObject(obj);
            jObject.Add(key, jsonObj);
        }

        public void reset()
        {
            jObject.RemoveAll();
        }

        public string getJObject()
        {
            return jObject.ToString();
        }

        public static string convertString(object o)
        {
            string jsonString = JsonConvert.SerializeObject(o);
            return jsonString;
        }

        public static object convertObject(string jsonString)
        {
            object obj = JsonConvert.DeserializeObject<object>(jsonString);
            return obj;
        }
    }
}
