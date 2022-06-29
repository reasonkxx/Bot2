using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot2.Models
{
    public class PlayerStatics
    {
        public int games_played { get; set; }


        public int player_id { get; set; }


        public int Season { get; set; }


        public string Min { get; set; }


        public double Fgm { get; set; }


        public double Fga { get; set; }


        public double Fg3m { get; set; }


        public double Fg3a { get; set; }


        public double Ftm { get; set; }


        public double Fta { get; set; }


        public double Oreb { get; set; }


        public double Dreb { get; set; }


        public double Reb { get; set; }


        public double Ast { get; set; }


        public double Stl { get; set; }


        public double Blk { get; set; }


        public double Turnover { get; set; }


        public double Pf { get; set; }


        public double Pts { get; set; }


        public double fg_pct { get; set; }


        public double fg_3pct { get; set; }


        public double ft_pct { get; set; }
    }
    public class Root1
    {
        public List<PlayerStatics> Data { get; set; }
    }
}

