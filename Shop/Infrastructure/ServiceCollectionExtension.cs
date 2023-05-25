using Repository;
using System.Reflection;

namespace Shop.API
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection serviceCollection)
        {
            var assembly = Assembly.GetAssembly(typeof(IRegisterScoped));
            if (assembly is null)
                return;

            var services = assembly.GetTypes().Where(x =>
            x.IsClass && (typeof(IRegisterScoped).IsAssignableFrom(x) || typeof(IRegisterHttpClient).IsAssignableFrom(x))).ToList();

            foreach (var service in services)
            {
                var interfac = service.GetInterfaces().FirstOrDefault();
                if (interfac is null)
                    continue;

                if (typeof(IRegisterScoped).IsAssignableFrom(service))
                    serviceCollection.AddScoped(interfac, service);
            }
            serviceCollection.AddScoped(typeof(IDapperService<,>), typeof(DapperService<,>));
            serviceCollection.AddScoped(typeof(IProductService), typeof(ProductService));
        }
    }
}