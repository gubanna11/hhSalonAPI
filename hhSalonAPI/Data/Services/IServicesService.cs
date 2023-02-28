using hhSalonAPI.Data.Base;
using hhSalonAPI.Data.Models;
using hhSalonAPI.Data.ViewModels;

namespace hhSalonAPI.Data.Services
{
	public interface IServicesService : IEntityBaseRepository<Service>
	{
		Task<ServiceVM> GetServiceByIdWithGroupAsync(int id);
		Task<List<ServiceVM>> GetServicesByGroupIdAsync(int groupId);
		Task AddNewServiceAsync(ServiceVM newService);
		Task UpdateServiceAsync(ServiceVM serviceVM);
	}
}
