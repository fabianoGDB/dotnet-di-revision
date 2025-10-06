Revision about .NET Dependency Injection

A quick, practical guide to DI, IoC, and DIP in .NET—what they are, how they relate, and how to use them correctly.

Table of Contents

What is DI?

What is IoC?

What is DIP?

How these ideas are related

DI in .NET (lifetimes & examples)

DbContext notes

Service Locator (SL)

Common pitfalls & best practices

What is DI?

Dependency Injection (DI) is not a design pattern like Singleton; it’s a technique for supplying an object’s dependencies from the outside (constructor/parameters) instead of creating them inside the object.

Implements the IoC principle in practice.

Promotes low coupling, easier testing, and clearer boundaries.

Example (constructor injection):

public interface IEmailSender { Task SendAsync(string to, string body); }

public sealed class SmtpEmailSender : IEmailSender
{
public Task SendAsync(string to, string body) => Task.CompletedTask; // impl...
}

public sealed class WelcomeService
{
private readonly IEmailSender \_email;
public WelcomeService(IEmailSender email) => \_email = email;

    public Task SendWelcome(string user) => _email.SendAsync(user, "Welcome!");

}

What is IoC?

Inversion of Control (IoC) is a broader principle: instead of your code calling everything directly, a framework or container controls the flow and provides dependencies.

Moves creation/coordination to an external component (e.g., the .NET IoC container).

Your code focuses on behavior, not wiring.

What is DIP?

Dependency Inversion Principle (DIP) says:

High-level modules must not depend on low-level modules.

Both should depend on abstractions (interfaces).

Abstractions should not depend on details.

DI helps you enforce DIP by having classes depend on interfaces instead of concrete implementations.

How these ideas are related
DIP (principle) -> tells you to depend on abstractions
IoC (principle) -> hands control of wiring to an external container
DI (technique) -> practical way to achieve IoC & uphold DIP

DI in .NET (lifetimes & examples)
Built-in container (Microsoft.Extensions.DependencyInjection)

Register services in Program.cs:

var builder = WebApplication.CreateBuilder(args);

// Lifetimes
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>(); // new instance every resolve
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // one per scope (web request)
builder.Services.AddSingleton<IClock, SystemClock>(); // one for the app lifetime

var app = builder.Build();

app.MapPost("/welcome", async (WelcomeService svc, string email) =>
{
await svc.SendWelcome(email);
return Results.Ok();
});

app.Run();

Lifetimes at a glance
Lifetime Meaning (ASP.NET) Typical use Avoid when…
AddTransient New instance per resolve Lightweight, stateless services, formatters You need to cache state or share resources
AddScoped One per request/scope Business logic, UoW, DbContext You’re outside a scope (create one!)
AddSingleton One for the entire app lifetime Config, clocks, stateless caches, mappers The type depends on scoped services or is not thread-safe

For non-web apps, create scopes manually:

using var scope = app.Services.CreateScope();
var svc = scope.ServiceProvider.GetRequiredService<WelcomeService>();

DbContext notes

AddDbContext<TContext>(...) registers Scoped by default (recommended).

DbContext is not thread-safe. Never register as Singleton.

Each scope/request should represent a unit of work.

If you need performance, consider AddDbContextPool<T>() (still scoped, but instances come from a pool).

builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

Service Locator (SL)

Service Locator = retrieving dependencies via a global container (IServiceProvider.GetService(...)) inside your business classes.

It hides dependencies and increases coupling.

Harder to test; violates DIP’s spirit.

Prefer constructor injection. If you absolutely must resolve at runtime (rare), confine it to composition/wiring layers, not domain logic.

Bad (avoid):

public sealed class BadService
{
private readonly IServiceProvider \_provider;
public BadService(IServiceProvider provider) => \_provider = provider;

    public void DoWork()
    {
        var repo = _provider.GetRequiredService<IRepo>(); // Service Locator
        // ...
    }

}

Good:

public sealed class GoodService
{
private readonly IRepo \_repo;
public GoodService(IRepo repo) => \_repo = repo;
}

Common pitfalls & best practices

✅ Depend on interfaces, not concretes (DIP).

✅ Keep services stateless when possible.

✅ Do not capture scoped services inside singletons.

✅ Use options pattern for configuration (IOptions<T>).

✅ Prefer constructor injection over property/field injection.

✅ For background tasks, use IHostedService/BackgroundService and scopes inside ExecuteAsync.

⚠️ Avoid Service Locator; it hides dependencies.

⚠️ Do not make DbContext a Singleton.

⚠️ Be careful with HttpClient: use AddHttpClient() and typed clients (sockets exhaustion).
