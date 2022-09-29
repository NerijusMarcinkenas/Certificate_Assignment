using Core.Domain;
using Core.Models;
using InsuranceCertificates.Services;
using InsuranceCertificates.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    internal static class ServiceExtension
    {
        internal static ServiceResultExtension<CertificateModel> ValidateInputs(CertificateModel model)
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

        internal static ServiceResultExtension<decimal> GetCertificateSum(decimal itemPriceSum)
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

        internal static Customer CreateCustomer(CertificateModel model)
        {
            return new Customer
            {
                Name = model.CustomerName,
                DateOfBirth = model.CustomerDateOfBirth
            };
        }

        internal static Certificate CreateCertificateEntity(CertificateModel model, decimal certificateSum)
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

        internal static void CreateCertificateNumber(Certificate certificate)
        {
            string format = "00000";
            certificate.Number = certificate.Id.ToString(format);
        }
    }
}
