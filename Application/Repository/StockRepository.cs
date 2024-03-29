using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.DTOs.Stock;
using Application.Helpers;
using Application.Interfaces;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository
{
    // Example of implementing repository pattern and dependency injection
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public StockRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _dbContext.Stock.AddAsync(stockModel);
            await _dbContext.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stockModel = await _dbContext.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }

            _dbContext.Stock.Remove(stockModel); // Remove does not have the async version of it
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
            // Explination of the `ThenInclude`
            // Note: The stock model is linked by one to many relationship with the comment and the comment is linked with
            // one to one with the comment. When we hit the stock endpoint we want to visualize the nested user 
            // in the comment in the stock endpoint response. To do so we use 
            // `var stocks = _dbContext.Stock.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();`
            var stocks = _dbContext.Stock.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();

            // Example of filtering by Symbol 
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            // Example of filtering by CompanyName
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            
            // Example of sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDesc ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
                else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDesc ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }

            // Example of pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            stocks = stocks.Skip(skipNumber).Take(query.PageSize);
            
            // Here when we are going to execute the queery
            return await stocks.ToListAsync();
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbContext.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _dbContext.Stock.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            var stockModel = await _dbContext.Stock.FirstOrDefaultAsync(stock => stock.Id == id);
            return stockModel != null;
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDTO stockDTO)
        {
            var stockModel = await _dbContext.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }

            stockModel.Symbol = stockDTO.Symbol;
            stockModel.CompanyName = stockDTO.CompanyName;
            stockModel.Purchase = stockDTO.Purchase;
            stockModel.LastDiv = stockDTO.LastDiv;
            stockModel.Industry = stockDTO.Industry;
            stockModel.MarketCap = stockDTO.MarketCap;

            await _dbContext.SaveChangesAsync();

            return stockModel;
        }
    }
}