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

            QuoteDetail quoteDetail = new QuoteDetail
            {
                Product = saveQuote.Product,
                Amount = saveQuote.Amount,
                Term = saveQuote.Term,
                Title = saveQuote.Title,
                FirstName = saveQuote.FirstName,
                LastName = saveQuote.LastName,
                DateOfBirth = saveQuote.DateOfBirth,
                Email = saveQuote.Email,
                Mobile = saveQuote.Mobile,
                CreatedDate = DateTime.UtcNow,
            };

            Quote quote = new()
            {
                Product = quoteDetail.Product,
                Amount = quoteDetail.Amount,
                Term = quoteDetail.Term,
                Title = quoteDetail.Title,
                FirstName = quoteDetail.FirstName,
                LastName = quoteDetail.LastName,
                DateOfBirth = quoteDetail.DateOfBirth,
                Email = quoteDetail.Email,
                Mobile = quoteDetail.Mobile,
                MonthlyRepaymentAmount = quoteDetail.MonthlyRepaymentAmount,
                CreatedBy = "rj@moneyme.com",
                CreatedDate = DateTime.UtcNow,
                Active = true
            };

            await _projectTemplateDBUnitOfWork.QuoteRepository.AddAsync(quote);
            await _projectTemplateDBUnitOfWork.SaveChangesAsync(saveQuote.Email);

            return Ok(quote);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Quotation details by ID")]
        public async Task<ActionResult<QuoteDetail>> Get(int id)
        {
            QuoteDetail quoteDetail = await _projectTemplateDBUnitOfWork.QuoteRepository.
                FirstOrDefaultAsync(selector: e => new QuoteDetail()
                {
                    QuoteId = id,
                    Product = e.Product,
                    Term = e.Term,
                    Amount = e.Amount,
                    MonthlyRepaymentAmount = e.MonthlyRepaymentAmount,
                    Title = e.Title,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Mobile = e.Mobile,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate ?? DateTime.MinValue,
                },
                predicate: e => e.QuoteId == id);

            if (quoteDetail == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, $"Error ID not exist: {id}"));
            }
            else
            {
                return Ok(quoteDetail);
            }
        }
    }
}
