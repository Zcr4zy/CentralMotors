using CentralMotors.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace CentralMotors.Web.Controllers
{
    public class ModeloController : Controller
    {
        Uri baseAddress = new("http://localhost:5160/api");
        private readonly HttpClient _client;

        public ModeloController()
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
            List<Modelo> modelos = [];
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
                modelos = JsonSerializer.Deserialize<List<Modelo>>(data, options);
            }
            return View(modelos);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Fabricantes"] = new SelectList(await GetFabricantes(), "FabricanteId", "Nome");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Modelo modelo)
        {
            try
            {
                string data = JsonSerializer.Serialize(modelo);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(
                    _client.BaseAddress + "/modelos", content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{modelo.Nome} Cadastrado com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            ViewData["Fabricantes"] = new SelectList(await GetFabricantes(), "FabricanteId", "Nome");
            return View(modelo);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var modelo = await GetModelo(id);
            if (modelo == null)
            {
                TempData["errorMessage"] = "Modelo não Localizado!";
                return RedirectToAction("Index");
            }
            ViewData["Fabricantes"] = new SelectList(await GetFabricantes(), "FabricanteId", "Nome");
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Modelo modelo)
        {
            try
            {
                string data = JsonSerializer.Serialize(modelo);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(
                    _client.BaseAddress + "/modelos/" + modelo.ModeloId, content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{modelo.Nome} Alterado com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            ViewData["Fabricantes"] = new SelectList(await GetFabricantes(), "FabricanteId", "Nome");
            return View(modelo);
        }

        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var modelo = await GetModelo(id);
            if (modelo == null)
            {
                TempData["errorMessage"] = "Modelo não Localizado!";
                return RedirectToAction("Index");
            }
            return View(modelo);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Modelo modelo = await GetModelo(id);
            if (modelo == null)
            {
                TempData["errorMessage"] = "Modelo não Localizado!";
                return RedirectToAction("Index");
            }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(
                    _client.BaseAddress + "/modelos/" + id
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{modelo.Nome} Excluído com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Excluir: " + ex.Message;
            }
            return View(modelo);
        }
        #endregion


        private async Task<List<Fabricante>> GetFabricantes()
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/fabricantes"
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                List<Fabricante> fabricantes = JsonSerializer.Deserialize<List<Fabricante>>(data, options);
                return fabricantes;
            }
            return null;
        }


        private async Task<Modelo> GetModelo(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/modelos/" + id
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                Modelo modelo = new();
                modelo = JsonSerializer.Deserialize<Modelo>(data, options);
                return modelo;
            }
            return null;
        }
    }
}
