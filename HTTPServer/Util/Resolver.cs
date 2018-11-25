using System;
using GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace HTTPServer.Util
{
    /**
     * Класс, который нужен для того, чтобы предоставить
     * функциональность IServiceProvider из ASP.NET core
     * для GraphQLResolver, который ожидает IDependencyResolver
     */
    public class Resolver : IDependencyResolver
    {
        private readonly IServiceProvider provider;

        public Resolver(IServiceProvider provider)
        {
            this.provider = provider;
        }
        
        public T Resolve<T>()
        {
            return provider.GetService<T>();
        }

        public object Resolve(Type type)
        {
            return provider.GetService(type);
        }
    }
}