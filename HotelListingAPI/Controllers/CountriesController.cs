using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using AutoMapper;
using HotelListingAPI.Contracts;

namespace HotelListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        //private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;
        
        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
        {
            //_context = context;
            this._mapper = mapper;
            _countriesRepository = countriesRepository;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _countriesRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            //var country = await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);
            var country = await _countriesRepository.GetDetails(id);

            if (country == null)
            {
                return NotFound();
            }

            var countryDto = _mapper.Map<GetCountryDetailsDto>(country);

            return Ok(countryDto);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updatecountry)
        {
            if (id != updatecountry.Id)
            {
                return BadRequest();
            }

            //_context.Entry(country).State = EntityState.Modified;
            //var country = await _context.Countries.FindAsync(id);
          
            var countryData = await _countriesRepository.GetAsync(id);
            if (countryData ==  null)
            {
                return NotFound();
            }
            
            _mapper.Map(updatecountry, countryData); // maps values from first argument to second argument

            try
            {
                //await _context.SaveChangesAsync();
                await _countriesRepository.UpdateAsync(countryData);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await _countriesRepository.Exists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountry)
        {

            var country = _mapper.Map<Country>(createCountry);

            //_context.Countries.Add(country);
            country = await _countriesRepository.AddAsync(country);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            //var country = await _context.Countries.FindAsync(id);
            var country = await _countriesRepository.Exists(id);
            if (!country)
            {
                return NotFound();
            }

            await _countriesRepository.DeleteAsync(id);
            //_context.Countries.Remove(country);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}
