using System;
using System.Collections.Generic;
using System.Diagnostics;
using EntityFramework.BulkInsert.Extensions;
using ETL.Data;
using ETL.Data.Models;

namespace ETL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ETL Console");

            //initDbEF();
            //initDbEFOptimized();
            initDbEFBulk();

            Console.Read();
        }

        /// <summary>
        /// 10,000 rows inserted in 35,898 milliseconds (35s)
        /// </summary>
        public static void initDbEF()
        {
            var stopwatch = Stopwatch.StartNew();

            using (var context = new ETLContext()) 
            {
                var sourceData = GenerateData(10000);

                sourceData.ForEach(i => context.Sources.Add(i));

                context.SaveChanges();
            }

            stopwatch.Stop();

            Console.WriteLine(stopwatch.ElapsedMilliseconds + " seconds:" + stopwatch.ElapsedMilliseconds/1000);
        }

        /// <summary>
        /// 10,000 rows inserted in 4612 milliseconds (4s)
        /// </summary>
        public static void initDbEFOptimized()
        {
            var stopwatch = Stopwatch.StartNew();

            using (var context = new ETLContext())
            {
                // Optimization 10,000 in 4 seconds vs 35
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                var sourceData = GenerateData(10000);

                context.Sources.AddRange(sourceData);
                context.SaveChanges();
            }

            stopwatch.Stop();

            Console.WriteLine(stopwatch.ElapsedMilliseconds + " seconds:" + stopwatch.ElapsedMilliseconds / 1000);
        }

        /// <summary>
        /// 10,000 rows inserted in 1833 milliseconds (1s)
        /// 100,000 rows inserted in 2640 milliseconds (2s)
        /// 1,000,000 rows inserted in 10208 milliseconds (10s)
        /// </summary>
        public static void initDbEFBulk()
        {
            var stopwatch = Stopwatch.StartNew();

            using (var context = new ETLContext())
            {
                var sourceData = GenerateData(1000000);

                context.BulkInsert(sourceData);
            }

            stopwatch.Stop();

            Console.WriteLine(stopwatch.ElapsedMilliseconds + " seconds:" + stopwatch.ElapsedMilliseconds / 1000);
        }

        public static List<Source> GenerateData(int rowCount)
        {
            var sourceData = new List<Source>();

            Console.WriteLine("Generating Data");

            var dataGenStopwatch = Stopwatch.StartNew();

            for (var i = 0; i < rowCount; i++)
                sourceData.Add(new Source { LocalEducationAgencyId = i, SchoolId = i % 2, StudentUSI = 55, Url = "Http://www.ed-fi.org/something" });

            dataGenStopwatch.Stop();

            Console.WriteLine(string.Format("{0} Rows Generated in {1} milliseconds ({2}s)",sourceData.Count.ToString("N0"),dataGenStopwatch.ElapsedMilliseconds,(dataGenStopwatch.ElapsedMilliseconds/1000)));

            return sourceData;
        }
    }
}
