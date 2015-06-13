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
            unit.UserRepository.Insert(new User()
            {
                UserName = "pmizel",
                FirstName = "Paul",
                LastName = "Mizel",
                Birthday = new DateTime(1980, 4, 1)
            });
            Console.WriteLine("INSERT FABIAN");
            unit.UserRepository.Insert(new User()
            {
                UserName = "fraetz",
                FirstName = "Fabian",
                LastName = "Raetz",
                Birthday = new DateTime(1989, 4, 1)
            });
            unit.Save();

            Console.WriteLine("GET ALL");
            var users = unit.UserRepository.Get(); //GET ALL
            foreach (var user in users)
            {
                Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
            }

            Console.WriteLine("GET Youngest");
            var youngest = unit.UserRepository.Get(orderBy: o => o.OrderByDescending(i => i.Birthday)).First();
            Console.WriteLine("{0} {1} ({2}) - {3:dd.MM.yyyy}", youngest.FirstName,
                                                                youngest.LastName,
                                                                youngest.UserName,
                                                                youngest.Birthday);

            Console.WriteLine("GET PAULs");
            users = unit.UserRepository.Get(f => f.FirstName == "Paul"); //GET ALL
            foreach (var user in users)
            {
                Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
            }
            Console.WriteLine("UPDATE PAULs to PABLOs");
            foreach (var user in users)
            {
                user.FirstName = "Pablo";
                unit.UserRepository.Update(user);
            }
            unit.Save();

            Console.WriteLine("GET ALL");
            users = unit.UserRepository.Get(); //GET ALL
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
