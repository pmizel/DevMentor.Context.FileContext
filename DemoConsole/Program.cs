using DevMentor.Context.Store;
using DevMentor.Data;
using DevMentor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var unit = new UnitOfWork(new Context()); //new Context(new InMemoryStoreStrategy())
            //INSERT
            Console.WriteLine("INSERT PAUL");
            unit.UserRepository.Insert(new User() { UserName="pmizel",FirstName="Paul", LastName="Mizel"});
            Console.WriteLine("INSERT FABIAN");
            unit.UserRepository.Insert(new User() { UserName = "fraetz", FirstName = "Fabian", LastName = "Raetz" });
            unit.Save();

            Console.WriteLine("GET ALL");
            var users=unit.UserRepository.Get(); //GET ALL
            foreach (var user in users)
            {
                Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
            }

            Console.WriteLine("GET PAUL");
            users = unit.UserRepository.Get(f=>f.FirstName=="Paul"); //GET ALL
            foreach (var user in users)
            {
                Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
            }

            //DELETE ALL
            Console.WriteLine("DELETE ALL");
            unit.UserRepository.Delete(unit.UserRepository.Get());
            users = unit.UserRepository.Get();
            foreach (var user in users)
            {
                Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
            }
            unit.Save();

            Console.ReadKey();
        }
    }
}
