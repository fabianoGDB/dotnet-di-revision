using DependencyInjectionLifetimeSample.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.TryAddTransient<IService, PrimaryService>();
//builder.Services.TryAddTransient<IService, PrimaryService>();
//builder.Services.TryAddTransient<IService, SecondaryService>();

//var descriptoriptors = new ServiceDescriptor(typeof(IService), typeof(PrimaryService), ServiceLifetime.Transient);
//builder.Services.TryAddEnumerable(descriptoriptors);


builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>());
//builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, SecondaryService>());

var app = builder.Build();

app.MapGet("/", (IEnumerable<IService> services) =>
{
    return Results.Ok(services.Select(x => x.GetType().Name));
});

app.Run();


public interface IService { }