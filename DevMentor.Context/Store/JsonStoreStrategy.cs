using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DevMentor.Context.Store
{
    public class JsonStoreStrategy:IStoreStrategy
    {
        public object Load(string contents, Type type)
        {
            return JsonHelper.DeserializeObject(contents, type);
        }

        public string Save(object o, Type type)
        {
            return JsonHelper.SerializeObject(o, type);
        }

        public string GetFileName(Type T)
        {
            var name = T.Name;
            string dir = HttpContext.Current.Server.MapPath("~");//System.Reflection.Assembly.GetExecutingAssembly().Location;

            if (System.Web.HttpContext.Current != null)
            {
                dir = Path.Combine(dir, @"App_Data\");
            }
            return Path.Combine(dir, name + ".json");
        }
    }
}
