using Dapper;
using MediatR;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;
using System.Data.Entity.Core;

namespace StargateAPI.Business.Queries
{
    public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
    {
        private readonly StargateContext _context;
        private readonly ILogger<GetAstronautDutiesByNameHandler> _logger;

        public GetAstronautDutiesByNameHandler(StargateContext context, ILogger<GetAstronautDutiesByNameHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.Name)) throw new ArgumentException("Name is Required");

            var result = new GetAstronautDutiesByNameResult();
            var query = $"SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate FROM [Person] a LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id WHERE \'{request.Name}\' = a.Name";

            var person = await _context.Connection.QueryFirstOrDefaultAsync<PersonAstronaut>(query);

            if (person == null)
                throw new ObjectNotFoundException($"Person by name {request.Name} does not exist");

            query = $"SELECT * FROM [AstronautDuty] WHERE {person.PersonId} = PersonId Order By DutyStartDate Desc";

            var duties = await _context.Connection.QueryAsync<AstronautDuty>(query);

            result.Person = person;
            result.AstronautDuties = duties.ToList();

            _logger.LogInformation($"Successfully retrieved duties for {request.Name}");
            return result;
        }
    }

    public class GetAstronautDutiesByNameResult : BaseResponse
    {
        public PersonAstronaut Person { get; set; }
        public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}
