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
                    
        static ConcurrentDictionary<string, XmlSerializer> XmlSerializerList = new ConcurrentDictionary<string, XmlSerializer>();

        public XmlSerializer this[Type type]
        {
            get
            {
                XmlSerializer result = null;
                var key = type.GenericTypeArguments[0].Name;
                if (XmlSerializerList.Keys.Contains(key))
                {
                    result = XmlSerializerList[key];
                }
                else
                {
                    result = new XmlSerializer(type); 
                    XmlSerializerList.Add(key, result);
                }
                return result;
            }
        }


    }
}
