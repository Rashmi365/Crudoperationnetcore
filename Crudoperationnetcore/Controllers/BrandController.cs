using Crudoperationnetcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Crudoperationnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {

        private readonly BrandContext _dbContext;
        public BrandController(BrandContext dbContext) {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            if (_dbContext.Brands == null)
            {
                return BadRequest();
            }
            return await _dbContext.Brands.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrands(int id)
        {
            if (_dbContext.Brands == null)
            {
                return BadRequest();
            }
            var brand = await _dbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return BadRequest();
            }
            return brand;
        }
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBrands), new { id = brand.Id }, brand);
        }
        [HttpPut]
        public async Task<ActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(brand).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }

            }
            return Ok();
        } 
        private bool BrandAvailable(int id)
        {
            return(_dbContext.Brands?.Any(x => x.Id == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id) {
            if (_dbContext.Brands== null) { 
                return NotFound();
            }
            var brand = await _dbContext.Brands.FindAsync(id);
            if (brand == null) { return NotFound(); }
            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();   
            return Ok();
        }
    }
}
