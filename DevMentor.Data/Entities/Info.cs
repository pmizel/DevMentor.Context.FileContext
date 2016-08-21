using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DevMentor.Data.Entities
{
    public partial class Info : Base
    {
        public Info()
            : base()
        {

        }

        public string Description { get; set; }
    }


}
