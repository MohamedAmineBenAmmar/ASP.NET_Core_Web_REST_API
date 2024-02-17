using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.DTOs.Stock;
using Application.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController: ControllerBase
    {
        // The creation of an attribute that is immutable
        private readonly ApplicationDBContext _dbContext;

        public StockController(ApplicationDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // The creation of our endpoints
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // We need to call ToList to execute the query
            // Check the differed execution behaviour
            var stocks = await _dbContext.Stock.ToListAsync();
            // The Select method is the .NET of the JS map function
            var stocksDTO = stocks.Select(s => s.ToStockDTO());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _dbContext.Stock.FindAsync(id);
            if (stock == null)
            {
                // The NotFound is a form of an IActionResult
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDTO)
        {
            var stockModel = stockDTO.ToStockFromCreateDTO();
            await _dbContext.AddAsync(stockModel);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateStockDTO)
        {
            var stockModel = await _dbContext.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null){
                return NotFound();
            }

            stockModel.Symbol = updateStockDTO.Symbol;
            stockModel.CompanyName = updateStockDTO.CompanyName;
            stockModel.Purchase = updateStockDTO.Purchase;
            stockModel.LastDiv = updateStockDTO.LastDiv;
            stockModel.Industry = updateStockDTO.Industry;
            stockModel.MarketCap = updateStockDTO.MarketCap;

            await _dbContext.SaveChangesAsync();
            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _dbContext.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null){
                return NotFound();
            }
            _dbContext.Remove(stockModel);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}