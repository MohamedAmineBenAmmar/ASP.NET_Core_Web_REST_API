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
        public IActionResult GetAll()
        {
            // We need to call ToList to execute the query
            // Check the differed execution behaviour
            var stocks = _dbContext.Stock.ToList()
            // The Select method is the .NET of the JS map function
            .Select(s => s.ToStockDTO());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _dbContext.Stock.Find(id);
            if (stock == null)
            {
                // The NotFound is a form of an IActionResult
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDTO stockDTO)
        {
            var stockModel = stockDTO.ToStockFromCreateDTO();
            _dbContext.Add(stockModel);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateStockDTO)
        {
            var stockModel = _dbContext.Stock.FirstOrDefault(x => x.Id == id);
            if (stockModel == null){
                return NotFound();
            }

            stockModel.Symbol = updateStockDTO.Symbol;
            stockModel.CompanyName = updateStockDTO.CompanyName;
            stockModel.Purchase = updateStockDTO.Purchase;
            stockModel.LastDiv = updateStockDTO.LastDiv;
            stockModel.Industry = updateStockDTO.Industry;
            stockModel.MarketCap = updateStockDTO.MarketCap;

            _dbContext.SaveChanges();
            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockModel = _dbContext.Stock.FirstOrDefault(x => x.Id == id);
            if (stockModel == null){
                return NotFound();
            }
            _dbContext.Remove(stockModel);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}