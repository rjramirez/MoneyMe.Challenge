namespace WebAPI.Services.Interfaces
{
    public interface IQuoteService
    {
        decimal CalculateMonthlyRepaymentAmount(decimal amountRequired, string product, short term);
    }
}
