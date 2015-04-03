using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Data.Entities
{
    public class Base
    {
        public Base()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;

        }

        public void Update()
        {
            UpdatedOn = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid OwnerId { get; set; }
        public bool Archived { get; set; }
    }
}
