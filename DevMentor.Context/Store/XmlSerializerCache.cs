using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DevMentor.Context.Store
{
    public class XmlSerializerCache
    {

        static Dictionary<string, XmlSerializer> XmlSerializerList = new Dictionary<string, XmlSerializer>();
        static object lockObject = new object();

        public XmlSerializer this[Type type]
        {
            get
            {
                lock (lockObject)
                {
                    XmlSerializer result = null;
                    var key = type.GenericTypeArguments[0].Name;
                    if (XmlSerializerList.Keys.Contains(key))
                    {
                        result = XmlSerializerList[key];
                    }
                    else
                    {
                        lock(lockObject)
                        {
                            result = new XmlSerializer(type);
                            if (!XmlSerializerList.Keys.Contains(key))
                            {
                                XmlSerializerList.Add(key, result);
                            }
                        }
                    }
                    return result;
                }
            }
        }
    }

}
