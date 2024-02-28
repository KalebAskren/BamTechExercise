using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Data.Entity.Core;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AstronautDutyController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Gets all astronaut duty for a person with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            var result = await _mediator.Send(new GetAstronautDutiesByName()//should this be get astronautByName controller instead?
            {
                Name = name
            });

            return this.GetResponse(result);           
        }
        /// <summary>
        /// Create a duty for an astronaut
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
                var result = await _mediator.Send(request);
                return this.GetResponse(result);           
        }
    }
}