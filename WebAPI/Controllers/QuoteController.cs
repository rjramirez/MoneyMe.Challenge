using Common.Constants;
using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Quote;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.UnitOfWorks.MoneyMeChallengeDB;
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
        public async Task<IActionResult> Create(SaveQuote saveQuote)
        {
            if (saveQuote == null)
            {
                return BadRequest("Invalid quote data.");
            }

            QuoteDetail quoteDetail = new QuoteDetail
            {
                Product = saveQuote.Product,
                AmountRequired = saveQuote.Amount,
                Term = saveQuote.Term,
                Title = saveQuote.Title,
                FirstName = saveQuote.FirstName,
                LastName = saveQuote.LastName,
                DateOfBirth = saveQuote.DateOfBirth,
                Email = saveQuote.Email,
                Mobile = saveQuote.Mobile,
            };

            Quote quote = new()
            {
                Product = quoteDetail.Product,
                Amount = quoteDetail.AmountRequired,
                Term = quoteDetail.Term,
                Title = quoteDetail.Title,
                FirstName = quoteDetail.FirstName,
                LastName = quoteDetail.LastName,
                DateOfBirth = quoteDetail.DateOfBirth,
                Email = quoteDetail.Email,
                Mobile = quoteDetail.Mobile,
                MonthlyRepaymentAmount = quoteDetail.MonthlyRepaymentAmount,
                CreatedBy = quoteDetail.Email,
                CreatedDate = DateTime.UtcNow,
                Active = true
            };

            await _projectTemplateDBUnitOfWork.QuoteRepository.AddAsync(quote);
            await _projectTemplateDBUnitOfWork.SaveChangesAsync(saveQuote.Email);

            return Ok(quote);
        }

        [HttpPut]
        [Route("update")]
        [SwaggerOperation(Summary = "Update Quotation")]
        public async Task<IActionResult> Update(SaveQuote saveQuote)
        {
            if (saveQuote == null || saveQuote.QuoteId == 0)
            {
                return BadRequest("Invalid quote data.");
            }

            QuoteDetail quoteDetail = new QuoteDetail
            {
                Product = saveQuote.Product,
                AmountRequired = saveQuote.Amount,
                Term = saveQuote.Term
            };

            Quote quoteDetailExisting = await _projectTemplateDBUnitOfWork.QuoteRepository.FirstOrDefaultAsync(e => e.QuoteId == saveQuote.QuoteId);
            
            quoteDetailExisting.Product = saveQuote.Product;
            quoteDetailExisting.Amount = saveQuote.Amount;
            quoteDetailExisting.MonthlyRepaymentAmount = quoteDetail.MonthlyRepaymentAmount;
            quoteDetailExisting.Term = saveQuote.Term;
            quoteDetailExisting.Title = saveQuote.Title;
            quoteDetailExisting.FirstName = saveQuote.FirstName;
            quoteDetailExisting.LastName = saveQuote.LastName;
            quoteDetailExisting.DateOfBirth = saveQuote.DateOfBirth;
            quoteDetailExisting.Email = saveQuote.Email;
            quoteDetailExisting.Mobile = saveQuote.Mobile;
            quoteDetailExisting.UpdatedBy = saveQuote.Email;
            quoteDetailExisting.UpdatedDate = DateTime.UtcNow;

            await _projectTemplateDBUnitOfWork.SaveChangesAsync(saveQuote.Email);

            return Ok(quoteDetailExisting);
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
                    AmountRequired = e.Amount,
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
                return NotFound(new ErrorMessageDetail(ErrorMessageTypeConstant.NotFound, $"Error ID not exist: {id}"));
            }
            else
            {
                return Ok(quoteDetail);
            }
        }
    }
}
