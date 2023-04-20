using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Repositories;
using Ordering.Domain.Repositories.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public static class ServiceIntegrator
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services                
                .AddTransient(typeof(IRepository<>), typeof(Repository<>))
                .AddTransient<IOrderRepository, OrderRepository>();
        }
    }
}
