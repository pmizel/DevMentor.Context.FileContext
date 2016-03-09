using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DevMentor.Context.Entity
{
    public abstract class BaseEntity
    {
        private bool isDirty;

        public BaseEntity()
        {
            var date = DateTime.Now;
            Id = Guid.NewGuid();
            Created = date;
            Updated = date;
        }

        [BsonId(IdGenerator = typeof(GuidGenerator))]
        [Key]
        public Guid Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        [NotMapped]
        [XmlIgnore]
        [BsonIgnore]
        public virtual bool IsDirty { get { return isDirty; } }


        [XmlIgnore]
        [NotMapped]
        [BsonIgnore]
        public bool IsNew { get { return Created == Updated; } }

        public void MakeDirty(bool dirty = false)
        {
            Updated = DateTime.Now;
            isDirty = dirty;
        }
    }
}
