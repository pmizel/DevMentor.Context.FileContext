using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Context.Interception
{
    public interface IBeforeSave
    {
        void OnBeforeSave(object sender);
    }
}
