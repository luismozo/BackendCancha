using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;
using BackendCancha.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Security.Claims;

namespace BackendCancha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost]
        [Authorize] // Cualquier usuario autenticado (cliente o admin) puede crear una reserva.
        public async Task<IActionResult> CreateReserva([FromBody] CreateReservaDTO dto)
        {
            // Obtenemos el ID del usuario de forma segura desde el token JWT.
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (usuarioIdClaim == null || !int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return Unauthorized();
            }

            try
            {
                var nuevaReserva = await _reservaService.CreateReservaAsync(dto, usuarioId);
                return Ok(new { message = "Reserva creada con éxito", reservaId = nuevaReserva.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}



