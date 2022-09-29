using Core.Domain;
using Core.Models;
using InsuranceCertificates.Services.Common;

namespace Services.Services
{
    public interface ICertificateService
    {
        Task<List<CertificateModel>> GetAllCertificateModelsAsync();
        Task<ServiceResultExtension<Certificate>> SaveToDbAsync(Certificate certificate);
        ServiceResultExtension<Certificate> CreateCertificate(CertificateModel model);
    }
}