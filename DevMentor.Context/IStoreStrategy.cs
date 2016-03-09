using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Context
{
    public interface IStoreStrategy
    {

        string PreLoad(Type type);

        object Load(string contents, Type type);
        string Save(object o, Type type);

        string ToDelete(object o, Type type);
        string ToUpdate(object o, Type type);


        string GetFileName(Type type);
    }
}
