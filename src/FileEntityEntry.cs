using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DevMentor.Context
{
    public class FileEntityEntry
    {
        public FileEntityEntry()
        {

        }

        public EntityState State
        {
            get { return EntityState.Modified; }
            set { }
        }
    }
}
