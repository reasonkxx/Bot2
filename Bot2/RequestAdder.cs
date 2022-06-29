using Bot2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Bot2
{
    public class RequestAdder
    {
        public static async Task AddRecuestAsync(string CurrentId, string UserIdt, string Requestt)
        {
            UserRequest request = new UserRequest
            {
                Id = CurrentId.ToString(),
                UserId = UserIdt,
                Request = Requestt

            };

            var json = JsonConvert.SerializeObject(request);

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var clientBot = new ClientBot();

            var post = await clientBot.Client.PostAsync("/Db/Add", data);

            post.EnsureSuccessStatusCode();

            var postcontent = post.Content.ReadAsStringAsync().Result;
            Console.WriteLine(postcontent);

        }

    }
}
