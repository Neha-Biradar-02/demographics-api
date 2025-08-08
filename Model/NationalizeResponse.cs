namespace DemographicsApi.Model
{
    public class NationalizeResponse
    {
        public string Name { get; set; }
        public List<CountryProbability> Country { get; set; }

    }
    public class CountryProbability
    {
        public string Country_Id { get; set; }
        public double Probability { get; set; }
    }

}
