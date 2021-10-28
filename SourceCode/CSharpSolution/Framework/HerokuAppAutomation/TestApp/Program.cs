using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using JsonSerializer = RestSharp.Serialization.Json.JsonSerializer;
using Newtonsoft.Json.Linq;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var testData = Utilities.ExcelDataManager.GetTestData(Constants.SuiteType.API, "Test Case 1 - Create Booking Postive scenario");
            RestClient client = new RestClient("https://restful-booker.herokuapp.com");
            RestRequest request = new RestRequest("/booking", Method.POST);
            IRestResponse response = null;
            request.RequestFormat = DataFormat.Json;
            var aa = testData["Request Body"];
            //booking dobj =  JsonConvert.DeserializeObject<booking>(aa);
            //request.JsonSerializer = serializer;
            request.AddJsonBody(aa);
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add(Constants.API.HeaderConstants.ACCEPT, Constants.API.HeaderConstants.APPLICATION_JSON);
            requestHeaders.Add(Constants.API.HeaderConstants.CONTENT_TYPE, Constants.API.HeaderConstants.APPLICATION_JSON);
            foreach (var header in requestHeaders)
                request.AddHeader(header.Key, header.Value);
            try
            {
                response = client.Execute(request);
                Console.WriteLine($"API with POST method has been executed successfully.");
                Console.WriteLine
                    ($"Status Code of the POST Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
                dynamic api = JObject.Parse(response.Content);
                int bookingId = api.bookingid;
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform post call on the URI {"booking"}" +
                          ex.Message;
                Console.WriteLine(msg);
                throw new Exception(msg, ex);
            }
        }
    }

    public class booking
    {
        public string firstname;
        public string lastname;
        public int totalprice;
        public bool depositpaid;

        public class bookingdates
        {
            public DateTime checkin;
            public DateTime checkout;
        }

        public string additionalneeds;
    }
}
