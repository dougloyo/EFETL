using System;
using System.Diagnostics;
using System.Linq;
using EntityFramework.BulkInsert.Extensions;
using ETL.Data;
using ETL.Data.Models;

namespace ETL.Infrastructure
{
    public class StepAggregateSessionTime : IEtlStep
    {
        public void ExecuteEtl()
        {
            // Extract and Transform as much as possible on the database server.
            using (var context = new ETLContext())
            {
                // Aggregating average time spent per session/login.
                Console.WriteLine("Aggregating Data...");
                var data = context.Sources
                    //.Where(x=>x.DateTimeAccessedResource>startDate && x.DateTimeAccessedResource < endDate)
                    .GroupBy(x => x.UserId)
                    .Select(
                        d => new { UserID = d.Key, TimeSpentInSeconds = d.Average(s => s.TimeSpentInSeconds)})
                    .ToList()
                    // Project
                    .Select(r => new Destination { UserID = r.UserID, TimeSpentInSeconds = r.TimeSpentInSeconds }).ToList();

                // Load (save in destination)
                Console.WriteLine("Bulk inserting aggregated data. found: {0} rows.", data.Count().ToString("N0"));
                var stopwatch = Stopwatch.StartNew();
                context.BulkInsert(data);
                stopwatch.Stop();
                Console.WriteLine("Done in {0} milliseconds.", stopwatch.ElapsedMilliseconds);
            }
        }

    }
}
