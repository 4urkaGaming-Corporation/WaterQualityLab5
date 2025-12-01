namespace WaterQualityApi.Models
{
    public class Parameter
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;

        public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
    }
}
