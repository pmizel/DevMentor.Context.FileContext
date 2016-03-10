using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DevMentor.Context.Store
{
    public class XmlSerializerCache
    {
        static Dictionary<string, XmlSerializer> XmlSerializerList = new Dictionary<string, XmlSerializer>();

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
