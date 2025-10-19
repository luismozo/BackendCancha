using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;
using BackendCancha.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendCancha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanchasController : ControllerBase
    {
        private readonly ICanchaService _canchaService;

        public CanchasController(ICanchaService canchaService)
        {
            _canchaService = canchaService;
        }

        // GET: api/Canchas - Público para todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Canchas>>> ObtenerTodasLasCanchas()
        {
            var canchas = await _canchaService.GetAllAsync();
            return Ok(canchas);
        }

        // GET: api/Canchas/5 - Público para todos
        [HttpGet("{id}")]
        public async Task<ActionResult<Canchas>> GetCanchas(int id)
        {
            var cancha = await _canchaService.GetByIdAsync(id);
            if (cancha == null)
                return NotFound();

            return Ok(cancha);
        }

        // POST: api/Canchas - MEJORA: Protegido. Solo Admins.
        [HttpPost]
        [Authorize(Roles = "admin")] // Solo los usuarios con el rol "Admin" en su token pueden acceder.
        public async Task<ActionResult> CrearCanchas([FromBody] CanchasDTO dto)
        {
            var nuevaCancha = await _canchaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetCanchas), new { id = nuevaCancha.Id }, nuevaCancha);
        }

        // PUT: api/Canchas/5 - MEJORA: Protegido y seguro. Solo Admins.
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarCanchas(int id, [FromBody] CanchasDTO dto)
        {
            var canchaActualizada = await _canchaService.UpdateAsync(id, dto);
            if (canchaActualizada == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Canchas/5 - MEJORA: Protegido y seguro. Solo Admins.
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarCanchas(int id)
        {
            var resultado = await _canchaService.DeleteAsync(id);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
