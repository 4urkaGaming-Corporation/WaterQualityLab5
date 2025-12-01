namespace WaterQualityApi.Models
{
    public class Measurement
    {
        public int Id { get; set; }

        public int StationId { get; set; }
        public Station Station { get; set; } = null!;

        public int ParameterId { get; set; }
        public Parameter Parameter { get; set; } = null!;

        public DateTime MeasuredAt { get; set; }
        public double Value { get; set; }
    }
}
