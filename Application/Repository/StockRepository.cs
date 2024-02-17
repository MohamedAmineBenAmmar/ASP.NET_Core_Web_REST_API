using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.DTOs.Stock;
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
            var stockModel= await _dbContext.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }

            _dbContext.Stock.Remove(stockModel); // Remove does not have the async version of it
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _dbContext.Stock.Include(c => c.Comments).ToListAsync();
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