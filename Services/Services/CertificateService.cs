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

        public async Task<ServiceResultExtension<CertificateModel>> CreateCertificate(CertificateModel model)
        {
            var validatedInputs = ValidateInputs(model);
            if (validatedInputs.Value == null)
            {
                return validatedInputs;
            }
            var certificateSum = GetCertificateSum(model.InsuredSum);
            var createdCerificateModel = new ServiceResultExtension<CertificateModel>();
            if (certificateSum.Value == 0)
            {
                createdCerificateModel.Message = certificateSum.Message;
                return createdCerificateModel;
            }
            var certificate = CreateCertificateEntity(model, certificateSum.Value);

            return await AddToDb(certificate);
        }

        public async Task<List<CertificateModel>> GetAllCertificates()
        {
            var certificates = await _dbRepository.GetAllCertificates();
            return certificates.Select(c => new CertificateModel(c)).ToList();
        }
       
        private async Task<ServiceResultExtension<CertificateModel>> AddToDb(Certificate certificate)
        {
            var addedCertificate = _dbRepository.AddNew(certificate);
            CreateCertificateNumber(addedCertificate);
            await _dbRepository.CommitAsync();

            var createdCerificateModel = new ServiceResultExtension<CertificateModel>
            {
                Value = new CertificateModel(certificate),
                Message = "Certificate created successfully"
            };
            return createdCerificateModel;
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

        private static Certificate CreateCertificateEntity(CertificateModel model,decimal certificateSum)
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
