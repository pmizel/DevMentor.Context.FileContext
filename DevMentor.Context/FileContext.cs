using DevMentor.Context.FileManager;
using DevMentor.Context.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;

namespace DevMentor.Context
{
    public class FileContext : IDisposable
    {
        IStoreStrategy store;

        public FileContext()
        {
            Init();
        }

        public FileContext(string connectionname)
        {
            Init();
        }

        public FileContext(string connectionname, IStoreStrategy store)
        {
            this.store = store;
            Init();
        }

        public FileContext(IStoreStrategy store)
        {
            this.store = store;
            Init();
        }

        private void Init()
        {
            if (this.store == null)
            {
                this.store = new DefaultStoreStrategy();
            }

            //Assembly asm= Assembly.GetExecutingAssembly();
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                if (propType.GenericTypeArguments.Length == 0)
                    continue;
                var gen_type = propType.GenericTypeArguments[0];
                var ret_type = gen_type.GetType();
                if (ret_type.IsInstanceOfType(typeof(IFileSet)))
                {
                    bool isInit = false;
                    //load
                    string filename = store.GetFileName(gen_type);
                    if (File.Exists(filename))
                    {
                        //string contents = File.ReadAllText(filename);
                        string contents = store.PreLoad(gen_type);
                        //object o = XmlHelper.DeserializeObject(contents, prop.PropertyType);
                        object o = store.Load(contents, prop.PropertyType);
                        if (o != null)
                        {
                            prop.SetValue(this, o);
                            isInit = true;
                        }
                        //XmlHelper.SerializeObject(o, prop.PropertyType);
                    }
                    else if (string.IsNullOrEmpty(filename))
                    {
                        object o = store.Load(string.Empty, prop.PropertyType);
                        if (o != null)
                        {
                            var list = o as IEnumerable;
                            if (list != null)
                            {
                                var gtype = prop.PropertyType.GenericTypeArguments.FirstOrDefault();
                                var instance = Activator.CreateInstance(prop.PropertyType);
                                foreach (var item in list)
                                {

                                    var method = item
                                    .GetType()
                                    .GetMethod("MakeDirty");

                                    if (method != null)
                                        method.Invoke(item, new object[] { false });

                                    prop.PropertyType
                                        .GetMethod("Add")
                                        .Invoke(instance, new object[] { item });
                                }
                                prop.SetValue(this, instance);
                                isInit = true;
                            }
                            o = null;
                        }
                        if (o != null)
                        {
                            prop.SetValue(this, o);
                            isInit = true;
                        }
                    }

                    if (!isInit)
                    {
                        //if empty = initial
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
            //TODO: //Update marker

            var type = entity.GetType();
            Type fileSetType = null;
            IFileSet o = null;
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                fileSetType = prop.PropertyType;
                var propType = prop.PropertyType;
                if (propType.GenericTypeArguments.Length == 0)
                    continue;
                var gen_type = propType.GenericTypeArguments[0];
                var ret_type = gen_type.GetType();
                if (gen_type == type)
                {
                    o = prop.GetValue(this) as IFileSet;
                    break;
                }
            }
            if (o != null)
            {
                var name = type.Name;
                var idPropInfo = type.GetProperty(type.Name + "Id");
                if (idPropInfo == null)
                    idPropInfo = type.GetProperty("Id");
                if (idPropInfo == null)
                    throw new ArgumentException("Type " + name + " don't have any Id name");
                var id = idPropInfo.GetValue(entity);
                object itemFromSet = null;
                foreach (var item in o)
                {
                    var idcmp = idPropInfo.GetValue(item);
                    if (Comparer.Default.Compare(idcmp, id) == 0)
                    {
                        itemFromSet = item;
                        break;
                    }
                }
                //if (itemFromSet != null)
                //{
                //    fileSetType.GetMethod("Remove").Invoke(o, new object[] { itemFromSet });
                //    fileSetType.GetMethod("Add").Invoke(o, new object[] { entity });
                //}
            }

            return _fileEntityEntry;
        }


        public FileSet<T> Set<T>()
            where T : class
        {
            object o = null;
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                if (propType.GenericTypeArguments.Length == 0)
                    continue;
                var gen_type = propType.GenericTypeArguments[0];
                var ret_type = gen_type.GetType();
                if (gen_type == typeof(T))
                {
                    o = prop.GetValue(this);
                    break;
                }
            }
            return (FileSet<T>)o;
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
                var propType = prop.PropertyType;
                if (propType.GenericTypeArguments.Length == 0)
                    continue;
                var gen_type = propType.GenericTypeArguments[0];
                var ret_type = gen_type.GetType();
                if (ret_type.IsInstanceOfType(typeof(IFileSet)))
                {
                    string filename = store.GetFileName(gen_type);
                    if (!string.IsNullOrEmpty(filename))
                    {
                        object o = prop.GetValue(this);
                        //string contents = XmlHelper.SerializeObject(o, prop.PropertyType);
                        string contents = store.Save(o, prop.PropertyType);
                        TxFileManager fileMgr = new TxFileManager();
                        using (TransactionScope scope1 = new TransactionScope())
                        {
                            fileMgr.WriteAllText(filename, contents);
                            scope1.Complete();
                        }
                    }
                    else if (string.IsNullOrEmpty(filename))
                    {
                        object o = prop.GetValue(this);
                        store.Save(o, prop.PropertyType);

                        var deletedList=o.GetType().GetProperty("LocalDeleted").GetValue(o);
                        store.ToDelete(deletedList, prop.PropertyType);

                        var updatedList = o.GetType().GetProperty("LocalUpdated").GetValue(o);
                        store.ToUpdate(updatedList, prop.PropertyType);
                    }
                }
            }

            return 0;
        }

        //public virtual int LoadChanges()
        //{
        //    return SaveChanges();
        //}


    }
}
