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
            var sourceData = new List<Source>();

            for (int i = 0; i < 10000; i++)
            {
                sourceData.Add(new Source{LocalEducationAgencyId = i, SchoolId = i%2, StudentUSI = 55, Url = "Http://www.ed-fi.org/something"});
            }

            sourceData.ForEach(i => context.Sources.Add(i));
            context.SaveChanges();
        }
    }
}
