using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using StargateAPITests.Helpers;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Net.WebSockets;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace StargateAPITests.QueryTests
{    
    public class GetPersonByNameTests
    {
        private Mock<ILogger<GetPersonByNameHandler>> logger;

        private TestDataContextFactory _factory;
        public GetPersonByNameTests()
        {
            logger = new Mock<ILogger<GetPersonByNameHandler>>();
            _factory = new TestDataContextFactory();
        }

        [Fact]
        public async void GetPersonByNameHandle_GivenBadName_ThrowsException()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.SaveChanges();

                var service = new GetPersonByNameHandler(ctx, logger.Object);

                //Act + Assert
                await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(new GetPersonByName { Name = String.Empty }, new CancellationToken()));

                //cleanup
                ctx.Dispose();
            }
        }
        
        [Fact]
        public async void GetPersonByNameHandle_GivenNameThatDoesntExist_ThrowsObjectNotFoundException()
        {
            //Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Name = "Test Name", Id = 1 });

                ctx.SaveChanges();

                //Act + Assert

                var service = new GetPersonByNameHandler(ctx, logger.Object);
                await Assert.ThrowsAsync<ObjectNotFoundException>(() => service.Handle(new GetPersonByName { Name = "Other Name" }, new CancellationToken()));

                //cleanup
                ctx.Dispose();
            }
        }
        //Happy Path
        [Fact]
        public async void GetPersonByNameHandle_GivenGoodName_ReturnsPersonId()
        {
            //Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Name = "Test Name", Id = 1 });
                ctx.SaveChanges();
                var service = new GetPersonByNameHandler(ctx, logger.Object);

                //Act
                var result = await service.Handle(new GetPersonByName { Name = "Test Name" }, new CancellationToken());

                //Assert
                Assert.Equal(1, result.Person.PersonId);

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

