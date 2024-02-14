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
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Veiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            var veiculos = await _context.Veiculos
                .Include(m => m.Modelo).ThenInclude(m => m.Fabricante)
                .Include(c => c.Cor)
                .Include(ti => ti.Tipo)
                .Include(tc => tc.TipoCombustivel)
                .Include(tt => tt.TipoTransmissao)
                .ToListAsync();
            return Ok(veiculos);
        }

        [HttpGet("modelos/{id}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetByModeloId(int id)
        {
            var veiculos = await _context.Veiculos
                .Where(m => m.ModeloId == id)
                .Include(c => c.Modelo)
                .Include(a => a.Modelo.Fabricante)
                .ToListAsync();
            return Ok(veiculos);
        }

        [HttpGet("cor/{id}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetByCorId(int id)
        {
            var veiculos = await _context.Veiculos
                .Where(m => m.CorId == id)
                .Include(c => c.Cor)
                .ToListAsync();
            return Ok(veiculos);
        }

        [HttpGet("tipos/{id}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetByTipoId(int id)
        {
            var veiculos = await _context.Veiculos
                .Where(m => m.TipoId == id)
                .Include(c => c.Tipo)
                .ToListAsync();
            return Ok(veiculos);
        }

        [HttpGet("tipostransmissao/{id}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetByTipoTransmissaoId(int id)
        {
            var veiculos = await _context.Veiculos
                .Where(m => m.TipoTransmissaoId == id)
                .Include(c => c.TipoTransmissao)
                .ToListAsync();
            return Ok(veiculos);
        }

        [HttpGet("tiposcombustivel/{id}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetByTipoCombustivelId(int id)
        {
            var veiculos = await _context.Veiculos
                .Where(m => m.TipoCombustivelId == id)
                .Include(c => c.TipoCombustivel)
                .ToListAsync();
            return Ok(veiculos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos
                .Where(m => m.VeiculoId == id)
                .Include(m => m.Modelo)
                .Include(c => c.Cor)
                .Include(ti => ti.Tipo)
                .Include(tc => tc.TipoCombustivel)
                .Include(tt => tt.TipoTransmissao)
                .Include(fa => fa.Modelo.Fabricante)
                .FirstOrDefaultAsync();

            if (veiculo == null)
            {
                return NotFound();
            }

            return veiculo;
        }

        // PUT: api/Veiculos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.VeiculoId)
            {
                return BadRequest();
            }

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoExists(id))
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

        // POST: api/Veiculos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            veiculo.Cor = null;
            veiculo.Modelo = null;
            veiculo.Tipo = null;
            veiculo.TipoTransmissao = null;
            veiculo.TipoCombustivel = null;
            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVeiculo", new { id = veiculo.VeiculoId }, veiculo);
        }

        // DELETE: api/Veiculos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.VeiculoId == id);
        }
    }
}
