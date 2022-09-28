using Core.Domain;
using Core.Models;
using DataAccess.Repositories;
using InsuranceCertificates.Services;
using InsuranceCertificates.Services.Common;


namespace Services.Services
{
    public class SertificateService : ICertificateService
    {
        private readonly IDbRepository<Certificate> _dbRepository;

        public SertificateService( IDbRepository<Certificate> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ServiceResultExtension<CertificateModel>> CreateCertificate(CertificateModel model)
        {
            var validatedInputs = ValidateInputs(model);
            if (validatedInputs.Value == null)
            {
                return validatedInputs;
            }

            return await AddNewCertificate(model);
        }

        public async Task<List<CertificateModel>> GetAllCertificates()
        {
            var certificates = await _dbRepository.GetAllCertificates();
            return certificates.Select(c => new CertificateModel(c)).ToList();
        }

        private static ServiceResultExtension<CertificateModel> ValidateInputs(CertificateModel model)
        {
            int validAge = 18;
            var resultExt = new ServiceResultExtension<CertificateModel>();
            if (model.CustomerDateOfBirth.AddYears(validAge) > DateTime.Now.Date)
            {
                resultExt.Message = $"User must be at least {validAge} years old";
                return resultExt;
            }
            if (string.IsNullOrEmpty(model.CustomerName))
            {
                resultExt.Message = "Costumer Name is required";
                return resultExt;
            }
            if (string.IsNullOrEmpty(model.InsuredItem))
            {
                resultExt.Message = "Insured item name is required";
                return resultExt;
            }
            resultExt.Value = model;
            return resultExt;

        }

        private async Task<ServiceResultExtension<CertificateModel>> AddNewCertificate(CertificateModel model)
        {
            var certificateSum = GetCertificateSum(model.InsuredSum);
            var createdCerificateModel = new ServiceResultExtension<CertificateModel>();
            if (certificateSum.Value == 0)
            {
                createdCerificateModel.Message = certificateSum.Message;
                return createdCerificateModel;
            }

            var certificate = CreateCertificate(model, certificateSum.Value);

            var addedCertificate = _dbRepository.AddNew(certificate);
            CreateCertificateNumber(addedCertificate);
            await _dbRepository.CommitAsync();

            createdCerificateModel.Value = new CertificateModel(certificate);
            createdCerificateModel.Message = "Certificate created successfully";
            return createdCerificateModel;
        }

        private static ServiceResultExtension<decimal> GetCertificateSum(decimal itemPriceSum)
        {
            var values = Constants.GetPriceRanges();
            decimal sum;
            var result = new ServiceResultExtension<decimal>();
            var max = values.Select(m => m.Key.Maximum).Max();
            var min = values.Select(m => m.Key.Minimum).Min();

            sum = values.SingleOrDefault(c => c.Key.IsInRange(itemPriceSum)).Value;
            if (sum == 0)
            {
                result.Message = $"Insured sum must be between " +
                    $"{min} and {max} eur sum. " +
                    $"For more datails please contact our service";
                return result;
            }
            result.Value = sum;
            return result;
        }
        private static Customer CreateCustomer(CertificateModel model)
        {
            return new Customer
            {
                Name = model.CustomerName,
                DateOfBirth = model.CustomerDateOfBirth
            };
        }
        private static Certificate CreateCertificate(CertificateModel model,decimal certificateSum)
        {
            return new Certificate
            {
                CertificateSum = certificateSum,
                InsuredItem = model.InsuredItem,
                Customer = CreateCustomer(model),
                InsuredSum = model.InsuredSum,
                CreationDate = model.CreationDate,
                ValidFrom = model.ValidFrom,
                ValidTo = model.ValidTo,
            };
        }

        private static void CreateCertificateNumber(Certificate certificate)
        {
            string format = "00000";
            certificate.Number = certificate.Id.ToString(format);
        }
    }


}
