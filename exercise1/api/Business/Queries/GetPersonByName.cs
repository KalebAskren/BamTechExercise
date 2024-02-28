using Dapper;
using MediatR;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;
using System.Data;
using System.Data.Entity.Core;
namespace StargateAPI.Business.Queries
{
    public class GetPersonByName : IRequest<GetPersonByNameResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class GetPersonByNameHandler : IRequestHandler<GetPersonByName, GetPersonByNameResult>
    {
        private readonly StargateContext _context;
        private readonly ILogger<GetPersonByNameHandler> _logger;
        public GetPersonByNameHandler(StargateContext context, ILogger<GetPersonByNameHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GetPersonByNameResult> Handle(GetPersonByName request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.Name)) throw new ArgumentException("Name is required");

            var result = new GetPersonByNameResult();

            var query = $"SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate FROM [Person] a " +
                $"LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id WHERE '{request.Name}' = a.Name";

            var person = await _context.Connection.QueryAsync<PersonAstronaut>(query);

            if (person.Count() < 1)
                throw new ObjectNotFoundException($"Person by the name {request.Name} does not exist.");

            result.Person = person.FirstOrDefault();
            _logger.LogInformation($"Success retrieving person {request.Name} by name.");
            return result;
        }
    }

    public class GetPersonByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
    }
}
