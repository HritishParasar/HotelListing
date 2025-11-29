using HotelListing.API.Data;
using HotelListing.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public CountriesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("getAllCountries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _dbContext.Countries.ToListAsync();
            return Ok(countries);
        }
        [HttpGet("getCountryByID/{id}")]
        public async Task<IActionResult> GetCountryByID(int id)
        {
            var country = await _dbContext.Countries.FindAsync(id);
            if(country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }
        [HttpPost("addCountry")]
        public async Task<IActionResult> AddCountry(Country country)
        {
            await _dbContext.Countries.AddAsync(country);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCountryByID), new { id = country.Id }, country);
        }
        [HttpPut("updateCountry/{id}")]
        public async Task<IActionResult> UpdateCountry(int id, Country country)
        {
            if(id != country.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(country).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Countries.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Successfully Updated!");
        }
        [HttpDelete("deleteCountry/{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _dbContext.Countries.FindAsync(id);
            if(country == null)
            {
                return NotFound();
            }
            _dbContext.Countries.Remove(country);
            await _dbContext.SaveChangesAsync();
            return Ok("Successfully Deleted!");
        }
    }
}
