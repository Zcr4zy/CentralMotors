using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text;
using CentralMotors.Models;

namespace CentralMotors.Web.Controllers
{
    public class TipoController : Controller
    {
        Uri baseAddress = new("http://localhost:5160/api");
        private readonly HttpClient _client;

        public TipoController()
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
            List<Tipo> tipos = [];
            HttpResponseMessage response = await _client.GetAsync(
                    _client.BaseAddress + "/tipos");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                tipos = JsonSerializer.Deserialize<List<Tipo>>(data, options);
            }
            return View(tipos);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Tipo tipo)
        {
            try
            {
                string conteudoJson = JsonSerializer.Serialize(tipo);
                StringContent conteudostring = new(conteudoJson, Encoding.UTF8, "application/json");
                HttpResponseMessage resposta = await _client.PostAsync(
                    _client.BaseAddress + "/tipos", conteudostring);
                if (resposta.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{tipo.Nome} Cadastrado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Problemas ao Salvar" + ex.Message;
            }
            ViewData["Tipo"] = new SelectList("TipoId", "Nome");
            return View(tipo);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tipo = await GetTipo(id);
            if (tipo == null)
            {
                TempData["errorMessage"] = "Tipo de Automóvel não Localizado!";
                return RedirectToAction("Index");
            }

            return View(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Tipo tipo)
        {
            try
            {
                string data = JsonSerializer.Serialize(tipo);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(
                    _client.BaseAddress + "/tipos/" + tipo.TipoId, content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Tipo de Automóvel Alterado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            return View(tipo);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var tipo = await GetTipo(id);
            if (tipo == null)
            {
                TempData["errorMessage"] = "Tipo de Automóvel não Localizado!";
                return RedirectToAction("Index");
            }
            return View(tipo);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Tipo tipo = await GetTipo(id);
            if (tipo == null)
            {
                TempData["errorMessage"] = "Tipo de Automóvel não Localizado!";
                return RedirectToAction("Index");
            }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(
                    _client.BaseAddress + "/tipos/" + id
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{tipo.TipoId} Excluído com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Excluir: " + ex.Message;
            }
            return View(tipo);

        }
        #endregion

        private async Task<Tipo> GetTipo(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/tipos/" + id
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                Tipo tipo = JsonSerializer.Deserialize<Tipo>(data, options);
                return tipo;
            }
            return null;
        }
    }
}
