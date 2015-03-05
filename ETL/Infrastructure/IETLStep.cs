using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Infrastructure
{
    public interface IEtlStep
    {
        void ExecuteEtl();
    }
}
