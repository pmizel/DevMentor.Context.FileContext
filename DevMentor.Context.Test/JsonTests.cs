using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevMentor.Data;
using DevMentor.Context.Store;

namespace DevMentor.Context.Test
{
    [TestClass]
    public class JsonStoreTests
    {
        IStoreStrategy store=new JsonStoreStrategy();
        [TestMethod]
        public void JsonStoreInsertTest()
        {
            var user=new Data.Entities.User
            {
                LastName = "Insert",
                FirstName = "FirstName "+DateTime.Now.ToString()
            };


            UnitOfWork unit = new UnitOfWork(store);
            unit.UserRepository.Insert(user);
            unit.Save();

            unit = new UnitOfWork(store);
            var datauser=unit.UserRepository.GetByID(user.Id);

            Assert.IsNotNull(datauser);
            Assert.AreEqual(datauser.FirstName, user.FirstName);

            unit.UserRepository.Delete(user);
            unit.Save();
        }

        [TestMethod]
        public void JsonStoreDeleteTest()
        {
            var user = new Data.Entities.User
            {
                LastName = "Delete",
                FirstName = "FirstName " + DateTime.Now.ToString()
            };

            UnitOfWork unit = new UnitOfWork(store);
            unit.UserRepository.Insert(user);
            unit.Save();

            unit = new UnitOfWork(store);
            unit.UserRepository.Delete(user.Id);
            unit.Save();

            unit = new UnitOfWork(store);
            var datauser = unit.UserRepository.GetByID(user.Id);

            Assert.IsNull(datauser);
            
        }


        [TestMethod]
        public void JsonStoreUpdateTest()
        {
            var user = new Data.Entities.User
            {
                LastName ="Update",
                FirstName = "FirstName " + DateTime.Now.ToString()
            };

            UnitOfWork unit = new UnitOfWork(store);
            unit.UserRepository.Insert(user);
            unit.Save();

            unit = new UnitOfWork(store);
            var datauser = unit.UserRepository.GetByID(user.Id);
            Assert.IsNotNull(datauser);
            Assert.AreEqual(datauser.FirstName, user.FirstName);

            var firstNameUpdated = "FirstName updated";
            datauser.FirstName = firstNameUpdated;
            unit.UserRepository.Update(datauser);
            unit.Save();

            unit = new UnitOfWork(store);
            datauser = unit.UserRepository.GetByID(user.Id);

            Assert.IsNotNull(datauser);
            Assert.AreEqual(datauser.FirstName, firstNameUpdated);

            unit.UserRepository.Delete(user);
            unit.Save();
        }
    }



    //public class GenericMongoDbRepository<TEntity>
    //     where TEntity : class
    //{
    //    internal DevMentor.Data.DocumentContext context;
    //    //internal IDbSet<TEntity> set; //install-package entityframework
    //    internal DocumentSet<TEntity> set;

    //    public GenericMongoDbRepository(DevMentor.Data.DocumentContext context)
    //    {
    //        this.context = context;
    //        this.set = context.Set<TEntity>();
    //    }

    //    public virtual IEnumerable<TEntity> Get(
    //      Expression<Func<TEntity, bool>> filter = null,
    //      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    //      string includeProperties = "")
    //    {
    //        IQueryable<TEntity> query = set.Local.AsQueryable();

    //        if (filter != null)
    //        {
    //            query = query.Where(filter);
    //        }

    //        //foreach (var includeProperty in includeProperties.Split
    //        //    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    //        //{
    //        //    query = query.Include(includeProperty);
    //        //}

    //        if (orderBy != null)
    //        {
    //            return orderBy(query).ToList();
    //        }
    //        else
    //        {
    //            return query.ToList();
    //        }
    //    }

    //    //public virtual IEnumerable<TEntity> Get(Guid accountId)
    //    //{
    //    //    IQueryable<TEntity> query = set;
    //    //    TODO Filter by AccountId == OwnerId
    //    //    return query.ToList();
    //    //}

    //    public virtual IEnumerable<TEntity> Get()
    //    {
    //        IQueryable<TEntity> query = set.Local.AsQueryable();
    //        return query.ToList();
    //    }
    //    public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null)
    //    {
    //        IQueryable<TEntity> query = set.Local.AsQueryable();
    //        if (filter != null)
    //            query = query.Where(filter);
    //        return query;
    //    }

    //    public virtual TEntity GetByID(object id)
    //    {
    //        return set.Find(id);
    //    }

    //    public virtual void Insert(TEntity entity)
    //    {
    //        set.Add(entity);
    //    }
    //    //public virtual void InsertOrUpdate(TEntity entity)
    //    //{
    //    //    if (set.Contains(entity))
    //    //    {
    //    //        Update(entity);
    //    //    }
    //    //    else
    //    //    {
    //    //        Insert(entity);
    //    //    }
    //    //}

    //    public virtual void Delete(object id)
    //    {
    //        TEntity entityToDelete = set.Find(id);
    //        Delete(entityToDelete);
    //    }

    //    public virtual void Delete(IEnumerable<TEntity> entitysToDelete)
    //    {
    //        foreach (var entityToDelete in entitysToDelete)
    //        {
    //            Delete(entityToDelete);
    //        }
    //    }

    //    public virtual void Delete(TEntity entityToDelete)
    //    {
    //        //if (context.Entry(entityToDelete).State == EntityState.Detached)
    //        //{
    //        //    set.Attach(entityToDelete);
    //        //}
    //        set.Remove(entityToDelete);
    //    }

    //    public virtual void Update(List<TEntity> entitysToUpdate)
    //    {
    //        foreach (var entityToUpdate in entitysToUpdate)
    //        {
    //            Update(entityToUpdate);
    //        }
    //    }

    //    public virtual void Update(TEntity entityToUpdate)
    //    {
    //        set.Attach(entityToUpdate);
    //        //context.Entry(entityToUpdate).State = EntityState.Modified;
    //    }

    //    public TEntity Find(Expression<Func<TEntity, bool>> filter)
    //    {
    //        return set.Local.AsQueryable().FirstOrDefault(filter);
    //        //return context.Set<TEntity>().SingleOrDefault(filter);
    //    }

    //    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter)
    //    {
    //        return await Task.FromResult(set.Local.AsQueryable().FirstOrDefault(filter));
    //        //return await context.Set<TEntity>().SingleOrDefaultAsync(filter);
    //    }

    //    public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> filter)
    //    {
    //        return set.Local.AsQueryable().Where(filter).ToList();
    //        //return context.Set<TEntity>().Where(filter).ToList();
    //    }

    //    public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter)
    //    {
    //        return set.Local.AsQueryable().Where(filter).ToList();
    //        //return await context.Set<TEntity>().Where(filter).ToListAsync();
    //    }


    //    /// <summary>
    //    /// Gets the count of the number of objects in the databse
    //    /// </summary>
    //    /// <remarks>Synchronous</remarks>
    //    /// <returns>The count of the number of objects</returns>
    //    public int Count(Expression<Func<TEntity, bool>> filter = null)
    //    {
    //        return set.Local.AsQueryable().Count(filter);
    //        //return context.Set<TEntity>().Count(filter);
    //    }

    //    /// <summary>
    //    /// Gets the count of the number of objects in the databse
    //    /// </summary>
    //    /// <remarks>Asynchronous</remarks>
    //    /// <returns>The count of the number of objects</returns>
    //    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
    //    {
    //        return await Task.FromResult(set.Local.AsQueryable().Count(filter));
    //        //return await context.Set<TEntity>().CountAsync(filter);
    //    }
    //}
}
