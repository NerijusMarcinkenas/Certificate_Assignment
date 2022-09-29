using AutoFixture;
using AutoFixture.Xunit2;
using Core.Domain;
using Core.Models;
using InsuranceCertificates.Controllers;
using InsuranceCertificates.Services.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Services;

namespace InsuranceCertificates.WebTests
{
    public class CertificatesControllerTests
    {

        private readonly Mock<ICertificateService> _serviceMock;
        private readonly CertificatesController _sut;
        private readonly IFixture _fixture;
        public CertificatesControllerTests()
        {
            _fixture = new Fixture();
            _serviceMock = new Mock<ICertificateService>();
            _sut = new CertificatesController(_serviceMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllCertificates()
        {
            var certificatesMock = _fixture.CreateMany<CertificateModel>(10).ToList();
            _serviceMock.Setup(g => g.GetAllCertificateModelsAsync()).ReturnsAsync(certificatesMock);

            var result = await _sut.Get();
            var resultList = result.ToList();

            Assert.NotNull(result);
            for (int i = 0; i < certificatesMock.Count; i++)
            {
                Assert.Equal(certificatesMock[i].Number, resultList[i].Number);
            }
            _serviceMock.Verify(g => g.GetAllCertificateModelsAsync(),Times.Once()); 
        }

        [Theory,AutoData]
        public async Task Create_WhenModelIsNotValidReturnsBadRequest(CertificateModel model)
        {
            model.CustomerDateOfBirth = DateTime.Now;
            var certificateResult = new ServiceResultExtension<Certificate> { Value = null, Message = It.IsAny<string>()};

            _serviceMock.Setup(c => c.CreateCertificate(model)).Returns(certificateResult);

            var result = await _sut.Create(model);
            var resultAsBadRequest = result as BadRequestObjectResult;

            Assert.Equal(400,resultAsBadRequest.StatusCode);
            _serviceMock.Verify(c => c.CreateCertificate(model), Times.Once());
            _serviceMock.Verify(s => s.SaveToDbAsync(certificateResult.Value), Times.Never);
        }

        [Theory, AutoData]
        public async Task Create_WhenModelIsValidReturnsStatusOk(CertificateModel model, string message)
        {
            model.CustomerDateOfBirth = new DateTime(1999, 01, 01);
            model.InsuredSum = 100;
            var certificateResult = new ServiceResultExtension<Certificate> { Value = new Certificate(), Message = message};

            _serviceMock.Setup(c => c.CreateCertificate(model)).Returns(certificateResult);

            var result = await _sut.Create(model);
            var asStatusCoreResult = result as StatusCodeResult;

            Assert.Equal(200, asStatusCoreResult.StatusCode);
            _serviceMock.Verify(c => c.CreateCertificate(model), Times.Once());
            _serviceMock.Verify(s => s.SaveToDbAsync(certificateResult.Value), Times.Once);
        }
    }
}