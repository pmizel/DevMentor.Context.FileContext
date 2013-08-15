using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace DevMentor.Context
{
    public interface IFileSet:IQueryable, IEnumerable
    {
    }
    public class FileSet<TEntity> : IQueryable<TEntity>, IEnumerable<TEntity>, IFileSet where TEntity : class
    {

        public FileSet()
        {
            Local = new ObservableCollection<TEntity>();
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

        public TEntity Find(int id)
        {
            return Local.FirstOrDefault(i => (int)i.GetType().GetProperty("Id").GetValue(i) == id);
        }

        public void Add(TEntity item)
        {
            Local.Add(item);
        }

        public void Remove(TEntity item)
        {
            Local.Remove(item);
        }
    }
}