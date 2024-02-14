using CentralMotors.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Text;
using System.Text.Json;

namespace CentralMotors.Web.Controllers
{
    public class VeiculoController : Controller
    {
        Uri baseAddress = new("http://localhost:5160/api");
        private readonly HttpClient _client;

        public VeiculoController()
        {
            _client = new HttpClient
            {
                BaseAddress = baseAddress,
            };
        }

        #region List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Veiculo> veiculos = [];
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/veiculos"
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                veiculos = JsonSerializer.Deserialize<List<Veiculo>>(data, options);
            }
            return View(veiculos);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Modelos"] = new SelectList(await GetModelos(), "ModeloId", "Nome", "FabricanteId");
            ViewData["Tipos"] = new SelectList(await GetTipos(), "TipoId", "Nome");
            ViewData["TiposTransmissao"] = new SelectList(await GetTiposTransmissao(), "TipoTransmissaoId", "Nome");
            ViewData["TiposCombustivel"] = new SelectList(await GetTiposCombustivel(), "TipoCombustivelId", "Nome");
            ViewData["Cores"] = new SelectList(await GetCores(), "CorId", "Nome");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Veiculo veiculo)
        {
            try
            {
                string data = JsonSerializer.Serialize(veiculo);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(
                    _client.BaseAddress + "/veiculos", content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Veículo Cadastrado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            ViewData["Modelos"] = new SelectList(await GetModelos(), "ModeloId", "Nome", "FabricanteId");
            ViewData["Tipos"] = new SelectList(await GetTipos(), "TipoId", "Nome");
            ViewData["TiposTransmissao"] = new SelectList(await GetTiposTransmissao(), "TipoTransmissaoId", "Nome");
            ViewData["TiposCombustivel"] = new SelectList(await GetTiposCombustivel(), "TipoCombustivelId", "Nome");
            ViewData["Cores"] = new SelectList(await GetCores(), "CorId", "Nome");
            return View(veiculo);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var veiculos = await GetVeiculos(id);
            if (veiculos == null)
            {
                TempData["errorMessage"] = "Veículo não Localizado!";
                return RedirectToAction("Index");
            }
            ViewData["Modelos"] = new SelectList(await GetModelos(), "ModeloId", "Nome", "FabricanteId");
            ViewData["Tipos"] = new SelectList(await GetTipos(), "TipoId", "Nome");
            ViewData["TiposTransmissao"] = new SelectList(await GetTiposTransmissao(), "TipoTransmissaoId", "Nome");
            ViewData["TiposCombustivel"] = new SelectList(await GetTiposCombustivel(), "TipoCombustivelId", "Nome");
            ViewData["Cores"] = new SelectList(await GetCores(), "CorId", "Nome");
            return View(veiculos);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Veiculo veiculo)
        {
            try
            {
                string data = JsonSerializer.Serialize(veiculo);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(
                    _client.BaseAddress + "/veiculos/" + veiculo.VeiculoId, content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Veículo Alterado com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            ViewData["Modelos"] = new SelectList(await GetModelos(), "ModeloId", "Nome", "FabricanteId");
            ViewData["Tipos"] = new SelectList(await GetTipos(), "TipoId", "Nome");
            ViewData["TiposTransmissao"] = new SelectList(await GetTiposTransmissao(), "TipoTransmissaoId", "Nome");
            ViewData["TiposCombustivel"] = new SelectList(await GetTiposCombustivel(), "TipoCombustivelId", "Nome");
            ViewData["Cores"] = new SelectList(await GetCores(), "CorId", "Nome");
            return View(veiculo);
        }

        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var veiculo = await GetVeiculos(id);
            if (veiculo == null)
            {
                TempData["errorMessage"] = "Veículo não Localizado!";
                return RedirectToAction("Index");
            }
            return View(veiculo);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Veiculo veiculo = await GetVeiculos(id);
            if (veiculo == null)
            {
                TempData["errorMessage"] = "Veículo não Localizado!";
                return RedirectToAction("Index");
            }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(
                    _client.BaseAddress + "/veiculos/" + id
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Veículo Excluído com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Excluir: " + ex.Message;
            }
            return View(veiculo);
        }
        #endregion


        private async Task<List<Modelo>> GetModelos()
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/modelos"
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                List<Modelo> modelos = JsonSerializer.Deserialize<List<Modelo>>(data, options);
                return modelos;
            }
            return null;
        }

        private async Task<List<Tipo>> GetTipos()
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/tipos"
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                List<Tipo> tipos = JsonSerializer.Deserialize<List<Tipo>>(data, options);
                return tipos;
            }
            return null;
        }

        private async Task<List<TipoTransmissao>> GetTiposTransmissao()
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/tipostransmissao"
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                List<TipoTransmissao> tipostransmissao = JsonSerializer.Deserialize<List<TipoTransmissao>>(data, options);
                return tipostransmissao;
            }
            return null;
        }

        private async Task<List<TipoCombustivel>> GetTiposCombustivel()
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/tiposcombustivel"
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                List<TipoCombustivel> tiposcombustivel = JsonSerializer.Deserialize<List<TipoCombustivel>>(data, options);
                return tiposcombustivel;
            }
            return null;
        }

        private async Task<List<Cor>> GetCores()
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/cor"
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                List<Cor> cores = JsonSerializer.Deserialize<List<Cor>>(data, options);
                return cores;
            }
            return null;
        }

        private async Task<Veiculo> GetVeiculos(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/veiculos/" + id
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                Veiculo veiculo = new();
                veiculo = JsonSerializer.Deserialize<Veiculo>(data, options);
                return veiculo;
            }
            return null;
        }
    }


}
