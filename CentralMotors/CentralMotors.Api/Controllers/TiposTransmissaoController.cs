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
    public class TiposTransmissaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposTransmissaoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TiposTransmissao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoTransmissao>>> GetTipoTransmissoes()
        {
            return await _context.TipoTransmissoes.ToListAsync();
        }

        // GET: api/TiposTransmissao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoTransmissao>> GetTipoTransmissao(int id)
        {
            var tipoTransmissao = await _context.TipoTransmissoes.FindAsync(id);

            if (tipoTransmissao == null)
            {
                return NotFound();
            }

            return tipoTransmissao;
        }

        // PUT: api/TiposTransmissao/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoTransmissao(int id, TipoTransmissao tipoTransmissao)
        {
            if (id != tipoTransmissao.TipoTransmissaoId)
            {
                return BadRequest();
            }

            _context.Entry(tipoTransmissao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoTransmissaoExists(id))
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

        // POST: api/TiposTransmissao
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoTransmissao>> PostTipoTransmissao(TipoTransmissao tipoTransmissao)
        {
            _context.TipoTransmissoes.Add(tipoTransmissao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoTransmissao", new { id = tipoTransmissao.TipoTransmissaoId }, tipoTransmissao);
        }

        // DELETE: api/TiposTransmissao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoTransmissao(int id)
        {
            var tipoTransmissao = await _context.TipoTransmissoes.FindAsync(id);
            if (tipoTransmissao == null)
            {
                return NotFound();
            }

            _context.TipoTransmissoes.Remove(tipoTransmissao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoTransmissaoExists(int id)
        {
            return _context.TipoTransmissoes.Any(e => e.TipoTransmissaoId == id);
        }
    }
}
