using CORE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Comman
{
    public interface ICommanDDLRepository
    {
        Task<List<DDList>> DDLgetlist(CommanDDLFuntion commonInput);
    }
}
