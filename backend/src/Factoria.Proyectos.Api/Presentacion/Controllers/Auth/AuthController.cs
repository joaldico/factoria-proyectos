using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Factoria.Proyectos.Api.Infraestructura.Data;
using Factoria.Proyectos.Api.Infraestructura.Servicios;
using Factoria.Proyectos.Api.Shared.Dto.Auth;
using Factoria.Proyectos.Api.Shared.Dto.Genericas;
using Factoria.Proyectos.Api.Shared.Models.Seguridad;

namespace Factoria.Proyectos.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PBKDF2 _crypto;
        private readonly TokenService _tokenService;
        private readonly ErrorService _errorService;

        public AuthController(
            ApplicationDbContext context, 
            PBKDF2 crypto, 
            TokenService tokenService, 
            ErrorService errorService)
        {
            _context = context;
            _crypto = crypto;
            _tokenService = tokenService;
            _errorService = errorService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => (u.CodigoUsuario == request.Usuario || u.Correo == request.Usuario) && u.Activo);

                if (usuario == null)
                {
                    return BadRequest(new RespuestaSimpleBE { rpt = 1, mensaje = "Credenciales incorrectas.", data = null });
                }

                string hashEntrante = _crypto.GenerarKeySecret(request.Password, usuario.Salt);
                if (usuario.ContrasenaHash != hashEntrante)
                {
                    return BadRequest(new RespuestaSimpleBE { rpt = 1, mensaje = "Credenciales incorrectas.", data = null });
                }

                var perfilesList = await _context.UsuarioPerfiles
                    .Where(up => up.UsuarioId == usuario.UsuarioId && up.Perfil.Activo)
                    .Select(up => up.Perfil.NombrePerfil)
                    .ToListAsync();

                var modulosList = await _context.PerfilModulos
                    .Where(pm => pm.Perfil.UsuarioPerfiles.Any(up => up.UsuarioId == usuario.UsuarioId) && pm.Modulo.Activo)
                    .Select(pm => new MenuDto
                    {
                        CodigoModulo = pm.Modulo.CodigoModulo,
                        NombreModulo = pm.Modulo.NombreModulo,
                        RutaFrontend = pm.Modulo.RutaFrontend
                    })
                    .Distinct()
                    .ToListAsync();

                string jwtToken = _tokenService.GenerarJwt(
                    usuario.UsuarioId.ToString(), 
                    usuario.CodigoUsuario, 
                    usuario.NombreCompleto
                );
                
                string refreshToken = _tokenService.GenerarRefreshToken();

                var nuevoToken = new RefreshToken
                {
                    UsuarioId = usuario.UsuarioId,
                    Token = refreshToken,
                    FechaExpiracion = DateTime.UtcNow.AddDays(7),
                    Origen = "Web Local / Host",
                    IndicadorMfaValidado = "1"
                };

                _context.RefreshTokens.Add(nuevoToken);
                await _context.SaveChangesAsync();

                var response = new LoginResponseDto
                {
                    UsuarioId = usuario.UsuarioId,
                    CodigoUsuario = usuario.CodigoUsuario,
                    NombreCompleto = usuario.NombreCompleto,
                    Correo = usuario.Correo,
                    EmpresaId = usuario.EmpresaId,
                    Token = jwtToken,
                    RefreshToken = refreshToken,
                    RequiereMfa = usuario.RequiereMfa,
                    EstadoSecretMfa = usuario.EstadoSecretMfa,
                    Perfiles = perfilesList,
                    Modulos = modulosList
                };

                return Ok(new RespuestaSimpleBE { rpt = 0, mensaje = "Login exitoso.", data = response });
            }
            catch (Exception ex)
            {
                return _errorService.Handle(ex);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto request)
        {
            try
            {
                var tokenBd = await _context.RefreshTokens
                    .Include(rt => rt.Usuario)
                    .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

                if (tokenBd == null || tokenBd.FechaExpiracion < DateTime.UtcNow)
                {
                    return Unauthorized(new RespuestaSimpleBE { rpt = -1, mensaje = "Refresh token inválido o expirado.", data = null });
                }

                string nuevoJwt = _tokenService.GenerarJwt(
                    tokenBd.UsuarioId.ToString(), 
                    tokenBd.Usuario.CodigoUsuario, 
                    tokenBd.Usuario.NombreCompleto
                );
                string nuevoRefreshToken = _tokenService.GenerarRefreshToken();

                _context.RefreshTokens.Remove(tokenBd);

                var nuevoRegistroToken = new RefreshToken
                {
                    Token = nuevoRefreshToken,
                    UsuarioId = tokenBd.UsuarioId,
                    FechaExpiracion = DateTime.UtcNow.AddDays(7),
                    Origen = tokenBd.Origen,
                    IndicadorMfaValidado = tokenBd.IndicadorMfaValidado
                };

                _context.RefreshTokens.Add(nuevoRegistroToken);
                await _context.SaveChangesAsync();

                return Ok(new RespuestaSimpleBE 
                { 
                    rpt = 0, 
                    mensaje = "Token renovado exitosamente.", 
                    data = new { token = nuevoJwt, refreshToken = nuevoRefreshToken } 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RespuestaSimpleBE { rpt = -1, mensaje = "Error interno al renovar la sesión.", detalleError = ex.Message });
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("generar-credenciales-prueba/{password}")]
        public IActionResult GenerarCredencialesPrueba(string password)
        {
            var saltGenerado = _crypto.GenerarSalt();
            var hashGenerado = _crypto.GenerarKeySecret(password, saltGenerado);
            
            return Ok(new 
            { 
                Mensaje = "Actualizar valores en DB",
                PasswordPlano = password, 
                Salt = saltGenerado, 
                ContrasenaHash = hashGenerado 
            });
        }
    }
}