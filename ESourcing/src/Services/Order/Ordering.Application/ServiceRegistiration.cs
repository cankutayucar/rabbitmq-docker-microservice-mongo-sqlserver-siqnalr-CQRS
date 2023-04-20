using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Mappers;
using Ordering.Application.PipelimeBehaviours;
using System.Reflection;

namespace Ordering.Application
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
                .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            var config = new MapperConfiguration(cnfig =>
            {
                cnfig.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cnfig.AddProfile<OrderMappingProfile>();
            });
            var mapper = config.CreateMapper();
        }
    }
}
