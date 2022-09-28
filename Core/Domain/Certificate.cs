namespace Core.Domain;

public class Certificate
{
    public int Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public Customer Customer { get; set; } = new Customer();

    public string InsuredItem { get; set; } = string.Empty;

    public decimal InsuredSum { get; set; }

    public decimal CertificateSum { get; set; }
}