using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Filters;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all people in the astronaut DB
        /// </summary>
        /// <returns></returns>
        [HttpGet("People")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPeople()
        {
            var result = await _mediator.Send(new GetPeople() {});

            return this.GetResponse(result);
        }

        /// <summary>
        /// Gets a Person by a name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPersonByName([Required]string name)
        {
            var result = await _mediator.Send(new GetPersonByName()
            {
                Name = name
            });

            return this.GetResponse(result);
        }

        [HttpPost("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePerson([Required]string name)
        {
            var result = await _mediator.Send(new CreatePerson()
            {
                Name = name
            });

            return this.GetResponse(result);
        }
    }
}