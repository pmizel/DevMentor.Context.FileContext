using DevMentor.Context.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Data.Entities
{
    public class Base: BaseEntity
    {
        public Guid OwnerId { get; set; }
        public bool Archived { get; set; }
    }
}
