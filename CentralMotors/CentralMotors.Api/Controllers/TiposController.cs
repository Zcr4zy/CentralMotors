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
    public class TiposController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tipos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipo>>> GetTipos()
        {
            return await _context.Tipos.ToListAsync();
        }

        // GET: api/Tipos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tipo>> GetTipo(int id)
        {
            var tipo = await _context.Tipos.FindAsync(id);

            if (tipo == null)
            {
                return NotFound();
            }

            return tipo;
        }

        // PUT: api/Tipos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipo(int id, Tipo tipo)
        {
            if (id != tipo.TipoId)
            {
                return BadRequest();
            }

            _context.Entry(tipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoExists(id))
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

        // POST: api/Tipos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tipo>> PostTipo(Tipo tipo)
        {
            _context.Tipos.Add(tipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipo", new { id = tipo.TipoId }, tipo);
        }

        // DELETE: api/Tipos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            var tipo = await _context.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }

            _context.Tipos.Remove(tipo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoExists(int id)
        {
            return _context.Tipos.Any(e => e.TipoId == id);
        }
    }
}
