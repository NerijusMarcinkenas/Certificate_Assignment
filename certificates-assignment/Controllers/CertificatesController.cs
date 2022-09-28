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
        return await _service.GetAllCertificates();
    }

    [HttpPost]
    public async Task<ActionResult> Create(CertificateModel model)
    {
        var certificate = await _service.CreateCertificate(model);
        if (certificate.Value == null)
        {
            return BadRequest(new { certificate.Message });
        }
        return Ok(certificate);
    }
}