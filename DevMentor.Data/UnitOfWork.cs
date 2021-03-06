﻿using DevMentor.Context;
using DevMentor.Context.Store;
using DevMentor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevMentor.Data
{
    public partial class UnitOfWork : IDisposable
    {
        private Context context;
        //private DocumentContext documentContext;
        public UnitOfWork()
        {
            context = new Context();
        }

        public UnitOfWork(IStoreStrategy store)
        {
            context = new Context(store);
        }

        public UnitOfWork(Context context)
        {
            this.context = context;
        }

        //public UnitOfWork(DocumentContext documentContext)
        //{
        //    this.documentContext = documentContext;
        //}

        //public bool IsFileContext()
        //{
        //    try
        //    {
        //        var cast = (System.Data.Entity.DbContext)context;
        //    }
        //    catch (Exception)
        //    {
        //        return true;
        //    }
        //    return false;
        //}


        //public void ClearDatabase()
        //{

        //    var list = "Accounts,AccountOptions,Divisions,DivisionLeaders,Companies,ServiceUsers,Services,Projects,Customers,UserOptions,Users,TimeEntries,TravelEntries".Split(',');
        //    foreach (var t in list)
        //    {
        //        try
        //        {
        //            context.Database.ExecuteSqlCommand(string.Format("DELETE FROM [{0}]", t));
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }


        //    var tableNames = context.Database.SqlQuery<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME NOT LIKE '%Migration%'").ToList();
        //    foreach (var tableName in tableNames)
        //    {
        //        foreach (var t in tableNames)
        //        {
        //            try
        //            {

        //                if (context.Database.ExecuteSqlCommand(string.Format("TRUNCATE TABLE [{0}]", t)) == 1)
        //                    break;

        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }
        //    context.SaveChanges();
        //}


        //public void DeleteAll()
        //{
        //    ClearDatabase();
        //}


        private GenericRepository<User> userRepository;
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                    this.userRepository = new GenericRepository<User>(context);
                return userRepository;
            }
        }

        private GenericRepository<Info> infoRepository;
        public GenericRepository<Info> InfoRepository
        {
            get
            {
                if (this.infoRepository == null)
                    this.infoRepository = new GenericRepository<Info>(context);
                return infoRepository;
            }
        }

        //private GenericMongoRepository<User> userMongoRepository;
        //public GenericMongoRepository<User> UserMongoRepository
        //{
        //    get
        //    {
        //        if (this.userMongoRepository == null)
        //            this.userMongoRepository = new GenericMongoRepository<User>(documentContext);
        //        return userMongoRepository;
        //    }
        //}

        //private GenericRepository<UserOptions> userOptionsRepository;
        //public GenericRepository<UserOptions> UserOptionsRepository
        //{
        //    get
        //    {
        //        if (this.userOptionsRepository == null)
        //            this.userOptionsRepository = new GenericRepository<UserOptions>(context);
        //        return userOptionsRepository;
        //    }
        //}

        int _counter = 0;
        public void Save()
        {
            try
            {
                context.SaveChanges();
                _counter = 0;
            }
            catch (Exception ex)
            {
                if (_counter < 10)
                {
                    _counter++;
                    Thread.Sleep(1000 * _counter);
                    Save();
                }
                else
                {
                    var octx = context.ObjectContext;
                    if (octx != null)
                    {
                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        octx.Refresh(System.Data.Entity.Core.Objects.RefreshMode.ClientWins, context.Users);
                        // Save changes.
                        context.SaveChanges();

                        Console.WriteLine("OptimisticConcurrencyException "
                        + "handled and changes saved");
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
