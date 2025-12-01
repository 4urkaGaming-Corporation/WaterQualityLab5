namespace WaterQualityApi.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;

        public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
    }
}
