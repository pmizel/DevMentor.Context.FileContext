﻿using DevMentor.Context;
using DevMentor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Data
{
    public partial class Context : DevMentor.Context.FileContext
#if ENABLE_EF
        , IObjectContextAdapter
#endif
    {
        public Context()
            : base("name=ContentItemContext")
        {
        }
        public Context(IStoreStrategy store):
            base("name=ContentItemContext",store)
        {
        }

        public override string PrefixPath
        {
            get
            {
                return "DynamicSubPath";
            }
         }

        public FileSet<User> Users { get; set; }

        [FileSetUsePrefixPath]
        public FileSet<Info> Infos { get; set; }

        public System.Data.Entity.Core.Objects.ObjectContext ObjectContext
        {
            get
            {
                var objectContext = (this as IObjectContextAdapter);
                if (objectContext != null)
                    return (this as IObjectContextAdapter).ObjectContext;
                else
                    return null;
            }
        }
    }
}
