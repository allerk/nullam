using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Public.DTO.v1;
using App.Public.Mappers;
using AutoMapper;

namespace WebApp.ApiControllers
{
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly EventMapper _mapper;

        public EventsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new EventMapper(mapper);
        }

        // GET: api/Events
        /// <summary>
        /// Get all Events
        /// </summary>
        /// <returns>Events list</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            // var res = _mapper.Map(await _bll.Events.GetAllSortedByTimeAsync());
            return (await _bll.Events.GetAllAsync()).Select(x => _mapper.Map(x)!).ToList();
            // return res;
        }

        // GET: api/Events/5
        /// <summary>
        /// Get Event
        /// </summary>
        /// <param name="id">Event id</param>
        /// <returns>Event</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(Guid id)
        {
            if (_bll.Events == null)
            {
              return NotFound();
            }
            var res = _mapper.Map(await _bll.Events.FirstOrDefaultAsync(id));

            if (res == null)
            {
                return NotFound();
            }

            return res;
        }

        // PUT: api/Events/5
        /// <summary>
        /// Update Event data
        /// </summary>
        /// <param name="id">Event id</param>
        /// <param name="eventEntity">Changed Event data</param>
        /// <returns>Updated Event data</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(Guid id, Event eventEntity)
        {
            if (id != eventEntity.Id)
            {
                return BadRequest();
            }

            _bll.Events.Update(_mapper.Map(eventEntity)!);
            
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        /// <summary>
        /// Create Event
        /// </summary>
        /// <param name="eventEntity">Event data</param>
        /// <returns>New Event</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event eventEntity)
        {
            if (_bll.Events == null)
            {
              return Problem("Entity set 'AppDbContext.Events'  is null.");
            }
            _bll.Events.Add(_mapper.Map(eventEntity)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = eventEntity.Id }, eventEntity);
        }

        // DELETE: api/Events/5
        /// <summary>
        /// Delete Event
        /// </summary>
        /// <param name="id">Event id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            if (_bll.Events == null)
            {
                return NotFound();
            }

            var eventEntity = await _bll.Events.FirstOrDefaultAsync(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            _bll.Events.Remove(eventEntity);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(Guid id)
        {
            return _bll.Events.Exists(id);
        }
    }
}
