namespace InsuranceCertificates.Services.Common
{
    public class ServiceResultExtension<T>
    {
        public T? Value { get; set; }
        public string? Message { get; set; }

    }
}
