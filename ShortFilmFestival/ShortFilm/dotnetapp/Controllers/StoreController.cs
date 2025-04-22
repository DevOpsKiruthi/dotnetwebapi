using dotnetapp.Exceptions;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("/")]
    public class StoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /getAllStoreitem
        [HttpGet("getAllStoreitem")]
        public async Task<ActionResult<IEnumerable<Store>>> GetAllStoreItems()
        {
            try
            {
                var storeItems = await _context.Stores.ToListAsync();
                return Ok(storeItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // POST /addStoreitem
        [HttpPost("addStoreitem")]
        public async Task<ActionResult<Store>> AddStoreItem(Store store)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (store.Price < 0)
                {
                    throw new PriceItemException("Price cannot be less than 0.");
                }

                _context.Stores.Add(store);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAllStoreItems), new { id = store.Id }, store);
            }
            catch (PriceItemException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
