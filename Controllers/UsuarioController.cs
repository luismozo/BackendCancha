using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BackendCancha.Services;

namespace BackendCancha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // El controlador ahora solo depende de la INTERFAZ del servicio.
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // POST: api/Usuario/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // 1. Delega toda la lógica al servicio.
            var response = await _usuarioService.LoginAsync(dto);

            // 2. Comprueba el resultado y devuelve la respuesta apropiada.
            if (response == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            return Ok(response);
        }

        // POST: api/Usuario (Registro)
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDTO>> CrearUsuario([FromBody] UsuarioDTO dto)
        {
            // 1. Delega la lógica de creación al servicio.
            var nuevoUsuario = await _usuarioService.RegisterAsync(dto);

            // 2. Devuelve una respuesta 201 Created con el nuevo usuario.
            return CreatedAtAction(nameof(CrearUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }

        // NOTA: JUANKI Los endpoints GET, PUT, DELETE para gestionar usuarios se pueden
        // añadir aquí de la misma forma, delegando siempre al servicio.
        // Por seguridad, deben estar protegidos para que solo un Admin pueda usarlos.
    }
}
