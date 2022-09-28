using AutoFixture;
using AutoFixture.Xunit2;
using Core.Domain;
using Core.Models;
using DataAccess.Repositories;
using InsuranceCertificates.Services.Common;
using Moq;
using Services.Services;

namespace ServiceTests
{
    public class ValidationServiceTests
    {
        private readonly Mock<IDbRepository<Certificate>> _dbRepositoryMock;
        private readonly IFixture _fixture;
        private readonly ICertificateService _sut; // Subbject under tests

        public ValidationServiceTests()
        {
            _dbRepositoryMock = new Mock<IDbRepository<Certificate>>();
            _fixture = new Fixture();
            _sut = new CertificateService(_dbRepositoryMock.Object);

        }

        [Theory, AutoData]
        public async Task CreateCertificate_WhenUserIsNotValidAge_ReturnsResultValueNullAndMessage(CertificateModel model)
        {
            model.CustomerDateOfBirth = DateTime.Now;    
            var result = await _sut.CreateCertificate(model);

            Assert.Null(result.Value);
            Assert.Equal($"User must be at least 18 years old",result.Message);

        }

        [Theory]
        [InlineData(19.99)]
        [InlineData(201.01)]        
        public async Task CreateCertificate_WhenInsuredSumIsOutOfRange_ReturnsResultValueNullAndMessage(decimal insuredSum)
        {           
            var modelMock = _fixture.Build<CertificateModel>()
                .With(d => d.CustomerDateOfBirth, new DateTime(1999, 01, 01))
                .Without(d => d.CertificateSum)
                .Create();

            modelMock.InsuredSum = insuredSum;

            var result = await _sut.CreateCertificate(modelMock);

            Assert.Null(result.Value);
            Assert.Equal("Insured sum must be between " +
                    $"20 and 200 eur sum. " +
                    $"For more datails please contact our service", result.Message);

        }
    }
}