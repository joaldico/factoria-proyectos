using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Factoria.Proyectos.Api.Shared.Dto.Genericas;

namespace Factoria.Proyectos.Api.Infraestructura.Servicios
{
    public class ErrorService
    {
        private readonly ArchivoTexto _log;

        public ErrorService(ArchivoTexto log)
        {
            _log = log;
        }

        public IActionResult Handle(Exception ex)
        {
            _log.GenerarArchivo(ex);

            if (ex is DbUpdateException dbEx && dbEx.InnerException != null)
            {
                var msg = dbEx.InnerException.Message.ToLower();

                if (msg.Contains("duplicate key value violates unique constraint"))
                {
                    return CrearRespuestaControlada("Ya existe un registro con esta información exacta en el sistema.");
                }
            }

            return new ObjectResult(new RespuestaSimpleBE
            {
                rpt = 500,
                mensaje = "Ocurrió un error interno en el servidor. Por favor, contacte a soporte técnico.",
                data = null,
                detalleError = ex.ToString() 
            })
            { StatusCode = 500 };
        }

        private IActionResult CrearRespuestaControlada(string mensajeUsuario)
        {
            return new BadRequestObjectResult(new RespuestaSimpleBE
            {
                rpt = 1, 
                mensaje = mensajeUsuario,
                data = null
            });
        }
    }
}