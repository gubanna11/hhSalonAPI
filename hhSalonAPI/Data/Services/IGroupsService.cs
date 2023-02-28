using hhSalonAPI.Data.Base;
using hhSalonAPI.Data.Models;

namespace hhSalonAPI.Data.Services
{
	public interface IGroupsService : IEntityBaseRepository<GroupOfServices>
	{
		Task UpdateGroupAsync(GroupOfServices group);
	}
}
