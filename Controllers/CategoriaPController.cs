using BackendCancha.DTO;
using BackendCancha.Model;
using BackendCancha.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BackendCancha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaPController : ControllerBase
    {
        private readonly ICategoriaPService _categoriaPService;

        public CategoriaPController(ICategoriaPService categoriaPService)
        {
            _categoriaPService = categoriaPService;
        }

        // GET: api/CategoriaP - Público, necesario para los formularios de productos.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaP>>> GetCategoriaP()
        {
            var categorias = await _categoriaPService.GetAllAsync();
            return Ok(categorias);
        }

        // GET: api/CategoriaP/5 - Público
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaP>> GetCategoriaP(int id)
        {
            var categoria = await _categoriaPService.GetByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        // POST: api/CategoriaP - Protegido. Solo Admins.
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CategoriaP>> CrearCategoriaP(CategoriaPDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nuevaCategoria = await _categoriaPService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetCategoriaP), new { id = nuevaCategoria.Id }, nuevaCategoria);
        }

        // PUT: api/CategoriaP/5 - Protegido. Solo Admins.
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarCategoriaP(int id, CategoriaPDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoriaActualizada = await _categoriaPService.UpdateAsync(id, dto);
            if (categoriaActualizada == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/CategoriaP/5 - Protegido. Solo Admins.
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarCategoriaP(int id)
        {
            var resultado = await _categoriaPService.DeleteAsync(id);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

