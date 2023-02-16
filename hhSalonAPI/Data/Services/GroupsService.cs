using hhSalonAPI.Data.Base;
using hhSalonAPI.Models;

namespace hhSalonAPI.Data.Services
{
	public class GroupsService : EntityBaseRepository<GroupOfServices>, IGroupsService
	{
		private readonly AppDbContext _context;

		public GroupsService(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task UpdateGroupAsync(GroupOfServices group)
		{
			var dbGroup = await GetByIdAsync(group.Id);
			if (dbGroup != null)
			{
				dbGroup.Name = group.Name;
				dbGroup.ImgUrl = group.ImgUrl;

				await _context.SaveChangesAsync();
			}
		}
	}
}
