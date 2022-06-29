using Bot2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot2.Models
{
    class Favorites
    {
        public string id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string position { get; set; }

        public Team Team { get; set; }
    }
}
