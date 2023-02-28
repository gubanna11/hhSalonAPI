using hhSalonAPI.Data;
using hhSalonAPI.Data.Services;
using hhSalonAPI.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServicesController : ControllerBase
	{
		private readonly IServicesService _servicesService;
		private readonly IGroupsService _groupsService;

		public ServicesController(IServicesService servicesService, AppDbContext context, IGroupsService groupsService)
		{
			_servicesService = servicesService;
			_groupsService = groupsService;
		}


		[HttpGet("{groupId}")]
		public async Task<ActionResult<List<ServiceVM>>> GetServicesByGroupId(int groupId)
		{
			return Ok(await _servicesService.GetServicesByGroupIdAsync(groupId));

			//var service = await context.Services.ToListAsync();
			//return Ok(service);
		}

		[HttpPost]
		public async Task<ActionResult<List<ServiceVM>>> CreateService(ServiceVM newService)
		{
			var groups = await _groupsService.GetAllAsync();
			
			try
			{
				await _servicesService.AddNewServiceAsync(newService);
			}
			catch (Exception)
			{
				return BadRequest();
			}

			return Ok(await _servicesService.GetServicesByGroupIdAsync(newService.GroupId));
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<List<ServiceVM>>> DeleteService(int id)
		{
			var serviceVM = await _servicesService.GetServiceByIdWithGroupAsync(id);
			
			await _servicesService.DeleteAsync(id);

			return Ok(await _servicesService.GetServicesByGroupIdAsync(serviceVM.GroupId));
		}


	}
}
