using System;
using System.Collections.Generic;

namespace _Project.Services
{
    public class ServiceLocator : IServiceLocator
    {
        public static IServiceLocator Instance { get; private set; }
        
        private readonly Dictionary<Type, IService> _services;

        public ServiceLocator()
        {
            _services = new Dictionary<Type, IService>(); 
            
            Instance = this;
        } 
        
        public void RegisterService<TService>(IService service) where TService : class
        {
            if (_services.ContainsKey(typeof(TService))) return;
            
            _services.Add(service.GetType(), service);
        }

        public TService GetService<TService>() where TService : class, IService
        {
            _services.TryGetValue(typeof(TService), out var service);
            
            return service as TService;
        }
    }
}