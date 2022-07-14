using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using Xunit;

namespace DeskBooker.Web.Pages
{
    public class BookDeskModelTests
    {
        private Mock<IDeskBookingRequestProcessor> _processorMock;
        private BookDeskModel _bookDeskModel;
        private DeskBookingResult _deskBookingResult;

        public BookDeskModelTests()
        {
            _processorMock = new Mock<IDeskBookingRequestProcessor>();
            _bookDeskModel = new BookDeskModel(_processorMock.Object)
            {
                DeskBookingRequest = new DeskBookingRequest()
            };

            _deskBookingResult = new DeskBookingResult
            {
                Code = DeskBookingResultCode.Success
            };

            _processorMock.Setup(x => x.BookDesk(_bookDeskModel.DeskBookingRequest))
                .Returns(_deskBookingResult);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void ShouldCallBookDeskMethodOfProcessorIfModelIsValid(int expectedBookDeskCalls, bool isModelValid)
        {
            //arrange
            if (!isModelValid)
            {
                _bookDeskModel.ModelState.AddModelError("JustAKey", "AnErrorMessage");
            }

            //act
            _bookDeskModel.OnPost();

            //assert
            _processorMock.Verify(x => x.BookDesk(_bookDeskModel.DeskBookingRequest), Times.Exactly(expectedBookDeskCalls));

        }

        [Fact]
        public void ShouldAddModelErrorIfNoDeskIsAvailable()
        {
            // arrange
            _deskBookingResult.Code = DeskBookingResultCode.NoDeskAvailable;

            //act
            _bookDeskModel.OnPost();

            //assert
            var modelStateEntry =
            Assert.Contains("DeskBookingRequest.Date", _bookDeskModel.ModelState);

            var modelError = Assert.Single(modelStateEntry.Errors);

            Assert.Equal("No desk available for selected date", modelError.ErrorMessage);


        }

        [Fact]
        public void ShouldNotAddModelErrorIfDeskIsAvailable()
        {
            // arrange
            _deskBookingResult.Code = DeskBookingResultCode.Success;

            //act
            _bookDeskModel.OnPost();

            //assert
            //var modelStateEntry = 
            //    Assert.Contains("DeskBookingRequest.Date", bookDeskModel.ModelState);
            //var modelError = Assert.Single(modelStateEntry.Errors);

            Assert.DoesNotContain("DeskBookingRequest.Date", _bookDeskModel.ModelState);


        }

        [Theory]
        [InlineData(typeof(PageResult), false, null)]
        [InlineData(typeof(PageResult), true, DeskBookingResultCode.NoDeskAvailable)]
        [InlineData(typeof(RedirectToPageResult), true, DeskBookingResultCode.Success)]
        public void ShouldReturnExpectedActionResult(Type ExpectedActionResultType, bool isModelValid, DeskBookingResultCode? deskBookingResultCode)
        {
            //arrange
            if (!isModelValid)
            {
                _bookDeskModel.ModelState.AddModelError("JustAKey", "AnErrorMessage");
            }

            if (deskBookingResultCode.HasValue)
            {
                _deskBookingResult.Code = deskBookingResultCode.Value;
            }

            //act
            IActionResult actionResult = _bookDeskModel.OnPost();

            //assert
            Assert.IsType(ExpectedActionResultType, actionResult);
        }
    }
}
