using Castle.Core.Logging;
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
using StargateAPITests.Helpers;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Net.WebSockets;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StargateAPITests.CommandTests
{
    public class CreatePersonTests
    {
        private Mock<ILogger<CreatePersonHandler>> logger;

        private TestDataContextFactory _factory;
        public CreatePersonTests()
        {
            logger = new Mock<ILogger<CreatePersonHandler>>();
            _factory = new TestDataContextFactory();
        }

        [Fact]
        public async void CreatePersonHandle_GivenBadName_ThrowsException()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.SaveChanges();

                var service = new CreatePersonHandler(ctx, logger.Object);

                //Act + Assert
                await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(new CreatePerson { Name = String.Empty }, new CancellationToken()));

                //cleanup
                ctx.Dispose();
            }
        }

        //Happy Path
        [Fact]
        public async void CreatePersonHandle_GivenGoodName_ReturnsPersonId()
        {
            //Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.SaveChanges();

                //Act
                var service = new CreatePersonHandler(ctx, logger.Object);
                var result = await service.Handle(new CreatePerson { Name = "test" }, new CancellationToken());

                //Assert
                Assert.Equal(3, result.Id);

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