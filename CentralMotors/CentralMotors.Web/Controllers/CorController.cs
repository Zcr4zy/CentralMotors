using CentralMotors.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CentralMotors.Web.Controllers
{
    public class CorController : Controller
    {
        Uri baseAddress = new("http://localhost:5160/api");
        private readonly HttpClient _client;

        public CorController()
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
            List<Cor> cors = [];
            HttpResponseMessage response = await _client.GetAsync(
                    _client.BaseAddress + "/cor");
            if(response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                cors = JsonSerializer.Deserialize<List<Cor>>(data, options);    
            }
            return View(cors);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Cor cor)
        {
            try
            {
                string conteudoJson = JsonSerializer.Serialize(cor);
                StringContent conteudostring = new(conteudoJson, Encoding.UTF8, "application/json");
                HttpResponseMessage resposta = await _client.PostAsync(
                    _client.BaseAddress + "/cor", conteudostring);
                if (resposta.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Cor Cadastrada com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Problemas ao Salvar" + ex.Message;
            }
            ViewData["Cor"] = new SelectList("CorId", "Nome");
            return View(cor);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cor = await GetCor(id);
            if (cor == null)
            {
                TempData["errorMessage"] = "Cor não Localizada!";
                return RedirectToAction("Index");
            }

            return View(cor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cor cor)
        {
            try
            {
                string data = JsonSerializer.Serialize(cor);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(
                    _client.BaseAddress + "/cor/" + cor.CorId, content
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Cor Alterada com Sucesso!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Salvar: " + ex.Message;
            }
            return View(cor);
        }

        
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cor = await GetCor(id);
            if(cor == null)
            {
                TempData["errorMessage"] = "Cor não Localizada!";
                return RedirectToAction("Index");
            }
            return View(cor);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Cor cor = await GetCor(id);
            if (cor == null)
            {
                TempData["errorMessage"] = "Cor não Localizada!";
                return RedirectToAction("Index");
            }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(
                    _client.BaseAddress + "/cor/" + id
                );
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"{cor.Nome} Excluída com Sucesso.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Problemas ao Excluir: " + ex.Message;
            }
            return View(cor);

        }
        #endregion


        private async Task<Cor> GetCor(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(
                _client.BaseAddress + "/cor/" + id
            );
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                Cor cor = JsonSerializer.Deserialize<Cor>(data, options);
                return cor;
            }
            return null;
        }

    }
}
