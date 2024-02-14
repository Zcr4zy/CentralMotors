using CentralMotors.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace CentralMotors.Web.Controllers
{
    public class TipoCombustivelController : Controller
    {
        Uri baseAddress = new("http://localhost:5160/api");
        private readonly HttpClient _client;

        public TipoCombustivelController()
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
            List<TipoCombustivel> tipoCombustivels = [];
            HttpResponseMessage response = await _client.GetAsync(
                    _client.BaseAddress + "/tiposcombustivel");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                tipoCombustivels = JsonSerializer.Deserialize<List<TipoCombustivel>>(data, options);
            }
            return View(tipoCombustivels);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(TipoCombustivel tipoCombustivel)
        {
            try
            {
                string conteudoJson = JsonSerializer.Serialize(tipoCombustivel);
                StringContent conteudostring = new(conteudoJson, Encoding.UTF8, "application/json");
                HttpResponseMessage resposta = await _client.PostAsync(
                    _client.BaseAddress + "/tiposcombustivel", conteudostring);
                if (resposta.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{tipoCombustivel.Nome} Cadastrado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Problemas ao Salvar" + ex.Message;
            }
            ViewData["TipoCombustivel"] = new SelectList("TipoCombustivelId", "Nome");
            return View(tipoCombustivel);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tipoCombustivel = await GetTipoCombustivel(id);
            if (tipoCombustivel == null)
            {
                TempData["errorMessage"] = "Tipo de Combustível não Localizado!";
                return RedirectToAction("Index");
            }

            return View(tipoCombustivel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TipoCombustivel tipoCombustivel)
        {
            try
            {
                string data = JsonSerializer.Serialize(tipoCombustivel);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(
                    _client.BaseAddress + "/tiposcombustivel/" + tipoCombustivel.TipoCombustivelId, content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Tipo de Combustível Alterado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            return View(tipoCombustivel);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var tipoCombustivel = await GetTipoCombustivel(id);
            if (tipoCombustivel == null)
            {
                TempData["errorMessage"] = "Tipo de Combustível não Localizado!";
                return RedirectToAction("Index");
            }
            return View(tipoCombustivel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TipoCombustivel tipoCombustivel = await GetTipoCombustivel(id);
            if (tipoCombustivel == null)
            {
                TempData["errorMessage"] = "Tipo de Combustível não Localizado!";
                return RedirectToAction("Index");
            }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(
                    _client.BaseAddress + "/tiposcombustivel/" + id
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{tipoCombustivel.TipoCombustivelId} Excluído com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Excluir: " + ex.Message;
            }
            return View(tipoCombustivel);

        }
        #endregion

        private async Task<TipoCombustivel> GetTipoCombustivel(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/tiposcombustivel/" + id
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                TipoCombustivel tipoCombustivel = JsonSerializer.Deserialize<TipoCombustivel>(data, options);
                return tipoCombustivel;
            }
            return null;
        }
    }
}
