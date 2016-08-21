using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Context.Store
{
    public class InMemoryStoreStrategy : IStoreStrategy
    {
        private Object thisLock = new Object();
        public static Dictionary<string, string> data = new Dictionary<string, string>();

        public object Load(string contents, Type type)
        {
            lock (thisLock)
            {
                if (data.Keys.Contains(type.Name))
                {
                    contents = data[type.Name];
                }
                return JsonHelper.DeserializeObject(contents, type);
                //return XmlHelper.DeserializeObject(contents, type);
            }
        }

        public string Save(object o, Type type)
        {
            lock (thisLock)
            {
                //var contents = XmlHelper.SerializeObject(o, type);
                var contents = JsonHelper.SerializeObject(o, type);
                if (data.Keys.Contains(type.Name))
                {
                    data.Remove(type.Name);
                }
                data.Add(type.Name, contents);
                return contents;
            }
        }

        public string GetFileName(Type T, string prefix = null)
        {
            return null;
        }

        public string ToDelete(object o, Type type)
        {
            return string.Empty;
        }

        public string ToUpdate(object o, Type type)
        {
            return string.Empty;
        }

        public string PreLoad(Type type, string prefix = null)
        {
            return string.Empty;
        }
    }
}
