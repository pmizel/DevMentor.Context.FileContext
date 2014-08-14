using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Context
{
    public interface IStoreStrategy
    {
        object Load(string contents, Type type);
        string Save(object o, Type type);
        string GetFileName(Type T);
    }
}
