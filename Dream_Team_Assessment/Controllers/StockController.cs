using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dream_Team_Assessment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
 

namespace Dream_Team_Assessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }


        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetStockPrices(string symbol)
        {
            var json = await _stockService.GetDailyStockPricesAsync(symbol);
            return Content(json, "application/json");
        }
    }
}

