using System;
 

namespace Dream_Team_Assessment.Services.Interfaces
{
	public interface IStockService
	{
        Task<string> GetDailyStockPricesAsync(string symbol);
    }
}

