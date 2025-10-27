using BackendCancha.Model;
using BackendCancha.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Security.Claims;
using BackendCancha.DTO;

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

        // POST /api/reservas (Tu método original, está perfecto)
        [HttpPost]
        [Authorize] // Cualquier usuario autenticado puede crear
        public async Task<IActionResult> CreateReserva([FromBody] CreateReservaDTO dto)
        {
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

        // --- AÑADE TODOS ESTOS MÉTODOS ---

        // GET /api/reservas (Para el Admin)
        [HttpGet]
        [Authorize(Roles = "admin")] // Solo el Admin puede ver todas
        public async Task<IActionResult> GetAllReservas()
        {
            try
            {
                var reservas = await _reservaService.GetAllReservasAsync();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET /api/reservas/user/{usuarioId} (Para el cliente)
        [HttpGet("user/{usuarioId:int}")]
        [Authorize] // Usuario autenticado (sea admin o el propio usuario)
        public async Task<IActionResult> GetReservasByUser(int usuarioId)
        {
            // Verificación de seguridad: Asegura que el usuario solo pida sus propias reservas
            // (a menos que sea Admin)
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var esAdmin = User.IsInRole("Admin");

            if (!esAdmin && usuarioIdClaim != usuarioId.ToString())
            {
                return Forbid("No tienes permiso para ver las reservas de otro usuario.");
            }

            try
            {
                var reservas = await _reservaService.GetReservasByUserIdAsync(usuarioId);
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT /api/reservas/{id}/status (Para el Admin)
        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "admin")] // Solo el Admin puede cambiar estados
        public async Task<IActionResult> UpdateReservaStatus(int id, [FromBody] UpdateStatusDTO dto)
        {
            try
            {
                var reservaActualizada = await _reservaService.UpdateReservaStatusAsync(id, dto.Status);
                if (reservaActualizada == null)
                {
                    return NotFound(new { message = "Reserva no encontrada" });
                }
                return Ok(reservaActualizada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/reservas/{id} (Para el Admin)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")] // Solo el Admin puede borrar
        public async Task<IActionResult> DeleteReserva(int id)
        {
            try
            {
                var exito = await _reservaService.DeleteReservaAsync(id);
                if (!exito)
                {
                    return NotFound(new { message = "Reserva no encontrada" });
                }
                return Ok(new { message = "Reserva eliminada con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTO simple para recibir el nuevo estado (añade esto al final del archivo o en su propio archivo)
    public class UpdateStatusDTO
    {
        public string Status { get; set; }
    }
}