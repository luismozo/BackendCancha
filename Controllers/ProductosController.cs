using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using BackendCancha.Model;

namespace BackendCancha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/Productos - Público para todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> GetProductos()
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }

        // GET: api/Productos/5 - Público para todos
        [HttpGet("{id}")]
        public async Task<ActionResult<Productos>> GetProductos(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        // POST: api/Productos - Protegido. Solo Admins.
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CrearProductos([FromBody] ProductosDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nuevoProducto = await _productoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetProductos), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        // PUT: api/Productos/5 - Protegido. Solo Admins.
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarProductos(int id, [FromBody] ProductosDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productoActualizado = await _productoService.UpdateAsync(id, dto);
            if (productoActualizado == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Productos/5 - Protegido. Solo Admins.
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarProductos(int id)
        {
            var resultado = await _productoService.DeleteAsync(id);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

