using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DevMentor.Context
{
    public class FileContext : IDisposable
    {


        public FileContext(string connectionname)
            : this()
        {

        }
        public FileContext()
        {
            //Assembly asm= Assembly.GetExecutingAssembly();
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                var gen_type = (prop.PropertyType).GenericTypeArguments[0];
                var ret_type = gen_type.GetType();
                if (ret_type.IsInstanceOfType(typeof(IFileSet)))
                {
                    bool isInit = false;
                    //lade
                    string filename = GetFileName(gen_type);
                    if (File.Exists(filename))
                    {
                        string contents = File.ReadAllText(filename);
                        object o = XmlHelper.DeserializeObject(contents, prop.PropertyType);
                        if (o != null)
                        {
                            prop.SetValue(this, o);
                            isInit = true;
                        }
                        //XmlHelper.SerializeObject(o, prop.PropertyType);
                    }

                    if (!isInit)
                    {
                        //falls leer = initial
                        var instance = Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(this, instance);
                    }
                }
                //Name: prop.Name
            }
        }

        FileEntityEntry _fileEntityEntry;
        public FileEntityEntry Entry(object entity)
        {
            if (_fileEntityEntry == null)
                _fileEntityEntry = new FileEntityEntry();
            return _fileEntityEntry;
        }


        public void Dispose()
        {
        }


        public virtual int SaveChanges()
        {
            //Assembly asm= Assembly.GetExecutingAssembly();
            var props = this.GetType().GetProperties();//.All(p => p.MemberType.GetType() == typeof(FileSet<Object>));
            foreach (var prop in props)
            {
                var gen_type = (prop.PropertyType).GenericTypeArguments[0];
                var ret_type = gen_type.GetType();
                if (ret_type.IsInstanceOfType(typeof(IFileSet)))
                {
                        string filename = GetFileName(gen_type);
                        object o = prop.GetValue(this);
                        string contents = XmlHelper.SerializeObject(o, prop.PropertyType);
                        File.WriteAllText(filename, contents);
                }
            }

            return 0;
        }

        public virtual int LoadChanges()
        {
            //Assembly asm= Assembly.GetExecutingAssembly();
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                var gen_type = (prop.PropertyType).GenericTypeArguments[0];
                var ret_type = gen_type.GetType();
                if (ret_type.IsInstanceOfType(typeof(IFileSet)))
                    if (ret_type.IsInstanceOfType(typeof(IFileSet)))
                    {
                        string filename = GetFileName(gen_type);
                        object o = prop.GetValue(this);
                        string contents = XmlHelper.SerializeObject(o, prop.PropertyType);
                        File.WriteAllText(filename, contents);
                    }
            }

            return 0;
        }

        string GetFileName(Type T)
        {
            var name = T.Name;
            string dir = HttpContext.Current.Server.MapPath("~");//System.Reflection.Assembly.GetExecutingAssembly().Location;

            if (System.Web.HttpContext.Current != null)
            {
                dir = Path.Combine(dir, @"App_Data\");
            }
            return Path.Combine(dir, name + ".xml");
        }

    }
}
