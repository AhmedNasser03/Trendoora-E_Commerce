using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface IDesireListItemsService
	{
		Task<DesireListItemResultDto> GetDesireListItemsById(string desireListId);
		Task<bool> AddItemsToDesireListAsync(string desireListId , string productId);
		Task<bool> DeleteItemFromDesireListAsync(string desireListId, string productId);
	}
}
