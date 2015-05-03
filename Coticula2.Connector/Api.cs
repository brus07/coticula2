using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Connector
{
    public class Api
    {
        private const string Url = "http://localhost:54828/api";

        public static Submit GetSubmit(int id)
        {
            var client = new RestClient(Url);

            var request = new RestRequest("Submits/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            IRestResponse<Submit> response = client.Execute<Submit>(request);
            return response.Data;
        }

        public static int[] UntestedSubmits()
        {
            var client = new RestClient(Url);

            var request = new RestRequest("Submits", Method.GET);
            IRestResponse<List<int>> response = client.Execute<List<int>>(request);
            return response.Data.ToArray();
        }
    }
}
