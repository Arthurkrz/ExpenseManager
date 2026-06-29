using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.Validators;
using ExpenseManager.ExternalServices;
using ExpenseManager.Infrastructure.Repositories;
using ExpenseManager.Service;
using ExpenseManager.Service.Utilities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManager.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExchangeService, ExchangeService>();
            return services;
        }

        public static IServiceCollection InjectRepositories(this IServiceCollection services)
        {
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            return services;
        }

        public static IServiceCollection InjectValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ExpenseValidator).Assembly);
            return services;
        }

        public static IServiceCollection InjectExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IExchangeHandler, ExchangeHandler>();
            return services;
        }
    }
}