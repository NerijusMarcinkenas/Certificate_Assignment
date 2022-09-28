using Core.Models;
using InsuranceCertificates.Services.Common;

namespace Services.Services
{
    public interface ICertificateService
    {
        Task<List<CertificateModel>> GetAllCertificates();
        Task<ServiceResultExtension<CertificateModel>> CreateCertificate(CertificateModel model);
    }
}