using ExpenseManager.Core.Contracts.Mapping;
using ExpenseManager.Infrastructure;
using ExpenseManager.IOC;
using ExpenseManager.Web.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

var redisConnectionString = configuration["Redis:ConnectionString"] ?? 
    throw new InvalidOperationException("Redis Connection String not configured.");

var connectionString = configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Database Connection String not configured.");

services.AddSingleton<IConnectionMultiplexer>(c => 
    ConnectionMultiplexer.Connect(redisConnectionString));

services.AddDbContext<Context>(options => 
    options.UseSqlServer(connectionString));

services.AddMemoryCache();
services.AddControllersWithViews();

services.InjectValidators()
        .InjectExternalServices()
        .InjectServices()
        .InjectRepositories();

services.AddSingleton<IMap, MappingProfile>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();