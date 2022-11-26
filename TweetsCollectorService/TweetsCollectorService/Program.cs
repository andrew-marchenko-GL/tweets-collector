using Jha.Services.TweetsCollectorService.Startup.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

// Add services to the container.
builder.Services.AddControllerServices();
builder.Services.AddTwitterServices(builder.Configuration);

#endregion

var app = builder.Build();

#region Configure application

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
