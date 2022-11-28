using Hangfire;
using Jha.Services.TweetsCollectorService.Startup.ApplicationBuilderExtensions;
using Jha.Services.TweetsCollectorService.Startup.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

// Add services to the container.
builder.Services.AddControllerServices();
builder.Services.AddTwitterServices(builder.Configuration);
builder.Services.AddHangfireServices();

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
app.UseStaticFiles();
app.UseAndConfigureHanfire();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});

#endregion

app.Run();
