using System.Text.Json.Serialization;
using Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomerRepository();

builder.Services
   .AddControllers()
   .AddJsonOptions(options => options
		   .JsonSerializerOptions
		   .Converters
		   .Add(new JsonStringEnumConverter())
		)
	;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseDarkSwaggerUI();
}

app.MapControllers();

app.InitializeSharedDataStore();
app.Run();