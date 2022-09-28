using Core.Domain;

namespace Core.Models;

public class CertificateModel
{
    public string? Number { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public DateTime CustomerDateOfBirth { get; set; }

    public string InsuredItem { get; set; } = string.Empty;

    public decimal InsuredSum { get; set; }

    public decimal CertificateSum { get; set; }

    public CertificateModel()
    {
        CreationDate = DateTime.Now;
        ValidFrom = DateTime.Now;
        ValidTo = CreationDate.AddYears(1).Date;
    }
    public CertificateModel(Certificate certificate)
    {
        Number = certificate.Number;
        CreationDate = certificate.CreationDate;
        ValidFrom = certificate.ValidFrom;
        ValidTo = certificate.ValidTo;
        CustomerName = certificate.Customer.Name;
        CustomerDateOfBirth = certificate.Customer.DateOfBirth;
        InsuredItem = certificate.InsuredItem;
        InsuredSum = certificate.InsuredSum;
        CertificateSum = certificate.CertificateSum;
    }
}