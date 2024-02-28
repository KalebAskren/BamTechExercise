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
    public class CreateAstronautDutyTests
    {
        private Mock<StargateContext> dbContext;
        private Mock<IDbConnection> dbConnection;
        private Mock<ILogger<CreateAstronautDutyHandler>> logger;
        private CreateAstronautDuty testDuty = new CreateAstronautDuty { DutyTitle = "testTitle", Name = "testName", Rank = "testRank" };

        private TestDataContextFactory _factory;
        public CreateAstronautDutyTests()
        {
            dbContext = new Mock<StargateContext>( new DbContextOptions <StargateContext>());
            dbConnection = new Mock<IDbConnection>();
            logger = new Mock<ILogger<CreateAstronautDutyHandler>>();

            _factory = new TestDataContextFactory();
        }
        [Fact]
        public async void CreateAstronautDutyHandle_GivenBadName_ThrowsException()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.SaveChanges();
                
                //Act
                var service = new CreateAstronautDutyHandler(ctx, logger.Object);
                
                //Assert
                await Assert.ThrowsAsync<ObjectNotFoundException>(() =>service.Handle(testDuty, new CancellationToken()));

                //cleanup
                ctx.Dispose();
            }
        }
        [Fact]
        public async void CreateAstronautDutyHandle_WithNoValidDetail_CreatesDetail()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Id = 1, Name = "testName" });
                ctx.SaveChanges();

                //Act
                var service = new CreateAstronautDutyHandler(ctx, logger.Object);
                await service.Handle(testDuty, new CancellationToken());

                //Assert
                Assert.NotNull(ctx.AstronautDetails.First(x => x.PersonId == 1));

                //cleanup
                ctx.Dispose();
            }
        }
        [Fact]
        public async void CreateAstronautDutyHandle_WithNoValidDetailAndRetired_RetiresAtStartDate()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Id = 1, Name = "testName" });
                ctx.SaveChanges();
                var timeStamp = new DateTime();

                //Act
                var service = new CreateAstronautDutyHandler(ctx, logger.Object);
                await service.Handle(new CreateAstronautDuty { DutyTitle = "RETIRED", DutyStartDate = timeStamp, Name = "testName", Rank = "testRank"}, new CancellationToken());

                //Assert
                Assert.Equal(timeStamp, ctx.AstronautDetails.First(x => x.PersonId == 1).CareerEndDate);

                //cleanup
                ctx.Dispose();
            }
        }
        [Fact]
        public async void CreateAstronautDutyHandle_WithNoValidDetailAndRetiredLowerCase_RetiresAtStartDate()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Id = 1, Name = "testName" });
                ctx.SaveChanges();
                var timeStamp = new DateTime();

                //Act
                var service = new CreateAstronautDutyHandler(ctx, logger.Object);
                await service.Handle(new CreateAstronautDuty { DutyTitle = "retired", DutyStartDate = timeStamp, Name = "testName", Rank = "testRank" }, new CancellationToken());

                //Assert
                Assert.Equal(timeStamp, ctx.AstronautDetails.First(x => x.PersonId == 1).CareerEndDate);

                //cleanup
                ctx.Dispose();
            }
        }
        [Fact]
        public async void CreateAstronautDutyHandle_WithValidDetailAndRetired_RetiresOneLessDayThanStartDate()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Id = 1, Name = "testName" });
                ctx.AstronautDetails.Add(new AstronautDetail
                {
                    CareerStartDate = new DateTime(),
                    CurrentRank = "testRank",
                    PersonId = 1
                });
                ctx.SaveChanges();
                ctx.ChangeTracker.Clear(); //Prevent Tracking conflicts
                var timeStamp = DateTime.UtcNow;

                //Act
                var service = new CreateAstronautDutyHandler(ctx, logger.Object);
                await service.Handle(new CreateAstronautDuty { DutyTitle = "retired", DutyStartDate = timeStamp, Name = "testName", Rank = "testRank" }, new CancellationToken());

                //Assert
                Assert.Equal(timeStamp.AddDays(-1).Date, ctx.AstronautDetails.First(x => x.PersonId == 1).CareerEndDate);
                Assert.Equal("RETIRED", ctx.AstronautDetails.First(x => x.PersonId == 1).CurrentDutyTitle);

                //cleanup
                ctx.Dispose();
            }
        }
        [Fact]
        public async void CreateAstronautDutyHandle_StartDateEarlierThanOldStartDate_ThrowsException()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                var timeStamp = DateTime.UtcNow;
                ctx.People.Add(new Person { Id = 1, Name = "testName" });
                ctx.AstronautDetails.Add(new AstronautDetail
                {
                    CareerStartDate = new DateTime(),
                    CurrentRank = "testRank",
                    PersonId = 1
                });
                ctx.AstronautDuties.Add(new AstronautDuty { DutyTitle = "old", DutyStartDate = timeStamp, PersonId = 1});
                ctx.SaveChanges();
                ctx.ChangeTracker.Clear(); //Prevent Tracking conflicts
                var tempDuty = testDuty;
                tempDuty.DutyStartDate = timeStamp.AddDays(-5).Date;

                //Act
                var service = new CreateAstronautDutyHandler(ctx, logger.Object);

                //Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() => service.Handle(tempDuty, new CancellationToken()));

                //cleanup
                ctx.Dispose();
            }
        }
        //Happy Path
        [Fact]
        public async void CreateAstronautDutyHandle_GivenGoodDataAndExistingDuty_ReturnsSecondDutyId()
        {
            // Arrange
            using (var ctx = _factory.Create())
            {
                ResetDb(ctx);
                ctx.People.Add(new Person { Id = 1, Name = "testName" });
                ctx.AstronautDetails.Add(new AstronautDetail
                {
                    CareerStartDate = new DateTime(),
                    CurrentRank = "testRank",
                    PersonId = 1
                });
                ctx.SaveChanges();
                ctx.ChangeTracker.Clear(); //Prevent Tracking conflicts
                var timeStamp = DateTime.UtcNow;

                //Act
                var service = new CreateAstronautDutyHandler(ctx, logger.Object);
                var result = await service.Handle(new CreateAstronautDuty { DutyTitle = "retired", DutyStartDate = timeStamp, Name = "testName", Rank = "testRank" }, new CancellationToken());

                //Assert
                Assert.Equal(2, result.Id);

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