using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Stock;
using Application.Models;

namespace Application.Mappers
{
    public static class StockMappers
    {
        // this keyword in the ToStockDTO refers to that this method is an extension method
        public static StockDTO ToStockDTO(this Stock stockModel){
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDTO stockDTO)
        {
            return new Stock{
                Symbol = stockDTO.Symbol,
                CompanyName = stockDTO.CompanyName,
                Purchase = stockDTO.Purchase,
                LastDiv = stockDTO.LastDiv,
                Industry = stockDTO.Industry,
                MarketCap = stockDTO.MarketCap
            };
        }
    }
}