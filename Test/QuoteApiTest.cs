using Common.Constants;
using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Quote;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.UnitOfWorks.MoneyMeChallengeDB;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.Controllers;
using Xunit;

namespace WebAPI.Tests.Controllers
{
    public class QuoteControllerTests
    {
        private readonly Mock<IMoneyMeChallengeDBUnitOfWork> _mockUnitOfWork;
        private readonly QuoteController _controller;

        public QuoteControllerTests()
        {
            _mockUnitOfWork = new Mock<IMoneyMeChallengeDBUnitOfWork>();
            _controller = new QuoteController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Calculate_ReturnsBadRequest_WhenSaveQuoteIsNull()
        {
            // Arrange
            SaveQuote? saveQuote = null;

            // Act
            var result = await _controller.Create(saveQuote);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Calculate_ReturnsOk_WhenSaveQuoteIsValid()
        {
            // Arrange
            var saveQuote = new SaveQuote
            {
                Product = "TestProduct",
                Amount = 1000,
                Term = 12,
                Title = "Mr",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                Email = "john.doe@example.com",
                Mobile = "1234567890"
            };

            _mockUnitOfWork.Setup(uow => uow.QuoteRepository.AddAsync(It.IsAny<Quote>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(1);

            // Act
            var result = await _controller.Create(saveQuote);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var quote = Assert.IsType<Quote>(okResult.Value);
            Assert.Equal(saveQuote.Product, quote.Product);
            Assert.Equal(saveQuote.Amount, quote.AmountRequired);
            Assert.Equal(saveQuote.Term, quote.Term);
            Assert.Equal(saveQuote.Title, quote.Title);
            Assert.Equal(saveQuote.FirstName, quote.FirstName);
            Assert.Equal(saveQuote.LastName, quote.LastName);
            Assert.Equal(saveQuote.DateOfBirth, quote.DateOfBirth);
            Assert.Equal(saveQuote.Email, quote.Email);
            Assert.Equal(saveQuote.Mobile, quote.Mobile);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenQuoteDoesNotExist()
        {
            // Arrange
            int quoteId = 1;
            _mockUnitOfWork.Setup(uow => uow.QuoteRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<Quote, QuoteDetail>>>(), It.IsAny<Expression<Func<Quote, bool>>>()))
                .ReturnsAsync((QuoteDetail?)null);

            // Act
            var result = await _controller.Get(quoteId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorMessage = Assert.IsType<ErrorMessageDetail>(notFoundResult.Value);
            Assert.Equal(ErrorMessageTypeConstant.NotFound, errorMessage.Type);
            Assert.Equal($"Error ID not exist: {quoteId}", errorMessage.Message);
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenQuoteExists()
        {
            // Arrange
            int quoteId = 1;
            var quoteDetail = new QuoteDetail
            {
                QuoteId = quoteId,
                Product = "TestProduct",
                AmountRequired = 1000,
                Term = 12,
                Title = "Mr",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                Email = "john.doe@example.com",
                Mobile = "1234567890",
                CreatedDate = DateTime.UtcNow
            };

            _mockUnitOfWork.Setup(uow => uow.QuoteRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<Quote, QuoteDetail>>>(), It.IsAny<Expression<Func<Quote, bool>>>()))
                .ReturnsAsync(quoteDetail);

            // Act
            var result = await _controller.Get(quoteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuoteDetail = Assert.IsType<QuoteDetail>(okResult.Value);
            Assert.Equal(quoteDetail.QuoteId, returnedQuoteDetail.QuoteId);
            Assert.Equal(quoteDetail.Product, returnedQuoteDetail.Product);
            Assert.Equal(quoteDetail.AmountRequired, returnedQuoteDetail.AmountRequired);
            Assert.Equal(quoteDetail.Term, returnedQuoteDetail.Term);
            Assert.Equal(quoteDetail.Title, returnedQuoteDetail.Title);
            Assert.Equal(quoteDetail.FirstName, returnedQuoteDetail.FirstName);
            Assert.Equal(quoteDetail.LastName, returnedQuoteDetail.LastName);
            Assert.Equal(quoteDetail.DateOfBirth, returnedQuoteDetail.DateOfBirth);
            Assert.Equal(quoteDetail.Email, returnedQuoteDetail.Email);
            Assert.Equal(quoteDetail.Mobile, returnedQuoteDetail.Mobile);
            Assert.Equal(quoteDetail.CreatedDate, returnedQuoteDetail.CreatedDate);
        }
    }
}
