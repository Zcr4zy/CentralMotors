using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text;
using CentralMotors.Models;

namespace CentralMotors.Web.Controllers
{
    public class TipoTransmissaoController : Controller
    {
        Uri baseAddress = new("http://localhost:5160/api");
        private readonly HttpClient _client;

        public TipoTransmissaoController()
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
            List<TipoTransmissao> tipoTransmissaos = [];
            HttpResponseMessage response = await _client.GetAsync(
                    _client.BaseAddress + "/tipostransmissao");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                tipoTransmissaos = JsonSerializer.Deserialize<List<TipoTransmissao>>(data, options);
            }
            return View(tipoTransmissaos);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(TipoTransmissao tipoTransmissao)
        {
            try
            {
                string conteudoJson = JsonSerializer.Serialize(tipoTransmissao);
                StringContent conteudostring = new(conteudoJson, Encoding.UTF8, "application/json");
                HttpResponseMessage resposta = await _client.PostAsync(
                    _client.BaseAddress + "/tipostransmissao", conteudostring);
                if (resposta.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{tipoTransmissao.Nome} Cadastrado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Problemas ao Salvar" + ex.Message;
            }
            ViewData["TipoTransmissao"] = new SelectList("TipoTransmissaoId", "Nome");
            return View(tipoTransmissao);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tipoTransmissao = await GetTipoTransmissao(id);
            if (tipoTransmissao == null)
            {
                TempData["errorMessage"] = "Tipo de Transmissão não Localizada!";
                return RedirectToAction("Index");
            }

            return View(tipoTransmissao);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TipoTransmissao tipoTransmissao)
        {
            try
            {
                string data = JsonSerializer.Serialize(tipoTransmissao);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(
                    _client.BaseAddress + "/tipostransmissao/" + tipoTransmissao.TipoTransmissaoId, content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Tipo de Transmissão Alterada com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            return View(tipoTransmissao);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var tipoTransmissao = await GetTipoTransmissao(id);
            if (tipoTransmissao == null)
            {
                TempData["errorMessage"] = "Tipo de Transmissão não Localizada!";
                return RedirectToAction("Index");
            }
            return View(tipoTransmissao);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TipoTransmissao tipoTransmissao = await GetTipoTransmissao(id);
            if (tipoTransmissao == null)
            {
                TempData["errorMessage"] = "Tipo de Transmissão não Localizada!";
                return RedirectToAction("Index");
            }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(
                    _client.BaseAddress + "/tipostransmissao/" + id
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{tipoTransmissao.TipoTransmissaoId} Excluído com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Excluir: " + ex.Message;
            }
            return View(tipoTransmissao);

        }
        #endregion

        private async Task<TipoTransmissao> GetTipoTransmissao(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/tipostransmissao/" + id
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                TipoTransmissao tipoTransmissao = JsonSerializer.Deserialize<TipoTransmissao>(data, options);
                return tipoTransmissao;
            }
            return null;
        }
    }
}
