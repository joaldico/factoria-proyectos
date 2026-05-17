using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Factoria.Proyectos.Api.Shared.Dto; 
using Factoria.Proyectos.Api.Shared.Dto.General; 
using Factoria.Proyectos.Api.Shared.Dto.Genericas; 
using Factoria.Proyectos.Api.Infraestructura.Servicios;

namespace Factoria.Proyectos.Api.Controllers.General;

[Authorize(Policy = "AccesoSeguroPodoestetik")]
[ApiController]
[Route("[controller]")]
public class ArchivoController : ControllerBase
{
    private readonly AwsS3Helper _s3Helper;
    private readonly UtilsService _utils;
    private readonly ErrorService _errorService;

    public ArchivoController(AwsS3Helper s3Helper, UtilsService utils, ErrorService errorService)
    {
        _s3Helper = s3Helper;
        _utils = utils;
        _errorService = errorService;
    }

    [HttpPost("subir-base64")]
    public async Task<IActionResult> SubirBase64([FromBody] UploadBase64RequestDto request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Base64))
                return BadRequest(new RespuestaBE<string> { rpt = 1, mensaje = "El archivo Base64 está vacío", data = null });

            int empresaId = _utils.ObtenerEmpresaIdDelToken();

            string urlPublica = await _s3Helper.UploadBase64(
                request.Base64, 
                request.TipoArchivo, 
                request.Folder, 
                empresaId
            );

            return Ok(new RespuestaBE<string> { rpt = 0, mensaje = "Archivo subido correctamente", data = urlPublica });
        }
        catch (Exception ex) { return _errorService.Handle(ex); }
    }

    [HttpPost("subir-multiples-base64")]
    public async Task<IActionResult> SubirMultiplesBase64([FromBody] UploadMultiplesBase64RequestDto request)
    {
        try
        {
            if (request.Archivos == null || !request.Archivos.Any())
                return BadRequest(new RespuestaBE<List<string>> { rpt = 1, mensaje = "No se enviaron archivos", data = null });

            int empresaId = _utils.ObtenerEmpresaIdDelToken();
            var urlsSubidas = new List<string>();

            foreach (var archivo in request.Archivos)
            {
                string url = await _s3Helper.UploadBase64(
                    archivo.Base64, 
                    archivo.TipoArchivo, 
                    archivo.Folder, 
                    empresaId
                );
                urlsSubidas.Add(url);
            }

            return Ok(new RespuestaBE<List<string>> { rpt = 0, mensaje = "Archivos subidos correctamente", data = urlsSubidas });
        }
        catch (Exception ex) { return _errorService.Handle(ex); }
    }
}