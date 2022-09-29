using Core.Domain;
using Core.Models;
using DataAccess.Repositories;
using InsuranceCertificates.Services;
using InsuranceCertificates.Services.Common;


namespace Services.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IDbRepository<Certificate> _dbRepository;

        public CertificateService( IDbRepository<Certificate> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public ServiceResultExtension<Certificate> CreateCertificate(CertificateModel model)
        {
            var certificateResult = new ServiceResultExtension<Certificate>();
            var validatedInputs = ServiceExtension.ValidateInputs(model);

            if (validatedInputs.Value == null)
            {
                certificateResult.Message = validatedInputs.Message;
                return certificateResult;
            }
            var certificateSum = ServiceExtension.GetCertificateSum(model.InsuredSum);
           
            if (certificateSum.Value == 0)
            {
                certificateResult.Message = certificateSum.Message;
                return certificateResult;
            }

            certificateResult.Value = ServiceExtension.CreateCertificateEntity(model, certificateSum.Value);           
            return certificateResult;          
        }

        public async Task<List<CertificateModel>> GetAllCertificateModelsAsync()
        {
            var certificates = await _dbRepository.GetAllCertificates();
            return certificates.Select(c => new CertificateModel(c)).ToList();
        }
       
        public async Task<ServiceResultExtension<Certificate>> SaveToDbAsync(Certificate certificate)
        {
            var addedCertificate = _dbRepository.AddNew(certificate);
            ServiceExtension.CreateCertificateNumber(addedCertificate);
            await _dbRepository.CommitAsync();

            var newCertificate = new ServiceResultExtension<Certificate>
            {
                Value = addedCertificate,
                Message = "Certificate created successfully"
            };
            return newCertificate;
        }       
    }
}
