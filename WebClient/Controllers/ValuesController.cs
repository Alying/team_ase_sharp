﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Http;


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
            client.DefaultRequestHeaders.Accept.Clear();

            var streamTask = client.GetStreamAsync(endpoint).Result;
            var response = System.Text.Json.JsonSerializer.DeserializeAsync<List<ClientResponse>>(streamTask).Result;
            return response;
        }
        private static async Task<ClientResponse> LocationInquiryClient(string endpoint)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            var streamTask = await client.GetStreamAsync(endpoint);
            var response = await System.Text.Json.JsonSerializer.DeserializeAsync<ClientResponse>(streamTask);
            return response;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            // can hit either recommendation or location inquiry endpoint from Safe Travel Service
            string recommendationEndpoint = "https://localhost:5001/api/recommendations";
            string locationInquiryEndpoint = "https://localhost:5001/api/recommendations/country/US/state/California";

            if (id == 1)
            {
                System.Diagnostics.Debug.WriteLine(id);

                string result = "";
                var task = RecommendationClient(recommendationEndpoint);
                foreach (var clientResponse in task.Result)
                {
                    result += clientResponse.State + " " + clientResponse.RecommendationState + " " + clientResponse.OverallScore + "\n";
                }

                return result;
            }
            else
            {
                return "hi2";
            }
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}