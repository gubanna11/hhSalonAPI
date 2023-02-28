using hhSalonAPI.Data.Base;
using hhSalonAPI.Data.Models;
using hhSalonAPI.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace hhSalonAPI.Data.Services
{
	public class ServicesService : EntityBaseRepository<Service>, IServicesService
	{
		private readonly AppDbContext _context;
		public ServicesService(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<ServiceVM>> GetServicesByGroupIdAsync(int groupId)
		{ 
			var services = await _context.Services.Include(s => s.Service_Group).Where(s => s.Service_Group.GroupId == groupId).ToListAsync();

			List<ServiceVM> servicesVM = new List<ServiceVM>();

			foreach (var service in services)
			{
				ServiceVM serviceVM = new ServiceVM()
				{
					Id = service.Id,
					Name = service.Name,
					Price = service.Price,
					GroupId = service.Service_Group.GroupId
				};
				servicesVM.Add(serviceVM);
			}
			return servicesVM;
		}


		public async Task AddNewServiceAsync(ServiceVM newService)
		{
			Service service = new Service
			{
				Name = newService.Name,
				Price = newService.Price
			};

			await _context.Services.AddAsync(service);
			await _context.SaveChangesAsync();

			Service_Group serviceGroup = new Service_Group
			{
				ServiceId = service.Id,
				GroupId = newService.GroupId
			};

			await _context.Services_Groups.AddAsync(serviceGroup);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateServiceAsync(ServiceVM serviceVM)
		{
			var service = await GetByIdAsync(serviceVM.Id);

			if (service != null)
			{
				service.Name = serviceVM.Name;
				service.Price = serviceVM.Price;
			}

			var service_group = _context.Services_Groups.Where(sg => sg.ServiceId == service.Id).FirstOrDefault();
			_context.Services_Groups.Remove(service_group);

			Service_Group newService_Group = new Service_Group()
			{
				ServiceId = service.Id,
				GroupId = serviceVM.GroupId
			};

			await _context.Services_Groups.AddAsync(newService_Group);
			await _context.SaveChangesAsync();
		}

		public async Task<ServiceVM> GetServiceByIdWithGroupAsync(int id)
		{

			//var service = await _context.Services.Where(s => s.Id == id)
			//	.Include(s => s.Service_Group).ThenInclude(sg => sg.Group).FirstOrDefaultAsync();

			var groupId = await _context.Services_Groups.Where(sg => sg.ServiceId == id).Select(sg => sg.GroupId).FirstOrDefaultAsync();


			var service = await _context.Services.Where(s => s.Id == id).FirstOrDefaultAsync();

			return new ServiceVM() { Id= id, Name = service.Name, Price = service.Price, GroupId = groupId };
		}
	}
}
