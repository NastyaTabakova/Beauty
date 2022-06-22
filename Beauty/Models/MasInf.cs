using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class MasInf
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public int Price { get; set; }
        public int SubId { get; set; }
        public string Discription { get; set; }

        public List<string> Dates { get; set; }


    }
}
