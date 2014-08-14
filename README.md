DevMentor.Context.FileContext
=============================

![DevMentor Logo](http://devmentor.de/templates/devmentor/images/devmentor_logo.png "DevMentor")

modify EntityFramework DbContext to FileContext

##Todo: 
  >1. Replace DbContext to FileContext
  >2. Replace DbSet to FileSet

##Example Code: 

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


##StoreStrategy

```C#
    //public class ContentItemContext : DbContext
    public class ContentItemContext : FileContext
    {
        public ContentItemContext()
            : base(new JsonStoreStrategy()) 
			// or DefaultStoreStrategy() => XmlStoreStrategy()
        {
        }

        public FileSet<ContentItem> ContentItems { get; set; }
        //public DbSet<ContentItem> ContentItems { get; set; }
    }
```