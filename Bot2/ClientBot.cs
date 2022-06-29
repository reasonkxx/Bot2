using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bot2
{
    public class ClientBot
    {
        public HttpClient Client;
        public ClientBot()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(@"https://api-nba.herokuapp.com/");
        }
    }

}
