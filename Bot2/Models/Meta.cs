using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot2.Models
{
    public class Meta
    {

        public int total_pages { get; set; }


        public int current_page { get; set; }


        public object next_page { get; set; }


        public int per_page { get; set; }


        public int total_count { get; set; }
    }
}
