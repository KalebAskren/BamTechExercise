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
    public class GetPeopleTests
    {
        private Mock<ILogger<GetPeopleHandler>> logger;

        private TestDataContextFactory _factory;
        public GetPeopleTests()
        {
            logger = new Mock<ILogger<GetPeopleHandler>>();
            _factory = new TestDataContextFactory();
        }

        [Fact]
        public async void GetPeopleHandle_WithNoPeople_ReturnEmptyList()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.SaveChanges();

                var service = new GetPeopleHandler(ctx, logger.Object);
                //Act
                var result = await service.Handle(new GetPeople(), new CancellationToken());
                
                //Assert
                Assert.Empty(result.People);

                //cleanup
                ctx.Dispose();
            }
        }

        //Happy Path
        [Fact]
        public async void GetPeopleHandle_WithPeople_ReturnPeopleList()
        {
            //Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.AddRange(
                    new Person 
                    { 
                        Name = "Test Name", Id = 1 
                    },
                    new Person
                    {
                        Name = "Other Name", Id=2
                    });
                ctx.SaveChanges();
                var service = new GetPeopleHandler(ctx, logger.Object);

                //Act
                var result = await service.Handle(new GetPeople { }, new CancellationToken());

                //Assert
                Assert.Equal(2, result.People.Count);

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

