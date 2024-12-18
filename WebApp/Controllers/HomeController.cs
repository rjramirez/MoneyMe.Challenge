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
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> QuotationCalculate(SaveQuote saveQuote)
        {
            HttpClient client = _httpClientFactory.CreateClient("ProjectApiClient");
            var quoteResponse = await client.PostAsync($"api/Quote/Calculate", saveQuote.GetStringContent());

            if (quoteResponse.IsSuccessStatusCode)
            {
                ErrorLogDetail errorLogDetail = JsonConvert.DeserializeObject<ErrorLogDetail>(await quoteResponse.Content.ReadAsStringAsync());
                return View(errorLogDetail);
            }
            else
            {
                return RedirectToAction("StatusPage", "Error", await quoteResponse.GetErrorMessage());
            }
        }

        public async Task<IActionResult> QuoteDetail(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("ProjectApiClient");
            var response = await client.GetAsync($"api/Quote/{id}");

            if (response.IsSuccessStatusCode)
            {
                QuoteDetail quoteDetail = JsonConvert.DeserializeObject<QuoteDetail>(await response.Content.ReadAsStringAsync());
                return View(quoteDetail);
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