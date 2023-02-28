using hhSalonAPI.Data;
using hhSalonAPI.Data.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddEntityFrameworkMySql().AddDbContext<AppDbContext>(options => {
	options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnectionString"), 
		new MySqlServerVersion(new Version(8, 0, 11)));
});

builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IServicesService, ServicesService>();


builder.Services.AddCors(options => options.AddPolicy(name: "HHOrigins",
		policy =>
		{
			policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
			//policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
		}));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("HHOrigins");

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

AppDbInitializer.Seed(app);

app.Run();
