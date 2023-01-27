using System.Diagnostics;
using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Public.DTO.v1;
using App.Public.DTO.v1.error;
using App.Public.Mappers;
using AutoMapper;

namespace WebApp.ApiControllers
{
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PersonMapper _mapper;

        public PersonsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new PersonMapper(mapper);
        }

        // GET: api/Persons
        /// <summary>
        /// Get all Persons
        /// </summary>
        /// <returns>Persons list</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            if (_bll.Persons == null)
            {
              return NotFound();
            }
            return (await _bll.Persons.GetAllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        // GET: api/Persons/5
        /// <summary>
        /// Get Person
        /// </summary>
        /// <param name="id">Person id</param>
        /// <returns>Person</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            if (_bll.Persons == null)
            {
              return NotFound();
            }
            var person = _mapper.Map(await _bll.Persons.FirstOrDefaultAsync(id));

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        /// <summary>
        /// Get Person by identity code
        /// </summary>
        /// <param name="identityCode">Person identity code</param>
        /// <param name="eventId">EventId id</param>
        /// <returns>Person</returns>
        [HttpGet("identityCode/{identityCode}/{eventId}")]
        public async Task<ActionResult<Person>> GetPersonByIdentityCode(string identityCode, Guid eventId)
        {
            if (_bll.Persons == null)
            {
                return NotFound();
            }

            try
            {
                var person = _mapper.Map(await _bll.Persons.GetPersonByIdentityCode(identityCode, eventId));
                return person;
            }
            catch (Exception e)
            {
                var errorResponse = new RestApiErrorResponse()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "App error",
                    Status = HttpStatusCode.BadRequest,
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                };
                
                errorResponse.Errors["errorMsg"] = new List<string>()
                {
                    e.Message
                };

                return BadRequest(errorResponse);
            }

            // if (person == null)
            // {
            //     return NotFound();
            // }
            //
            // return person;
        }

        // PUT: api/Persons/5
        /// <summary>
        /// Update Person data
        /// </summary>
        /// <param name="id">Person id</param>
        /// <param name="person">Changed Person data</param>
        /// <returns>Updated Persons data</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(Guid id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            try
            {
                _bll.Persons.Update(_mapper.Map(person)!);
            }
            catch (Exception e)
            {
                var errorResponse = new RestApiErrorResponse()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "App error",
                    Status = HttpStatusCode.BadRequest,
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                };
                
                errorResponse.Errors["errorMsg"] = new List<string>()
                {
                    e.Message
                };

                return BadRequest(errorResponse);
            }

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Persons
        /// <summary>
        /// Create Person
        /// </summary>
        /// <param name="person">Person data</param>
        /// <returns>Person</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            if (_bll.Persons == null)
            {
              return Problem("Entity set 'AppDbContext.Persons'  is null.");
            }

            try
            {
                _bll.Persons.Add(_mapper.Map(person)!);
                await _bll.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var errorResponse = new RestApiErrorResponse()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "App error",
                    Status = HttpStatusCode.BadRequest,
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                };
                
                errorResponse.Errors["errorMsg"] = new List<string>()
                {
                    e.Message
                };

                return BadRequest(errorResponse);
            }

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/Persons/5
        /// <summary>
        /// Delete Person
        /// </summary>
        /// <param name="id">Person id</param>
        /// <returns></returns>~
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            if (_bll.Persons == null)
            {
                return NotFound();
            }
            var person = await _bll.Persons.FirstOrDefaultAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _bll.Persons.Remove(person);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(Guid id)
        {
            return _bll.Persons.Exists(id);
        }
    }
}
