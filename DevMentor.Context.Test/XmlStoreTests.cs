﻿using System;
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
    public class XmlStoreTests
    {
        IStoreStrategy store=new XmlStoreStrategy();
        [TestMethod]
        public void XmlStoreInsertTest()
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
        public void XmlStoreDeleteTest()
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
        public void XmlStoreUpdateTest()
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

    
}
