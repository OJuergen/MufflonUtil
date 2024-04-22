using System;
using System.Collections.Generic;

namespace MufflonUtil
{
    public static class ServiceProvider
    {
        private static readonly Dictionary<Type, object> Services = new();

        public static T Get<T>()
        {
            if (Services.TryGetValue(typeof(T), out object service))
            {
                return (T)service;
            }

            throw new InvalidOperationException($"Service of type {typeof(T)} not found");
        }

        public static void Set<T>(T service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            Services[typeof(T)] = service;
        }
    }
}