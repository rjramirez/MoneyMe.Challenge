using Common.Constants;
using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Quote;
using Common.DataTransferObjects.ReferenceData;
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
        private readonly HttpClient _httpClientMoneyMe;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientMoneyMe = httpClientFactory.CreateClient(HttpNamedClientConstant.ProjectApiClient);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QuoteCalculate([FromBody] SaveQuote saveQuote)
        {
            
            var quoteResponse = await _httpClientMoneyMe.PostAsync($"api/Quote/save", saveQuote.GetStringContent());

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
                    Title = quoteDetail.Title,
                    FirstName = quoteDetail.FirstName,
                    LastName = quoteDetail.LastName,
                    DateOfBirth = quoteDetail.DateOfBirth,
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

        [HttpPut]
        public async Task<IActionResult> QuoteUpdateCalculate([FromBody] SaveQuote saveQuote)
        {

            var quoteResponse = await _httpClientMoneyMe.PutAsync($"api/Quote/update", saveQuote.GetStringContent());

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
                    Title = quoteDetail.Title,
                    FirstName = quoteDetail.FirstName,
                    LastName = quoteDetail.LastName,
                    DateOfBirth = quoteDetail.DateOfBirth,
                    Email = quoteDetail.Email,
                    Mobile = quoteDetail.Mobile,
                    CreatedDate = quoteDetail.CreatedDate,
                    UpdatedDate = quoteDetail.UpdatedDate
                };

                return PartialView("~/Views/Loan/_QuotationDetailModal.cshtml", quoteVM);
            }
            else
            {
                return RedirectToAction("StatusPage", "Error", await quoteResponse.GetErrorMessage());
            }
        }

        public async Task<IActionResult> QuoteDetail(int id)
        {
            var response = await _httpClientMoneyMe.GetAsync($"api/Quote/{id}");

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

        public async Task<IActionResult> GetBlackList()
        {
            BlacklistViewModel blacklistVM = new();

            var response = await _httpClientMoneyMe.GetAsync($"api/Blacklist/GetBlackList");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ReferenceDataDetail> blacklistDetailList = JsonConvert.DeserializeObject<IEnumerable<ReferenceDataDetail>>(await response.Content.ReadAsStringAsync());
                
                return Ok(new
                {
                    IsSuccessful = true,
                    Message = "Blacklists found",
                    Data = blacklistDetailList
                });
            }
            else
            {
                return Ok(new
                {
                    IsSuccessful = false,
                    Message = "No available blacklist",
                    Data = new BlacklistViewModel()
                });
            }
        }

        public IActionResult AuthenticationCallback()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}