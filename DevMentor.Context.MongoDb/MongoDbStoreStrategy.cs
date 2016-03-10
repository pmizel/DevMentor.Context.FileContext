using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Context.MongoDb
{
    /// <summary>
    /// Install-Package MongoDB.Driver
    /// </summary>
    public class MongoDbStoreStrategy : IStoreStrategy
    {
        MongoDB.Driver.IMongoDatabase database;
        public MongoDbStoreStrategy(string databasename)
        {
            MongoDB.Driver.MongoClient client = new MongoDB.Driver.MongoClient();
            database = client.GetDatabase(databasename);
        }
        public MongoDbStoreStrategy(string databasename, MongoDatabaseSettings settings)
        {
            MongoDB.Driver.MongoClient client = new MongoDB.Driver.MongoClient();
            database = client.GetDatabase(databasename, settings);
        }
        public string GetFileName(Type T)
        {
            return string.Empty;
        }

        public string ToDelete(object o, Type type)
        {
            var gtype = type.GenericTypeArguments.FirstOrDefault();

            var collection =
                typeof(MongoDB.Driver.IMongoDatabase)
                .GetMethod("GetCollection")
                .MakeGenericMethod(gtype)
                .Invoke(database, new object[] { gtype.Name, null });

            var list = o as IEnumerable;
            if (list != null)
            {
                foreach (var item in list)
                {

                    var name = gtype.Name;
                    var idPropInfo = gtype.GetProperty(type.Name + "Id");
                    if (idPropInfo == null)
                        idPropInfo = gtype.GetProperty("Id");
                    if (idPropInfo == null)
                        throw new ArgumentException("Type " + name + " don't have any Id name");

                    var id = (Guid)idPropInfo.GetValue(item);

                    //f=>f.Id
                    var filter = GetType().GetMethod("BuildExpression")
                        .MakeGenericMethod(gtype, typeof(bool))
                        .Invoke(this, new object[] { "Id", id });


                    //MongoDB.Driver.IMongoCollectionExtensions.DeleteOne
                    typeof(MongoDB.Driver.IMongoCollectionExtensions)
                    .GetMethods()
                    .Where(m => m.Name == "DeleteOne")
                    .First()
                    .MakeGenericMethod(gtype)
                    .Invoke(null, new object[] { collection, filter, default(System.Threading.CancellationToken) });
                }
            }
            return string.Empty;
        }

        public string ToUpdate(object o, Type type)
        {
            return string.Empty;
        }

        public object Load(string contents, Type type)
        {
            var gtype = type.GenericTypeArguments.FirstOrDefault();


            var collection =
                typeof(MongoDB.Driver.IMongoDatabase)
                .GetMethod("GetCollection")
                .MakeGenericMethod(gtype)
                .Invoke(database, new object[] { gtype.Name, null });


            var query = typeof(IMongoCollectionExtensions)
                .GetMethod("AsQueryable", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(gtype)
                .Invoke(null, new object[] { collection });


            var result = typeof(Enumerable)
                .GetMethod("ToList", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(gtype)
                .Invoke(null, new object[] { query });

            return result;
        }

        public string Save(object o, Type type)
        {
            var gtype = type.GenericTypeArguments.FirstOrDefault();


            var collection =
                typeof(MongoDB.Driver.IMongoDatabase)
                .GetMethod("GetCollection")
                .MakeGenericMethod(gtype)
                .Invoke(database, new object[] { gtype.Name, null });

            var list = o as IEnumerable;
            if (list != null)
            {
                foreach (var item in list)
                {
                    var isDirty = (Boolean)item.GetType().GetProperty("IsDirty").GetValue(item);
                    var isNew = (Boolean)item.GetType().GetProperty("IsNew").GetValue(item);

                    if (isDirty)
                    { //update
                        var name = gtype.Name;
                        var idPropInfo = gtype.GetProperty(type.Name + "Id");
                        if (idPropInfo == null)
                            idPropInfo = gtype.GetProperty("Id");
                        if (idPropInfo == null)
                            throw new ArgumentException("Type " + name + " don't have any Id name");

                        var id = (Guid)idPropInfo.GetValue(item);

                        //f=>f.Id
                        var filter = GetType().GetMethod("BuildExpression")
                            .MakeGenericMethod(gtype, typeof(bool))
                            .Invoke(this, new object[] { "Id", id });


                        typeof(MongoDB.Driver.IMongoCollectionExtensions)
                        //.MakeGenericType(gtype)
                        .GetMethods()
                        .Where(m => m.Name == "FindOneAndReplace")
                        .Where(m =>
                        {
                            return m.GetParameters()[1].ParameterType.Name == "Expression`1";
                        })
                        .First()
                        .MakeGenericMethod(gtype)
                        .Invoke(null, new object[] { collection, filter, item, null, default(System.Threading.CancellationToken) });
                    }
                    else if (isNew)
                    {

                        typeof(MongoCollectionBase<>)
                        .MakeGenericType(gtype)
                        .GetMethod("InsertOne")
                        //.MakeGenericMethod(gtype)
                        .Invoke(collection, new object[] { item, null, default(System.Threading.CancellationToken) });
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/12120751/creating-an-expressionfunct-object-variable-by-reflection
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Expression<Func<TClass, TProperty>> BuildExpression<TClass, TProperty>(string fieldName, Guid id)
        {
            // f=>f.[fieldName]
            //var param = Expression.Parameter(typeof(TClass));
            //var field = Expression.PropertyOrField(param, fieldName);
            //return Expression.Lambda<Func<TClass, TProperty>>(field, param);

            var param = Expression.Parameter(typeof(TClass));
            var field = Expression.PropertyOrField(param, fieldName);

            Expression target = Expression.Constant(id);
            Expression equalsMethod = Expression.Call(field, "Equals", null, target);

            return Expression.Lambda<Func<TClass, TProperty>>(equalsMethod, param);
        }

        public string PreLoad(Type type)
        {
            return string.Empty;
        }
    }

    //public static class ExpressionBuilder
    //{
    //    public static Expression<Func<TClass, TProperty>> Build<TClass, TProperty>(string fieldName)
    //    {
    //        var param = Expression.Parameter(typeof(TClass));
    //        var field = Expression.PropertyOrField(param, fieldName);
    //        return Expression.Lambda<Func<TClass, TProperty>>(field, param);
    //    }
    //}
}
