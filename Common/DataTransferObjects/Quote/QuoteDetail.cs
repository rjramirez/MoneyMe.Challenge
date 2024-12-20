﻿namespace Common.DataTransferObjects.Quote
{
    public class QuoteDetail
    {
        public int QuoteId { get; set; }
        public string Product { get; set; }
        public string Term { get; set; }
        public decimal Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public decimal MonthlyRepaymentAmount
        {
            get
            {
                return CalculateMonthlyRepaymentAmount();
            }
        }

        private decimal CalculateMonthlyRepaymentAmount()
        {
            decimal annualInterestRate = 0.10m; // Default 10% annual interest rate
            decimal monthlyInterestRate = annualInterestRate / 12;
            int termInMonths = int.Parse(Term);

            if (Product == "ProductA")
            {
                monthlyInterestRate = 0; // 0% interest for ProductA
            }
            else if (Product == "ProductB")
            {
                // First 2 months 0% interest, then 10% annual interest rate
                int interestFreeMonths = 2;
                decimal interestFreeRepayment = Amount / termInMonths;
                decimal remainingPrincipal = Amount - (interestFreeRepayment * interestFreeMonths);
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

            return CalculateAnnuityRepayment(Amount, monthlyInterestRate, termInMonths);
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
