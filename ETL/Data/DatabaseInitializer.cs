using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETL.Data.Models;

namespace ETL.Data
{
    public class DataBaseInitializer : DropCreateDatabaseIfModelChanges<ETLContext>
    {
        protected override void Seed(ETLContext context)
        {
        }
    }
}
