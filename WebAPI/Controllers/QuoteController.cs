using Common.Constants;
using Common.DataTransferObjects.CollectionPaging;
using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Quote;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.UnitOfWorks.MoneyMeChallengeDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IMoneyMeChallengeDBUnitOfWork _projectTemplateDBUnitOfWork;
        public QuoteController(IMoneyMeChallengeDBUnitOfWork projectTemplateDBUnitOfWork)
        {
            _projectTemplateDBUnitOfWork = projectTemplateDBUnitOfWork;
        }

        [HttpPost]
        [Route("save")]
        [SwaggerOperation(Summary = "Save Quotation")]
        public async Task<IActionResult> Calculate(SaveQuote saveQuote)
        {
            if (saveQuote == null)
            {
                return BadRequest("Invalid quote data.");
            }

            Quote quote = new()
            {
                Amount = saveQuote.Amount,
                Term = saveQuote.Term,
                Title = saveQuote.Title,
                FirstName = saveQuote.FirstName,
                LastName = saveQuote.LastName,
                DateOfBirth = saveQuote.DateOfBirth,
                Email = saveQuote.Email,
                Mobile = saveQuote.Mobile,
                CreatedBy = "rj@moneyme.com",
                CreatedDate = DateTime.UtcNow,
                Active = true
            };

            await _projectTemplateDBUnitOfWork.QuoteRepository.AddAsync(quote);
            await _projectTemplateDBUnitOfWork.SaveChangesAsync(saveQuote.Email);

            return Ok(quote);
        }

        //[HttpGet("{id}")]
        //[SwaggerOperation(Summary = "Get Quotation details by ID")]
        //public async Task<ActionResult<QuoteDetail>> Get(int id)
        //{
        //    QuoteDetail quoteDetail = await _projectTemplateDBUnitOfWork.QuoteRepository.
        //        FirstOrDefaultAsync(selector: e => new QuoteDetail()
        //        {
        //            QuoteId = id,
        //            FinanceAmount = e.Amount,
        //            Title = e.Title, 
        //            FirstName = e.FirstName,
        //            LastName = e.LastName,
        //            Email = e.Email,
        //            Mobile= e.Mobile
        //        },
        //        predicate: e => e.QuoteId == id);


        //    if (quoteDetail == null)
        //    {
        //        return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, $"Error ID not exist: {id}"));
        //    }
        //    else
        //    {
        //        return Ok(quoteDetail);
        //    }
        //}

        //[HttpGet]
        //[Route("QuoteListPagedSearch")]
        //[SwaggerOperation(Summary = "Search Error Log with Paging")]
        //public async Task<ActionResult<PagedList<QuoteDetail>>> Search([FromQuery] CommonSearchFilter commonSearchFilter)
        //{
        //    PagedList<QuoteDetail> QuoteDetails = await _projectTemplateDBUnitOfWork.QuoteRepository.GetPagedListAsync(
        //        selector: e => new QuoteDetail()
        //        {
        //            QuoteId = e.QuoteId,
        //            Amount = e.Amount,
        //            Title = e.Title,
        //            FirstName = e.FirstName,
        //            LastName = e.LastName,
        //            Email = e.Email,
        //            Mobile = e.Mobile
        //        },
        //        predicate: e => ((commonSearchFilter.StartDate == null || e.DateCreated >= commonSearchFilter.StartDate) &&
        //                         (commonSearchFilter.EndDate == null || e.DateCreated <= commonSearchFilter.EndDate)) &&
        //                         (string.IsNullOrEmpty(commonSearchFilter.SearchKeyword) || (e.BuildVersion.Contains(commonSearchFilter.SearchKeyword) ||
        //                                                                  e.Message.Contains(commonSearchFilter.SearchKeyword) ||
        //                                                                  e.Path.Contains(commonSearchFilter.SearchKeyword) ||
        //                                                                  e.Source.Contains(commonSearchFilter.SearchKeyword) ||
        //                                                                  e.StackTrace.Contains(commonSearchFilter.SearchKeyword) ||
        //                                                                  e.StackTraceId.Contains(commonSearchFilter.SearchKeyword) ||
        //                                                                  e.UserIdentity.Contains(commonSearchFilter.SearchKeyword))),
        //        pagingParameter: commonSearchFilter,
        //        orderBy: o => o.OrderByDescending(e => e.DateCreated));

        //    Response.Headers.Add(PagingConstant.PagingHeaderKey, QuoteDetails.PagingHeaderValue);

        //    return Ok(QuoteDetails);
        //}
    }
}
