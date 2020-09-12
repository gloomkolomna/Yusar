using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Yusar.Core;
using Yusar.Core.Entities;

namespace Yusar.Client
{
    public class DependencyResolver
    {
        public DependencyResolver()
        {
            IServiceCollection services = new ServiceCollection();

            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; }

        private void ConfigureServices(IServiceCollection services)
        {
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IYusarRepository<SimpleString>, YusarRepository<SimpleString>>();
        }
    }
}
