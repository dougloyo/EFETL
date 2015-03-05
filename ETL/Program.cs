using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EntityFramework.BulkInsert.Extensions;
using ETL.Data;
using ETL.Data.Models;
using ETL.Infrastructure;

namespace ETL
{
    class Program
    {
        // TODO: Replace with IoC constructor array injection.
        private static IEtlStep[] EtlSteps { get; set; }
        public static void Init()
        {
            EtlSteps = new IEtlStep[]{ new StepAggregateSessionTime(), };
        }

        static void Main(string[] args)
        {
            Console.WriteLine("ETL Console");

            //initDbEF();
            //initDbEFOptimized();
            //initDbEFBulk();

            var stopwatch = Stopwatch.StartNew();

            Init();
            initDbEFBulk();

            foreach (var etlStep in EtlSteps)
            {
                // Write status
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Executing step:" + etlStep.ToString());
                Console.ResetColor();                
                etlStep.ExecuteEtl();
            }
            stopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Total time for all:" + stopwatch.ElapsedMilliseconds.ToString("N0") + " milliseconds (" + stopwatch.ElapsedMilliseconds / 1000 + " seconds)");
            Console.ResetColor();

            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Done");

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

            int totalRecordCount = 0;
            using (var context = new ETLContext())
            {
                var sourceData = GenerateData(1000000);

                Console.WriteLine("Bulk inserting data to Db...");
                context.BulkInsert(sourceData);

                totalRecordCount = context.Sources.Count();
            }

            stopwatch.Stop();

            Console.WriteLine("Total source records in database {0}", totalRecordCount.ToString("N0"));

            Console.WriteLine("Total time for Bulk Insert {0} milliseconds {1} seconds", stopwatch.ElapsedMilliseconds, stopwatch.ElapsedMilliseconds / 1000);

        }

        public static List<Source> GenerateData(int rowCount)
        {
            var sourceData = new List<Source>();

            Console.WriteLine("Generating Data");

            var dataGenStopwatch = Stopwatch.StartNew();

            for (var i = 0; i < rowCount; i++)
                sourceData.Add(new Source
                {
                    UserId = GenerateUser(i),
                    DateTimeAccessedResource = DateTime.Now,
                    SessionId = GenerateGuid(i),
                    LocalEducationAgencyId = i%10, 
                    SchoolId = i, 
                    StudentUSI = 55, 
                    Url = "Http://www.ed-fi.org/something",
                    TimeSpentInSeconds = GetTimeSpent(),
                });

            dataGenStopwatch.Stop();

            Console.WriteLine(string.Format("{0} Rows Generated in {1} milliseconds ({2}s)",sourceData.Count.ToString("N0"),dataGenStopwatch.ElapsedMilliseconds,(dataGenStopwatch.ElapsedMilliseconds/1000)));

            return sourceData;
        }

        private static readonly Random Random = new Random();
        private static int GetTimeSpent()
        {
            return Random.Next(10, 240);
        }

        private static Guid _lastGuid;
        public static Guid GenerateGuid(int i)
        {
            if(i%45==0)
                _lastGuid = Guid.NewGuid();

            return _lastGuid;
        }

        private static int  _lastUser;
        public static int GenerateUser(int i)
        {
            if (i % 100 == 0)
                _lastUser = i;

            return _lastUser;
        }
    }
}
