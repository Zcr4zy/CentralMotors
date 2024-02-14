using CentralMotors.Models;
using CentralMotors.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace CentralMotors.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Uri baseAddress = new("http://localhost:5160/api");
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _client = new HttpClient
            {
                BaseAddress = baseAddress,
            };
        }

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

        
        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            var Veiculo = await GetVeiculo(id);
            if (Veiculo == null)
            {
                TempData["errorMessage"] = "Modelo não Localizado!";
                return RedirectToAction("Index");
            }
            return View(Veiculo);
        }

        private async Task<Veiculo> GetVeiculo(int id)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }  
    }

