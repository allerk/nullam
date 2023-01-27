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
    public class PaymentTypesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PaymentTypeMapper _mapper;

        public PaymentTypesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new PaymentTypeMapper(mapper);
        }

        // GET: api/PaymentTypes
        /// <summary>
        /// Get all PaymentTypes
        /// </summary>
        /// <returns>PaymentTypes list</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentType>>> GetPaymentTypes()
        {
            if (_bll.PaymentTypes == null)
            {
              return NotFound();
            }
            return (await _bll.PaymentTypes.GetAllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        // GET: api/PaymentTypes/5
        /// <summary>
        /// Get PaymentType
        /// </summary>
        /// <param name="id">PaymentType id</param>
        /// <returns>PaymentType</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentType>> GetPaymentType(Guid id)
        {
            if (_bll.PaymentTypes == null)
            {
              return NotFound();
            }
            var paymentType = _mapper.Map(await _bll.PaymentTypes.FirstOrDefaultAsync(id));

            if (paymentType == null)
            {
                return NotFound();
            }

            return paymentType;
        }

        // PUT: api/PaymentTypes/5
        /// <summary>
        /// Update PaymentType data
        /// </summary>
        /// <param name="id">PaymentType id</param>
        /// <param name="paymentType">Changed PaymentType data</param>
        /// <returns>Updated PaymentType data</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentType(Guid id, PaymentType paymentType)
        {
            if (id != paymentType.Id)
            {
                return BadRequest();
            }

            _bll.PaymentTypes.Update(_mapper.Map(paymentType)!);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentTypeExists(id))
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

        // POST: api/PaymentTypes
        /// <summary>
        /// Create PaymentType
        /// </summary>
        /// <param name="paymentType">PaymentType data</param>
        /// <returns>PaymentType</returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentType>> PostPaymentType(PaymentType paymentType)
        {
            if (_bll.PaymentTypes == null)
            {
              return Problem("Entity set 'AppDbContext.PaymentTypes'  is null.");
            }
            _bll.PaymentTypes.Add(_mapper.Map(paymentType)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPaymentType", new { id = paymentType.Id }, paymentType);
        }

        // DELETE: api/PaymentTypes/5
        /// <summary>
        /// Delete PaymentType
        /// </summary>
        /// <param name="id">PaymentType id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentType(Guid id)
        {
            if (_bll.PaymentTypes == null)
            {
                return NotFound();
            }
            var paymentType = await _bll.PaymentTypes.FirstOrDefaultAsync(id);
            if (paymentType == null)
            {
                return NotFound();
            }

            _bll.PaymentTypes.Remove(paymentType);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentTypeExists(Guid id)
        {
            return _bll.PaymentTypes.Exists(id);
        }
    }
}
