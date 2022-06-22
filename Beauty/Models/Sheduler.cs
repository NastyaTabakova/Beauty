using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class Sheduler
    {
        public int Id { get; set; }
        public int Masterinfoid { get; set; }
        public string Date { get; set; }
        public string Time{ get; set; }
        
        public string Status { get; set; }
        
    }
}
