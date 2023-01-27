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
    public class EnterprisesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly EnterpriseMapper _mapper;

        public EnterprisesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new EnterpriseMapper(mapper);
        }

        // GET: api/Enterprises
        /// <summary>
        /// Get all Enterprise clients
        /// </summary>
        /// <returns>Enterprises list</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enterprise>>> GetEnterprises()
        {
            return (await _bll.Enterprises.GetAllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        // GET: api/Enterprises/5
        /// <summary>
        /// Get Enterprise client
        /// </summary>
        /// <param name="id">Enterprise id</param>
        /// <returns>Enterprise</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Enterprise>> GetEnterprise(Guid id)
        {
            if (_bll.Enterprises == null)
            {
              return NotFound();
            }
            var enterprise = _mapper.Map(await _bll.Enterprises.FirstOrDefaultAsync(id));

            if (enterprise == null)
            {
                return NotFound();
            }

            return enterprise;
        }
        /// <summary>
        /// Get Enterprise by register code
        /// </summary>
        /// <param name="registerCode">Enterprise register code</param>
        /// <param name="eventId">EventId</param>
        /// <returns>Enterprise</returns>
        [HttpGet("registerCode/{registerCode}/{eventId}")]
        public async Task<ActionResult<Enterprise>> GetEnterpriseByRegisterCode(string registerCode, Guid eventId)
        {
            if (_bll.Enterprises == null)
            {
                return NotFound();
            }

            try
            {
                var enterprise = _mapper.Map(await _bll.Enterprises.GetEnterpriseByRegisterCode(registerCode, eventId));
                return enterprise;
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
        }

        // PUT: api/Enterprises/5
        /// <summary>
        /// Update Enterprise data
        /// </summary>
        /// <param name="id">Enterprise id</param>
        /// <param name="enterprise">Changed Enterprise data</param>
        /// <returns>Updated Enterprise data</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnterprise(Guid id, Enterprise enterprise)
        {
            if (id != enterprise.Id)
            {
                return BadRequest();
            }

            try
            {
                _bll.Enterprises.Update(_mapper.Map(enterprise)!);
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
                if (!EnterpriseExists(id))
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

        // POST: api/Enterprises
        /// <summary>
        /// Create Enterprise
        /// </summary>
        /// <param name="enterprise">Enterprise data</param>
        /// <returns>Enterprise</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Enterprise>> PostEnterprise(Enterprise enterprise)
        {
            if (_bll.Enterprises == null)
            {
              return Problem("Entity set 'AppDbContext.Enterprises'  is null.");
            }

            try
            {
                _bll.Enterprises.Add(_mapper.Map(enterprise)!);
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

            return CreatedAtAction("GetEnterprise", new { id = enterprise.Id }, enterprise);
        }

        // DELETE: api/Enterprises/5
        /// <summary>
        /// Delete Enterprise client
        /// </summary>
        /// <param name="id">Enterprise id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnterprise(Guid id)
        {
            if (_bll.Enterprises == null)
            {
                return NotFound();
            }
            var enterprise = await _bll.Enterprises.FirstOrDefaultAsync(id);
            if (enterprise == null)
            {
                return NotFound();
            }

            _bll.Enterprises.Remove(enterprise);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool EnterpriseExists(Guid id)
        {
            return _bll.Enterprises.Exists(id);
        }
    }
}
