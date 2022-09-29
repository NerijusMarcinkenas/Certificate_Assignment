using AutoFixture;
using AutoFixture.Xunit2;
using Common;
using Core.Domain;
using Core.Models;
using DataAccess.Repositories;
using Moq;
using Services.Services;

namespace ServiceTests
{
    public class ValidationServiceTests
    {
        private readonly Mock<IDbRepository<Certificate>> _dbRepositoryMock;
        private readonly IFixture _fixture;
        private readonly ICertificateService _sut; // Subbject Under Tests

        public ValidationServiceTests()
        {
            _dbRepositoryMock = new Mock<IDbRepository<Certificate>>();
            _fixture = new Fixture();
            _sut = new CertificateService(_dbRepositoryMock.Object);
        }

        [Theory, AutoData]
        public void CreateCertificate_WhenUserIsNotValidAge_ReturnsResultValueNullAndMessage(CertificateModel model)
        {
            model.CustomerDateOfBirth = DateTime.Now;
            var result = _sut.CreateCertificate(model);

            Assert.Null(result.Value);
            Assert.Equal($"User must be at least 18 years old", result.Message);
        }

        [Theory]
        [InlineData(19.99)]
        [InlineData(201.01)]
        public void CreateCertificate_WhenInsuredSumIsOutOfRange_ReturnsResultValueNullAndMessage(decimal insuredSum)
        {
            var modelMock = _fixture.Build<CertificateModel>()
                .With(d => d.CustomerDateOfBirth, new DateTime(1999, 01, 01))
                .Without(d => d.CertificateSum)
                .Create();

            modelMock.InsuredSum = insuredSum;

            var result = _sut.CreateCertificate(modelMock);

            Assert.Null(result.Value);
            Assert.Equal("Insured sum must be between " +
                    $"20 and 200 eur sum. " +
                    $"For more datails please contact our service", result.Message);
        }

        [Theory]
        [InlineData(20.09, 8)]
        [InlineData(50.09, 15)]
        [InlineData(100.09, 25)]
        public  void CreateCertificate_WhenInsuredSumIsValid_ReturnsResultValueCertificate(decimal insuredSum, decimal certificateSum)
        {
            var modelMock = _fixture.Build<CertificateModel>()
                .With(d => d.CustomerDateOfBirth, new DateTime(1999, 01, 01))
                .With(i => i.InsuredSum, insuredSum)
                .Without(d => d.CertificateSum)
                .Create();

            var result = _sut.CreateCertificate(modelMock).Value;

            Assert.NotNull(result);
            Assert.Equal(modelMock.InsuredItem, result.InsuredItem);
            Assert.Equal(modelMock.CustomerDateOfBirth, result.Customer.DateOfBirth);
            Assert.Equal(modelMock.InsuredSum, result.InsuredSum);
            Assert.Equal(modelMock.CreationDate, result.CreationDate);
            Assert.Equal(modelMock.ValidFrom, result.ValidFrom);
            Assert.Equal(modelMock.ValidTo, result.ValidTo);
            Assert.Equal(certificateSum, result.CertificateSum);

        }

        [Theory, ValidCertificate]
        public async Task SaveToDb_ReturnsCertificateWithIdAndNumber(Certificate certificate)
        {
            _dbRepositoryMock.Setup(a => a.AddNew(certificate)).Returns(certificate);

            var result = await _sut.SaveToDbAsync(certificate);

            Assert.NotNull(result.Value);
            Assert.Equal("00001", result.Value.Number);
            Assert.Equal("Certificate created successfully", result.Message);
            _dbRepositoryMock.Verify(a => a.AddNew(certificate), Times.Once);
            _dbRepositoryMock.Verify(c => c.CommitAsync(), Times.Once);
        }


        [Fact]
        public async Task GetAllCertificateModels_ReturnsMapedCertificates()
        {
            var certificatesMock = _fixture.CreateMany<Certificate>(10).ToList();
            _dbRepositoryMock.Setup(g => g.GetAllCertificates()).ReturnsAsync(certificatesMock);

            var result = await _sut.GetAllCertificateModelsAsync();

            for (int i = 0; i < certificatesMock.Count; i++)
            {
                Assert.IsType<CertificateModel>(result[i]);
                Assert.Equal(certificatesMock[i].Number, result[i].Number);
            }
            _dbRepositoryMock.Verify(g => g.GetAllCertificates(), Times.Once);
        }


    }
}