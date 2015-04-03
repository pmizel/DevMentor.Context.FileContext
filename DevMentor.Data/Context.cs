using DevMentor.Context;
using DevMentor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Data
{
    public partial class Context : DevMentor.Context.FileContext,IObjectContextAdapter
    {
        public Context()
            : base("name=ContentItemContext")
        {
        }

        public FileSet<User> Users { get; set; }

        public System.Data.Entity.Core.Objects.ObjectContext ObjectContext
        {
            get { return null; }
        }
    }
}
