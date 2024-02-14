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
    public class TiposCombustivelController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposCombustivelController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TiposCombustivel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCombustivel>>> GetTipoCombustiveis()
        {
            return await _context.TipoCombustiveis.ToListAsync();
        }

        // GET: api/TiposCombustivel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoCombustivel>> GetTipoCombustivel(int id)
        {
            var tipoCombustivel = await _context.TipoCombustiveis.FindAsync(id);

            if (tipoCombustivel == null)
            {
                return NotFound();
            }

            return tipoCombustivel;
        }

        // PUT: api/TiposCombustivel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoCombustivel(int id, TipoCombustivel tipoCombustivel)
        {
            if (id != tipoCombustivel.TipoCombustivelId)
            {
                return BadRequest();
            }

            _context.Entry(tipoCombustivel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoCombustivelExists(id))
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

        // POST: api/TiposCombustivel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoCombustivel>> PostTipoCombustivel(TipoCombustivel tipoCombustivel)
        {
            _context.TipoCombustiveis.Add(tipoCombustivel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoCombustivel", new { id = tipoCombustivel.TipoCombustivelId }, tipoCombustivel);
        }

        // DELETE: api/TiposCombustivel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoCombustivel(int id)
        {
            var tipoCombustivel = await _context.TipoCombustiveis.FindAsync(id);
            if (tipoCombustivel == null)
            {
                return NotFound();
            }

            _context.TipoCombustiveis.Remove(tipoCombustivel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoCombustivelExists(int id)
        {
            return _context.TipoCombustiveis.Any(e => e.TipoCombustivelId == id);
        }
    }
}
