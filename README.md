DevMentor.Context.FileContext
=============================

![DevMentor Logo](http://devmentor.de/templates/devmentor/images/devmentor_logo.png "DevMentor")

FileContext is a data access layer (DAL) Framework for 
rapid data driven application development (RDDAD). 

###benefits
  > 1. don't need a database
  > 2. rapid data-modeling, -modification
  > 3. provide your data in version-control
  > 4. rapid synchronisation between environments (test -> dev)
  > 5. all serializable .NET types are allowed (DateTime.Min, TimeSpan > 24h,...)
  > 6. all linq query with own-methods as FilterExpression are allowed
  > 7. rapid migration to Entity-Framework 6.*
  > 8. Generic repository pattern inside
  > 9. Unit of work patten inside
  >10. threadsafe for prototyping
  
hint: don't use for production.

###Todo: in two steps to FileContext
modify EntityFramework DbContext to FileContext
  >1. Replace DbContext to FileContext
  >2. Replace DbSet to FileSet

###Example Code:
 
```C#
var unit = new UnitOfWork(new Context()); //new Context(new InMemoryStoreStrategy())
//INSERT
Console.WriteLine("INSERT PAUL");
unit.UserRepository.Insert(new User() { UserName="pmizel",
                                        FirstName="Paul", 
										LastName="Mizel"});
Console.WriteLine("INSERT FABIAN");
unit.UserRepository.Insert(new User() { UserName = "fraetz", 
                                        FirstName = "Fabian", 
										LastName = "Raetz" });
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
```

###Output:
```sh
INSERT PAUL
INSERT FABIAN
GET ALL
Paul Mizel (pmizel)
Fabian Raetz (fraetz)
GET PAUL
Paul Mizel (pmizel)
DELETE ALL
```

###Example Context: 

```C#
//public class ContentItemContext : DbContext
public class ContentItemContext : FileContext
{
	public ContentItemContext()
		: base("name=ContentItemContext")
	{
	}

	public FileSet<ContentItem> ContentItems { get; set; }
	//public DbSet<ContentItem> ContentItems { get; set; }
}
```


###StoreStrategy

```C#
//public class ContentItemContext : DbContext
public class ContentItemContext : FileContext
{
	public ContentItemContext()
		: base(new JsonStoreStrategy()) 
		// or DefaultStoreStrategy() => XmlStoreStrategy()
		// or InMemoryStoreStrategy()
	{
	}

	public FileSet<ContentItem> ContentItems { get; set; }
	//public DbSet<ContentItem> ContentItems { get; set; }
}
```

for UnitTests/IntegrationsTests use InMemoryStoreStrategy instance.
