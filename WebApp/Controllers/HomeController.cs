using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Quote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebApp.Extensions;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CalculateQuotation(SaveQuote saveQuote)
        {
            HttpClient client = _httpClientFactory.CreateClient("ProjectApiClient");
            var response = await client.GetAsync($"api/ErrorLog/{id}");

            if (response.IsSuccessStatusCode)
            {
                ErrorLogDetail errorLogDetail = JsonConvert.DeserializeObject<ErrorLogDetail>(await response.Content.ReadAsStringAsync());
                return View(errorLogDetail);
            }
            else
            {
                return RedirectToAction("StatusPage", "Error", await response.GetErrorMessage());
            }
        }

        public IActionResult AuthenticationCallback()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}