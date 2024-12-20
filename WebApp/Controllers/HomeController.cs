using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Quote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebApp.Extensions;
using WebApp.Models.Home;

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

        [HttpPost]
        public async Task<IActionResult> QuoteCalculate([FromBody] SaveQuote saveQuote)
        {
            HttpClient client = _httpClientFactory.CreateClient("ProjectApiClient");
            var quoteResponse = await client.PostAsync($"api/Quote/save", saveQuote.GetStringContent());

            if (quoteResponse.IsSuccessStatusCode)
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                };

                QuoteDetail quoteDetail = JsonConvert.DeserializeObject<QuoteDetail>(await quoteResponse.Content.ReadAsStringAsync(), jsonSettings);


                QuoteViewModel quoteVM = new QuoteViewModel
                {
                    QuoteId = quoteDetail.QuoteId,
                    Product = quoteDetail.Product,
                    Amount = quoteDetail.Amount,
                    Term = quoteDetail.Term,
                    MonthlyRepaymentAmount = quoteDetail.MonthlyRepaymentAmount,
                    FirstName = quoteDetail.FirstName,
                    LastName = quoteDetail.LastName,
                    Email = quoteDetail.Email,
                    Mobile = quoteDetail.Mobile,
                    CreatedDate = quoteDetail.CreatedDate,
                    UpdatedDate = quoteDetail.UpdatedDate
                };

                return PartialView("~/Views/Loan/_QuotationDetail.cshtml", quoteVM);
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