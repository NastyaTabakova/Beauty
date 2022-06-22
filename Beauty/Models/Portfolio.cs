using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class Portfolio
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string ServiceLocation { get; set; }
        public string WorkExperience { get; set; }
        public byte[] Image { get; set; }

        public List<byte[]> Images { get; set; }
    }
}
