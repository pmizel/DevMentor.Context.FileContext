using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Context.Store
{
    public class CacheStorageStrategy : IStoreStrategy
    {
        IStoreStrategy baseStore;
        static ConcurrentDictionary<string, object> _concurrent = new ConcurrentDictionary<string, object>();


        public CacheStorageStrategy(IStoreStrategy baseStore)
        {
            this.baseStore = baseStore;

        }
        public string GetFileName(Type type)
        {
            return baseStore.GetFileName(type);
        }

        public string PreLoad(Type type)
        {
            if (_concurrent.Keys.Contains(type.Name))
            {
                return string.Empty;
            }
            return baseStore.PreLoad(type);
        }

        public object Load(string contents, Type type)
        {
            var gtype = type.GenericTypeArguments.FirstOrDefault();
            if (_concurrent.Keys.Contains(gtype.Name))
            {
                object obj = null;
                _concurrent.TryGetValue(gtype.Name, out obj);
                return obj;
            }
            return baseStore.Load(contents, type);
        }

        public string Save(object o, Type type)
        {
            var gtype = type.GenericTypeArguments.FirstOrDefault();
            _concurrent.AddOrUpdate(gtype.Name, o, (key, oldvalue) => oldvalue);

            return baseStore.Save(o, type);
        }

        public string ToDelete(object o, Type type)
        {
            return baseStore.ToDelete(o, type);
        }

        public string ToUpdate(object o, Type type)
        {
            return baseStore.ToUpdate(o, type);
        }
    }
}
