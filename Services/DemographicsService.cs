using DemographicsApi.Model;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace DemographicsApi.Services
{
    public class DemographicsService
    {
        private readonly HttpClient _httpClient;

        public DemographicsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DemographicResult> GetDemographicDataAsync(string name)
        {
            // JSON options to safely deserialize incoming responses
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };

            // Prepare response variables
            AgifyResponse ageData = null;
            GenderizeResponse genderData = null;
            NationalizeResponse nationData = null;

            try
            {
                var ageResponse = await _httpClient.GetStringAsync($"https://api.agify.io/?name={name}");
                ageData = JsonSerializer.Deserialize<AgifyResponse>(ageResponse, options);
            }
            catch
            {
                ageData = new AgifyResponse { Age = null };
            }

            try
            {
                var genderResponse = await _httpClient.GetStringAsync($"https://api.genderize.io/?name={name}");
                genderData = JsonSerializer.Deserialize<GenderizeResponse>(genderResponse, options);
            }
            catch
            {
                genderData = new GenderizeResponse { Gender = null, Probability = 0 };
            }

            try
            {
                var nationalResponse = await _httpClient.GetStringAsync($"https://api.nationalize.io/?name={name}");
                nationData = JsonSerializer.Deserialize<NationalizeResponse>(nationalResponse, options);
            }
            catch
            {
                nationData = new NationalizeResponse { Country = new List<CountryProbability>() };
            }

            var result = new DemographicResult
            {
                Name = name,
                Age = ageData?.Age,
                Gender = genderData?.Gender,
                GenderProbability = genderData?.Probability,
                Nationalities = nationData?.Country != null
                    ? nationData.Country
                        .OrderByDescending(c => c.Probability)
                        .Take(2)
                        .Select(c => new CountryInfo
                        {
                            CountryCode = c.Country_Id,
                            CountryName = GetCountryName(c.Country_Id),
                            Probability = c.Probability
                        })
                        .ToList()
                    : new List<CountryInfo>()
            };

            return result;
        }

        // Helper method to convert country code to full country name
        private string GetCountryName(string countryCode)
        {
            try
            {
                var region = new RegionInfo(countryCode);
                return region.EnglishName;
            }
            catch
            {
                return countryCode; // fallback if code is invalid
            }
        }
    }
}
