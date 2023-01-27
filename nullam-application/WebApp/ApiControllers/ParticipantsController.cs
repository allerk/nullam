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
    public class ParticipantsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly ParticipantMapper _mapper;

        public ParticipantsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new ParticipantMapper(mapper);
        }

        // GET: api/Participants
        /// <summary>
        /// Get all Participants
        /// </summary>
        /// <returns>Participants list</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants()
        {
            if (_bll.Participants == null)
            {
              return NotFound();
            }
            return (await _bll.Participants.GetAllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        // GET: api/Participants/5
        /// <summary>
        /// Get Participant
        /// </summary>
        /// <param name="id">Participant id</param>
        /// <returns>Participant</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Participant>> GetParticipant(Guid id)
        {
            if (_bll.Participants == null)
            {
              return NotFound();
            }
            var participant = _mapper.Map(await _bll.Participants.FirstOrDefaultAsync(id));

            if (participant == null)
            {
                return NotFound();
            }

            return participant;
        }

        // PUT: api/Participants/5
        /// <summary>
        /// Update Participant data
        /// </summary>
        /// <param name="id">Participant id</param>
        /// <param name="participant">Changed Participant data</param>
        /// <returns>Updated Participant data</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipant(Guid id, Participant participant)
        {
            if (id != participant.Id)
            {
                return BadRequest();
            }

            _bll.Participants.Update(_mapper.Map(participant)!);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
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

        // POST: api/Participants
        /// <summary>
        /// Create Participant
        /// </summary>
        /// <param name="participant">Participant data</param>
        /// <returns>New Participant</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Participant>> PostParticipant(Participant participant)
        {
            if (_bll.Participants == null)
            {
              return Problem("Entity set 'AppDbContext.Participants'  is null.");
            }
            _bll.Participants.Add(_mapper.Map(participant)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetParticipant", new { id = participant.Id }, participant);
        }

        // DELETE: api/Participants/5
        /// <summary>
        /// Delete Participant
        /// </summary>
        /// <param name="id">Participant id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(Guid id)
        {
            if (_bll.Participants == null)
            {
                return NotFound();
            }
            var participant = await _bll.Participants.FirstOrDefaultAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            _bll.Participants.Remove(participant);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipantExists(Guid id)
        {
            return _bll.Participants.Exists(id);
        }
    }
}
