using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public int Categoryid { get; set; }
        public string Subcategoryname { get; set; }
        public byte[] Image { get; set; }

    }
}
