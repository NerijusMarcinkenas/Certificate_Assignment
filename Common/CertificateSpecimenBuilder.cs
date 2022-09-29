using AutoFixture.Kernel;
using Core.Domain;
using System.Reflection;

namespace Common
{
    public class CertificateSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(Certificate))
            {
                return new Certificate
                {
                    Id = 1,
                    InsuredItem = "Test Item",                  
                    InsuredSum = 100,
                    CreationDate = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    ValidTo = DateTime.Now.AddYears(1).Date,
                    Customer = new Customer
                    {
                        Id = 1,
                        DateOfBirth = new DateTime(1999, 01, 01),
                        Name = "Test Name"
                    }
                };
            }
            return new NoSpecimen();
        }
    }
}
