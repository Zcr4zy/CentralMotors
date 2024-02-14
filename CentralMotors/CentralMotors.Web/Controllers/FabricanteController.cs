using CentralMotors.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace CentralMotors.Web.Controllers
{
    public class FabricanteController : Controller
    {

        Uri baseAddress = new("http://localhost:5160/api");
        private readonly HttpClient _client;

        public FabricanteController()
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
            List<Fabricante> fabricantes = [];
            HttpResponseMessage response = await _client.GetAsync(
                    _client.BaseAddress + "/fabricantes");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                fabricantes = JsonSerializer.Deserialize<List<Fabricante>>(data, options);
            }
            return View(fabricantes);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Fabricante fabricante)
        {
            try
            {
                string conteudoJson = JsonSerializer.Serialize(fabricante);
                StringContent conteudostring = new(conteudoJson, Encoding.UTF8, "application/json");
                HttpResponseMessage resposta = await _client.PostAsync(
                    _client.BaseAddress + "/fabricantes", conteudostring);
                if (resposta.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{fabricante.Nome} Cadastrado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Problemas ao Salvar" + ex.Message;
            }
            ViewData["Fabricante"] = new SelectList("FabricanteId", "Nome");
            return View(fabricante);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var fabricante = await GetFabricante(id);
            if (fabricante == null)
            {
                TempData["errorMessage"] = "Fabricante não Localizado!";
                return RedirectToAction("Index");
            }

            return View(fabricante);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Fabricante fabricante)
        {
            try
            {
                string data = JsonSerializer.Serialize(fabricante);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(
                    _client.BaseAddress + "/fabricantes/" + fabricante.FabricanteId, content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Fabricante Alterado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            return View(fabricante);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var fabricante = await GetFabricante(id);
            if (fabricante == null)
            {
                TempData["errorMessage"] = "Fabricante não Localizado!";
                return RedirectToAction("Index");
            }
            return View(fabricante);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Fabricante fabricante = await GetFabricante(id);
            if (fabricante == null)
            {
                TempData["errorMessage"] = "Fabricante não Localizado!";
                return RedirectToAction("Index");
            }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(
                    _client.BaseAddress + "/fabricantes/" + id
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{fabricante.FabricanteId} Excluído com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Excluir: " + ex.Message;
            }
            return View(fabricante);

        }
        #endregion

        private async Task<Fabricante> GetFabricante(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/fabricantes/" + id
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                Fabricante fabricante = JsonSerializer.Deserialize<Fabricante>(data, options);
                return fabricante;
            }
            return null;
        }
    }
}
