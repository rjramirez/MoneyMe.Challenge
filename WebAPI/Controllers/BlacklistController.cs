using Common.Constants;
using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Quote;
using Common.DataTransferObjects.ReferenceData;
using DataAccess.UnitOfWorks.MoneyMeChallengeDB;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        private readonly IMoneyMeChallengeDBUnitOfWork _projectDBUnitOfWork;
        public BlacklistController(IMoneyMeChallengeDBUnitOfWork projectTemplateDBUnitOfWork)
        {
            _projectDBUnitOfWork = projectTemplateDBUnitOfWork;
        }

        [HttpGet]
        [Route("GetBlackList")]
        [SwaggerOperation(Summary = "Get BlackList")]
        public async Task<ActionResult<IEnumerable<ReferenceDataDetail>>> GetBlackList()
        {
            IEnumerable<ReferenceDataDetail> referenceDataDetails = await _projectDBUnitOfWork.BlacklistRepository.FindAsync(
                selector: b => new ReferenceDataDetail()
                {
                    Name = b.Mobile ?? "",
                    Value = b.Domain ?? "",
                    Active = b.Active
                },
                predicate: r => r.Active == true,
                orderBy: r => r.OrderBy(o => o.BlacklistId));
            return Ok(referenceDataDetails);
        }
    }
}
