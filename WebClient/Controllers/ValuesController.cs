﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    public class ClientResponse
    {
        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("recommendationState")]
        public string RecommendationState { get; set; }

        [JsonPropertyName("overallScore")]
        public float OverallScore { get; set; }

        [JsonPropertyName("airQualityScore")]
        public float AirQualityScore { get; set; }

        [JsonPropertyName("covidIndexScore")]
        public float CovidIndexScore { get; set; }

        [JsonPropertyName("weatherScore")]
        public float WeatherScore { get; set; }
    }

    public class ValuesController : ApiController
    {
        private static async Task<List<ClientResponse>> RecommendationClient(string endpoint)
        {
            HttpClient client = new HttpClient();

            var resp = await client.GetAsync(endpoint);
            var stringValue = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ClientResponse>>(stringValue);
        }

        private static async Task<ClientResponse> LocationInquiryClient(string endpoint)
        {
            HttpClient client = new HttpClient();

            var resp = await client.GetAsync(endpoint);
            var stringValue = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ClientResponse>(stringValue);
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] {
                "endpoint should be https://localhost:44324/api/values/1 for the Recommendation Client",
                "endpoint should be https://localhost:44324/api/values/2 for the Location Inquiry Client"
            };
        }

        // GET api/values/{id}
        public async Task<string> Get(int id)
        {
            // can hit either recommendation or location inquiry endpoint from Safe Travel Service _once_
            string recommendationEndpoint = "https://localhost:5001/api/recommendations";
            string locationInquiryEndpoint = "https://localhost:5001/api/recommendations/country/US/state/CA";

            if (id == 1)
            {
                System.Diagnostics.Debug.WriteLine(id);

                string result = "";
                var task = await RecommendationClient(recommendationEndpoint);
                foreach (var clientResponse in task)
                {
                    result += clientResponse.State + " " + clientResponse.RecommendationState + " " + clientResponse.OverallScore + "\n";
                }

                return result;
            }
            else
            {
                var clientResponse = await LocationInquiryClient(locationInquiryEndpoint);
                string result = clientResponse.CovidIndexScore + " " +
                    clientResponse.AirQualityScore + " " +
                    clientResponse.OverallScore + " " +
                    clientResponse.RecommendationState;
                return result;
            }
        }

        // POST api/values. Not used.
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5. Not used.
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5. Not used.
        public void Delete(int id)
        {
        }
    }
}
