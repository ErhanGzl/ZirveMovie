using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;

namespace ZirveMovie.ORM
{
    public interface IOperationBaseClass
    {
        int Insert(IEntityBaseClass personel);
        int Update(IEntityBaseClass persnl);
        int Delete(int AutoID);
        IEntityBaseClass Select(int AutoID);
    }
}
