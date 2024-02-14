using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CentralMotors.Api.Data;
using CentralMotors.Models;

namespace CentralMotors.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Modelos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Modelo>>> GetModelos()
        {
            var modelos = await _context.Modelos
                .Include(m => m.Fabricante)
                .ToListAsync();
            return Ok(modelos);
        }

        [HttpGet("fabricante/{id}")]
        public async Task<ActionResult<IEnumerable<Modelo>>> GetByMarcaId(int id)
        {
            var modelos = await _context.Modelos
                .Where(m => m.FabricanteId == id)
                .Include(m => m.Fabricante)
                .ToListAsync();
            return Ok(modelos);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Modelo>> GetModelo(int id)
        {
            var modelo = await _context.Modelos
             .Where(m => m.ModeloId == id)
             .Include(m => m.Fabricante)
             .FirstOrDefaultAsync();

            if (modelo == null)
            {
                return NotFound();
            }

            return modelo;
        }

        // PUT: api/Modelos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModelo(int id, Modelo modelo)
        {
            if (id != modelo.ModeloId)
            {
                return BadRequest();
            }

            _context.Entry(modelo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeloExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Modelos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Modelo>> PostModelo(Modelo modelo)
        {
            modelo.Fabricante = null;
            _context.Modelos.Add(modelo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModelo", new { id = modelo.ModeloId }, modelo);
        }

        // DELETE: api/Modelos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModelo(int id)
        {
            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            _context.Modelos.Remove(modelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeloExists(int id)
        {
            return _context.Modelos.Any(e => e.ModeloId == id);
        }
    }
}
