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
    public class CorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cor>>> GetCores()
        {
            var cor= await _context.Cores.ToListAsync();
            return Ok(cor);
        }

        // GET: api/Cor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cor>> GetCor(int id)
        {
            var cor = await _context.Cores.FindAsync(id);

            if (cor == null)
            {
                return NotFound();
            }

            return cor;
        }

        // PUT: api/Cor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCor(int id, Cor cor)
        {
            if (id != cor.CorId)
            {
                return BadRequest();
            }

            _context.Entry(cor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CorExists(id))
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

        // POST: api/Cor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cor>> PostCor(Cor cor)
        {
            _context.Cores.Add(cor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCor", new { id = cor.CorId }, cor);
        }

        // DELETE: api/Cor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCor(int id)
        {
            var cor = await _context.Cores.FindAsync(id);
            if (cor == null)
            {
                return NotFound();
            }

            _context.Cores.Remove(cor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CorExists(int id)
        {
            return _context.Cores.Any(e => e.CorId == id);
        }
    }
}
