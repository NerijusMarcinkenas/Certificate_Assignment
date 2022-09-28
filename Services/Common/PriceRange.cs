namespace InsuranceCertificates.Services.Common
{
    public class PriceRange
    {

        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }
               
        public PriceRange()
        {
        }

        public PriceRange(decimal min, decimal max)
        {
            Minimum = min;
            Maximum = max;
        }

        public bool IsInRange(decimal value)
        {
            return value >= Minimum && value <= Maximum;
        }
    }
}
