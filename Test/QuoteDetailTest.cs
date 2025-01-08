namespace Common.DataTransferObjects.Quote
{
    public class QuoteDetailTests
    {
        [Fact]
        public void CalculateMonthlyRepaymentAmount_ProductA_NoInterest()
        {
            // Arrange
            var quoteDetail = new QuoteDetail
            {
                Product = "ProductA",
                AmountRequired = 1000,
                Term = 12
            };

            var establishmentFee = 100m;

            // Act
            var monthlyRepayment = quoteDetail.MonthlyRepaymentAmount;

            // Assert
            decimal expectedRepayment = (1000m + establishmentFee) / 12m; // Amount + Establishment Fee / Term
            Assert.Equal(Math.Round(expectedRepayment, 2), Math.Round(monthlyRepayment, 2));
        }

        [Fact]
        public void CalculateMonthlyRepaymentAmount_ProductB_TwoMonthsFreeInterest()
        {
            // Arrange
            var quoteDetail = new QuoteDetail
            {
                Product = "ProductB",
                AmountRequired = 1000,
                Term = 12
            };

            // Act
            var monthlyRepayment = quoteDetail.MonthlyRepaymentAmount;

            // Assert
            decimal annualInterestRate = 0.10m;
            decimal monthlyInterestRate = annualInterestRate / 12;
            int interestFreeMonths = 2;
            decimal establishmentFee = 100m;
            decimal interestFreeRepayment = (1000m + establishmentFee) / 12m;
            decimal remainingPrincipal = 1000m - (interestFreeRepayment * interestFreeMonths);
            int remainingTerm = 12 - interestFreeMonths;

            decimal remainingRepayment = CalculatePMT(monthlyInterestRate, remainingTerm, remainingPrincipal);
            decimal expectedRepayment = (interestFreeRepayment * interestFreeMonths + remainingRepayment * remainingTerm) / 12m;

            Assert.Equal(Math.Round(expectedRepayment, 2), Math.Round(monthlyRepayment, 2));
        }

        private decimal CalculatePMT(decimal rate, int nper, decimal pv)
        {
            if (rate == 0)
            {
                return pv / nper;
            }

            decimal numerator = pv * rate * (decimal)Math.Pow((double)(1 + rate), nper);
            decimal denominator = (decimal)Math.Pow((double)(1 + rate), nper) - 1;

            return numerator / denominator;
        }
    }
}

