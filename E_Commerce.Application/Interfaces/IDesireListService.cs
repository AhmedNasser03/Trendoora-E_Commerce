using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface IDesireListService
	{
		Task<DesireListResultDto> GetDesireListById(string id);
		Task<bool> AddDesireListAsync(DesireListDto desireListDto);
		Task<IEnumerable<DesireListResultDto>> GetAllDesireListsAsync();
		Task<bool> UpdateDesireListAsync(string desireListId, DesireListDto desireListDto);
		Task<bool> DeleteDesireListAsync(string id);
	}
}
