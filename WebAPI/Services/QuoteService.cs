
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class QuoteService : IQuoteService
    {
        public QuoteService()
        {

        }

        public decimal CalculateMonthlyRepaymentAmount(decimal amountRequired, string product, short term)
        {
            decimal annualInterestRate = 0.10m; // Default 10% annual interest rate
            decimal monthlyInterestRate = annualInterestRate / 12;
            int termInMonths = term; // Use Term directly as it is already a numeric type
            decimal establishmentFee = 100m; // Establishment fee

            if (product == "ProductA")
            {
                monthlyInterestRate = 0; // 0% interest for ProductA
            }
            else if (product == "ProductB")
            {
                // First 2 months 0% interest, then 10% annual interest rate
                int interestFreeMonths = 2;
                decimal interestFreeRepayment = (amountRequired + establishmentFee) / termInMonths;
                decimal remainingPrincipal = amountRequired - (interestFreeRepayment * interestFreeMonths);
                int remainingTerm = termInMonths - interestFreeMonths;

                if (remainingTerm > 0)
                {
                    decimal remainingRepayment = CalculateAnnuityRepayment(remainingPrincipal, monthlyInterestRate, remainingTerm);
                    return (interestFreeRepayment * interestFreeMonths + remainingRepayment * remainingTerm) / termInMonths;
                }
                else
                {
                    return interestFreeRepayment;
                }
            }

            //ProductC
            return CalculateAnnuityRepayment(amountRequired + establishmentFee, monthlyInterestRate, termInMonths);
        }

        private decimal CalculateAnnuityRepayment(decimal principal, decimal monthlyInterestRate, int termInMonths)
        {
            if (monthlyInterestRate == 0)
            {
                return principal / termInMonths;
            }

            decimal numerator = principal * monthlyInterestRate * (decimal)Math.Pow((double)(1 + monthlyInterestRate), termInMonths);
            decimal denominator = (decimal)Math.Pow((double)(1 + monthlyInterestRate), termInMonths) - 1;

            return numerator / denominator;
        }
    }
}

