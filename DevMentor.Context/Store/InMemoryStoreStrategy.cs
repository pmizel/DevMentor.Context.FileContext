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

        public object Load(string contents, Type type)
        {
            lock (thisLock)
            {
                return XmlHelper.DeserializeObject(contents, type);
            }
        }

        public string Save(object o, Type type)
        {
            lock (thisLock)
            {
                return XmlHelper.SerializeObject(o, type);
            }
        }

        public string GetFileName(Type T)
        {
            return null;
        }
    }
}
