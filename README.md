DevMentor.Context.FileContext
=============================

DbContext replace to FileContext

Beipiel Code: 
1) Replace DbContext to FileContext
2) Replace DbSet to FileSet

```C#
    //public class ContentItemContext : DbContext
    public class ContentItemContext : FileContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(
        // new System.Data.Entity.DropCreateDatabaseIfModelChanges<MvcApplication1.Models.ContentItemContext>());

        public ContentItemContext()
            : base("name=ContentItemContext")
        {
        }

        public FileSet<ContentItem> ContentItems { get; set; }
        //public DbSet<ContentItem> ContentItems { get; set; }
    }
```
