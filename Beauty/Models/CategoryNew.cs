using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class CategoryNew
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public byte[] Image { get; set; }

    }
}
