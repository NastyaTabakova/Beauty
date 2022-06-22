using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class ServiceHistory
    {
        public int id { get; set; }
        public int clientinfoid { get; set; }
        public int masterserviceid { get; set; }
        public string recorddate { get; set; }
        public string recordtime { get; set; }
        public string cost { get; set; }
        public string status { get; set; }

    }
}
