using ExpenseManager.Infrastructure;
using ExpenseManager.IOC;
using ExpenseManager.Web.Mapping;
using ExpenseManager.Web.Mapping.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

var connectionString = configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Database connection string not configured.");

services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));

services.AddMemoryCache();
services.AddControllersWithViews();

services.InjectDependencies(configuration);

services.AddSingleton<IObjectMapper, ObjectMapper>();
services.AddSingleton<IPaginatedViewModelMapper, PaginatedViewModelMapper>();

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