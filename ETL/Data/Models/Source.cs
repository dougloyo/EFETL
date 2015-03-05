using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Data.Models
{
    public class Source
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid SessionId { get; set; }
        public DateTime DateTimeAccessedResource { get; set; }
        public int LocalEducationAgencyId { get; set; }
        public int SchoolId { get; set; }
        public int StudentUSI { get; set; }
        public string Url { get; set; }
        public int TimeSpentInSeconds { get; set; }
    }
}
