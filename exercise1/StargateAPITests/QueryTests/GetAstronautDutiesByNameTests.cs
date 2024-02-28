using Microsoft.Extensions.Logging;
using Moq;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using StargateAPITests.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StargateAPITests.QueryTests
{
    public class GetAstronautDutiesByNameTests
    {
        private Mock<ILogger<GetAstronautDutiesByNameHandler>> logger;

        private TestDataContextFactory _factory;
        public GetAstronautDutiesByNameTests()
        {
            logger = new Mock<ILogger<GetAstronautDutiesByNameHandler>>();
            _factory = new TestDataContextFactory();
        }

        [Fact]
        public async void GetAstronautDutiesByName_GivenEmptyName_ThrowsException()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.SaveChanges();

                var service = new GetAstronautDutiesByNameHandler(ctx, logger.Object);

                //Act + Assert
                await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(new GetAstronautDutiesByName { Name = String.Empty }, new CancellationToken()));

                //cleanup
                ctx.Dispose();
            }
        }

        [Fact]
        public async void GetAstronautDutiesByName_GivenInvalidName_ThrowsException()
        {
            //Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Name = "Test Name", Id = 1 });

                ctx.SaveChanges();

                //Act + Assert

                var service = new GetAstronautDutiesByNameHandler(ctx, logger.Object);

                //Act + Assert
                await Assert.ThrowsAsync<ObjectNotFoundException>(() => service.Handle(new GetAstronautDutiesByName { Name = "Some Other Name" }, new CancellationToken()));

                //cleanup
                ctx.Dispose();
            }
        }
        //Happy Path
        [Fact]
        public async void GetAstronautDutiesByName_GivenValidName_ReturnsDuties()
        {
            //Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Name = "Test Name", Id = 1 });
                ctx.AstronautDuties.Add(new AstronautDuty { DutyTitle = "test duty", PersonId = 1 });
                ctx.SaveChanges();
                ctx.ChangeTracker.Clear();

                var service = new GetAstronautDutiesByNameHandler(ctx, logger.Object);

                //Act
                var result = await service.Handle(new GetAstronautDutiesByName { Name = "Test Name" }, new CancellationToken());

                //Assert
                Assert.Single(result.AstronautDuties);

                //cleanup
                ctx.Dispose();
            }
        }

        //Reset DB between each test for fresh accurate data
        private void ResetDb(StargateContext ctx)
        {
            ctx.AstronautDuties.RemoveRange(ctx.AstronautDuties.ToList());
            ctx.AstronautDetails.RemoveRange(ctx.AstronautDetails.ToList());
            ctx.People.RemoveRange(ctx.People.ToList());
            ctx.Errors.RemoveRange(ctx.Errors.ToList());
            ctx.SaveChanges();
        }
    }
}
