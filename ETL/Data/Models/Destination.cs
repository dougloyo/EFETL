using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Data.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public int LEAId { get; set; }
        public int SchoolId { get; set; }
        public string URL { get; set; }
    }
}
