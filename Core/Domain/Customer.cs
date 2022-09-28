namespace Core.Domain;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }
}