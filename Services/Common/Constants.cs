using InsuranceCertificates.Services.Common;

namespace InsuranceCertificates.Services
{
    public static class Constants
    {
        public static Dictionary<PriceRange, decimal> GetPriceRanges()
        {
            return new Dictionary<PriceRange, decimal>
            {
                { new PriceRange(20M, 50M), 8M },
                { new PriceRange(50.01M,100M), 15M },
                { new PriceRange(100.01M,200M), 25M},
            };

        }
    }
}
