using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.Validators;
using ExpenseManager.ExternalServices;
using ExpenseManager.ExternalServices.Settings;
using ExpenseManager.Infrastructure.Repositories;
using ExpenseManager.Service;
using ExpenseManager.Service.Utilities;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace ExpenseManager.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.InjectServices()
                    .InjectRepositories()
                    .InjectValidators()
                    .InjectSettings(configuration)
                    .InjectExternalServices(configuration)
                    .InjectCache(configuration);

            return services;
        }

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<IExpenseSummaryService, ExpenseSummaryService>();

            return services;
        }

        public static IServiceCollection InjectRepositories(this IServiceCollection services)
        {
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IExpenseSummaryRepository, ExpenseSummaryRepository>();
            
            return services;
        }

        public static IServiceCollection InjectValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ExpenseValidator).Assembly);
            
            return services;
        }

        public static IServiceCollection InjectSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExchangeRatesSettings>(
                configuration.GetSection("ExchangeRates"));

            services.Configure<ExchangeApiSettings>(
                configuration.GetSection("ExchangeApi"));

            services.Configure<RedisSettings>(
                configuration.GetSection("Redis"));

            return services;
        }

        public static IServiceCollection InjectExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            var exchangeProvider = configuration["ExchangeRates:Provider"] ?? "Fixed";

            if (exchangeProvider.Equals("Api", StringComparison.OrdinalIgnoreCase))
                services.AddHttpClient<IExchangeRateProvider, ApiExchangeRateProvider>();

            else services.AddScoped<IExchangeRateProvider, FixedExchangeRateProvider>();
            
            return services;
        }

        public static IServiceCollection InjectCache(this IServiceCollection services, IConfiguration configuration)
        {
            var exchangeProvider = configuration["ExchangeRates:Provider"] ?? "Fixed";

            if (!configuration.GetValue<bool>("Redis:Enabled") || 
                !exchangeProvider.Equals("Api", StringComparison.OrdinalIgnoreCase))
            {
                services.AddSingleton<ICacheService, MemoryCacheService>();

                return services;
            }

            var redisConnectionString = configuration["Redis:ConnectionString"] ??
                throw new InvalidOperationException("Redis connection string not configured.");

            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var options = ConfigurationOptions.Parse(redisConnectionString);
                options.AbortOnConnectFail = false;

                return ConnectionMultiplexer.Connect(options);
            });

            services.AddSingleton<ICacheService, RedisCacheService>();

            return services;
        }
    }
}