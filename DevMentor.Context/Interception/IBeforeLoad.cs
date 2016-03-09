using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Context.Interception
{
    public interface IBeforeLoad
    {
        void OnBeforeLoad(object sender);
    }
}
