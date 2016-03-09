using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DevMentor.Context.Store
{
    public class XmlStoreStrategy : IStoreStrategy
    {

        private Object thisLock = new Object();

        public string ToDelete(object o, Type type)
        {
            return string.Empty;
        }

        public string ToUpdate(object o, Type type)
        {
            return string.Empty;
        }

        public string PreLoad(Type type)
        {
            lock (thisLock)
            {
                var fileName = this.GetFileName(type);
                if (File.Exists(fileName))
                    return File.ReadAllText(fileName);
                return string.Empty;
            }
        }

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
            var name = T.Name;
            string dir = AppDomain.CurrentDomain.BaseDirectory; //HttpContext.Current.Server.MapPath("~");//System.Reflection.Assembly.GetExecutingAssembly().Location;

            dir = Path.Combine(dir, @"App_Data\");

            Directory.CreateDirectory(dir);

            return Path.Combine(dir, name + ".xml");
        }
    }
}
