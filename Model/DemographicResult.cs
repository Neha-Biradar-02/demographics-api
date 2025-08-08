namespace DemographicsApi.Model
{
    public class DemographicResult
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public double? GenderProbability { get; set; }
        public List<CountryInfo> Nationalities { get; set; }
    }
    public class CountryInfo
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public double Probability { get; set; }
    }
}
