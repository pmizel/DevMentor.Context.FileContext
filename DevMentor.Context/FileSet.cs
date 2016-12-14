using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DevMentor.Context
{

    public class FileSet<TEntity> :
        System.Data.Entity.IDbSet<TEntity>,
        IQueryable<TEntity>, IEnumerable<TEntity>, IFileSet where TEntity : class
    {

        public FileSet()
        {
            Local = new ObservableCollection<TEntity>();
            LocalDeleted = new ObservableCollection<TEntity>();
            LocalUpdated = new ObservableCollection<TEntity>();

        }


        //[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Intentionally just implicit to reduce API clutter.")]
        //public static implicit operator FileSet(FileSet<TEntity> entry);

        // Summary:
        //     Gets an System.Collections.ObjectModel.ObservableCollection<T> that represents
        //     a local view of all Added, Unchanged, and Modified entities in this set.
        //     This local view will stay in sync as entities are added or removed from the
        //     context. Likewise, entities added to or removed from the local view will
        //     automatically be added to or removed from the context.
        //
        // Remarks:
        //     This property can be used for data binding by populating the set with data,
        //     for example by using the Load extension method, and then binding to the local
        //     data through this property. For WPF bind to this property directly. For Windows
        //     Forms bind to the result of calling ToBindingList on this property
        public ObservableCollection<TEntity> Local { get; private set; }
        public ObservableCollection<TEntity> LocalDeleted { get; private set; }
        public ObservableCollection<TEntity> LocalUpdated { get; private set; }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Local.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Local.GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(TEntity); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return Local.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return Local.AsQueryable().Provider; }
        }

        public TEntity Find(params object[] ids)
        {
            if (ids.Length != 1)
                throw new ArgumentException("FileSet support only one id as parameter");

            object id = ids[0]; //looks like error ;-)

            var type = typeof(TEntity);
            var name = type.Name;
            var idPropInfo = type.GetProperty(type.Name + "Id");
            if (idPropInfo == null)
                idPropInfo = type.GetProperty("Id");
            if (idPropInfo == null)
                throw new ArgumentException("Type " + name + " don't have any Id name");

            return Local.FirstOrDefault(i => Comparer.Default.Compare(idPropInfo.GetValue(i), id) == 0);
        }


        public TEntity Add(TEntity item)
        {
            Local.Add(item);
            return item;
        }

        //public object Remove(object item)
        //{
        //    return Remove(item as TEntity);
        //}

        public TEntity Remove(TEntity item)
        {
            Local.Remove(item);
            if (!LocalDeleted.Contains(item))
            {
                LocalDeleted.Add(item);
            }
            return item;
        }

        public TEntity Attach(TEntity item)
        {
            var method = item
            .GetType()
            .GetMethod("MakeDirty");

            if (method != null)
                method.Invoke(item, new object[] { true });

            LocalUpdated.Add(item);

            object id = GetId(item);

            var firstOrDefault = Local.FirstOrDefault(i => Comparer.Default.Compare(GetId(i), id) == 0);
            if (firstOrDefault != null)
            { //map => update
                CopyObjectData(item, firstOrDefault);
            }
            else {
                //insert
                Local.Add(item);
            }


            return item;
        }

        private object GetId(TEntity item)
        {
            var type = typeof(TEntity);
            var name = type.Name;
            var idPropInfo = type.GetProperty(type.Name + "Id");
            if (idPropInfo == null)
                idPropInfo = type.GetProperty("Id");
            if (idPropInfo == null)
                throw new ArgumentException("Type " + name + " don't have any Id name");

            return idPropInfo.GetValue(item);
        }

        private void CopyObjectData(object source, object target)
        {
            string excludedProperties = null;
            BindingFlags memberAccess = BindingFlags.Instance | BindingFlags.Public;
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            MemberInfo[] miT = target.GetType().GetMembers(memberAccess);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;

                // Skip over any property exceptions
                if (!string.IsNullOrEmpty(excludedProperties) &&
                    excluded.Contains(name))
                    continue;

                if (Field.MemberType == MemberTypes.Field)
                {
                    FieldInfo SourceField = source.GetType().GetField(name);
                    if (SourceField == null)
                        continue;

                    object SourceValue = SourceField.GetValue(source);
                    ((FieldInfo)Field).SetValue(target, SourceValue);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    PropertyInfo piTarget = Field as PropertyInfo;
                    PropertyInfo SourceField = source.GetType().GetProperty(name, memberAccess);
                    if (SourceField == null)
                        continue;

                    if (piTarget.CanWrite && SourceField.CanRead)
                    {
                        object SourceValue = SourceField.GetValue(source, null);
                        piTarget.SetValue(target, SourceValue, null);
                    }
                }
            }
        }

        public TEntity Create()
        {
            return default(TEntity);
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity
        {
            return default(TDerivedEntity);
        }
    }
}
