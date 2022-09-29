using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace InsuranceCertificates.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificatesController : ControllerBase
{    
    private readonly ICertificateService _service;

    public CertificatesController(ICertificateService service)
    {       
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<CertificateModel>> Get()
    {        
        return await _service.GetAllCertificateModelsAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CertificateModel model)
    {
        var certificate = _service.CreateCertificate(model);
        if (certificate.Value == null)
        {
            return BadRequest(new { certificate.Message });
        }

        await _service.SaveToDbAsync(certificate.Value);
        return Ok();
    }
}