using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using StargateAPI.Business.Data.Helpers;
using StargateAPI.Filters;
using StargateAPITests.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace StargateAPITests.FiltersTests
{
    public class ExceptionHandlerAttributeTests
    {
        private Mock<ILogger<ExceptionHandlerAttribute>> _logger;
        private Mock<ExceptionLoggingHelper> _exceptionLoggingHelper;
        private Mock<ExceptionContext> _exceptionContext;
        private TestDataContextFactory _factory = new TestDataContextFactory();

        public ExceptionHandlerAttributeTests()
        {
            _logger = new Mock<ILogger<ExceptionHandlerAttribute>>();
            _exceptionLoggingHelper = new Mock<ExceptionLoggingHelper>(_factory.Create(), new Mock<ILogger<ExceptionLoggingHelper>>().Object);
            //ActionContext actionContext, IList< IFilterMetadata > filters
            _exceptionContext = new Mock<ExceptionContext>(new ActionContext() 
                { 
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor()
                }, new List<IFilterMetadata>());
        }

        [Fact]
        public void OnException_GivenObjectNotFoundException_ReturnsBadRequest()
        {
            //Arrange
            var service = new ExceptionHandlerAttribute(_logger.Object, _exceptionLoggingHelper.Object);
            _exceptionContext.Setup(x => x.Exception).Returns(new ObjectNotFoundException("Test Message"));
            _exceptionLoggingHelper.Setup(x => x.PersistException(It.IsAny<ObjectNotFoundException>())).Verifiable();

            //Act
            service.OnException(_exceptionContext.Object);

            //Assert
            Assert.Equal(400, _exceptionContext.Object.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void OnException_GivenInvalidOperation_ReturnsBadRequest()
        {
            //Arrange
            var service = new ExceptionHandlerAttribute(_logger.Object, _exceptionLoggingHelper.Object);
            _exceptionContext.Setup(x => x.Exception).Returns(new InvalidOperationException("Test Message"));
            _exceptionLoggingHelper.Setup(x => x.PersistException(It.IsAny<InvalidOperationException>())).Verifiable();

            //Act
            service.OnException(_exceptionContext.Object);

            //Assert
            Assert.Equal(400, _exceptionContext.Object.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void OnException_GivenArgumentException_ReturnsBadRequest()
        {
            //Arrange
            var service = new ExceptionHandlerAttribute(_logger.Object, _exceptionLoggingHelper.Object);
            _exceptionContext.Setup(x => x.Exception).Returns(new ArgumentException("Test Message"));
            _exceptionLoggingHelper.Setup(x => x.PersistException(It.IsAny<ArgumentException>())).Verifiable();

            //Act
            service.OnException(_exceptionContext.Object);

            //Assert
            Assert.Equal(400, _exceptionContext.Object.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void OnException_GivenUnauthorizedAccess_ReturnsUnauthorized()
        {
            //Arrange
            var service = new ExceptionHandlerAttribute(_logger.Object, _exceptionLoggingHelper.Object);
            _exceptionContext.Setup(x => x.Exception).Returns(new UnauthorizedAccessException("Test Message"));
            _exceptionLoggingHelper.Setup(x => x.PersistException(It.IsAny<UnauthorizedAccessException>())).Verifiable();

            //Act
            service.OnException(_exceptionContext.Object);

            //Assert
            Assert.Equal(401, _exceptionContext.Object.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void OnException_GivenException_PersistsException()
        {
            //Arrange
            var service = new ExceptionHandlerAttribute(_logger.Object, _exceptionLoggingHelper.Object);
            _exceptionContext.Setup(x => x.Exception).Returns(new ObjectNotFoundException("Test Message"));
            _exceptionLoggingHelper.Setup(x => x.PersistException(It.IsAny<ObjectNotFoundException>())).Verifiable();

            //Act
            service.OnException(_exceptionContext.Object);

            //Assert
            _exceptionLoggingHelper.Verify(x => x.PersistException(It.IsAny<Exception>()), Times.Once);
        }
    }
}
