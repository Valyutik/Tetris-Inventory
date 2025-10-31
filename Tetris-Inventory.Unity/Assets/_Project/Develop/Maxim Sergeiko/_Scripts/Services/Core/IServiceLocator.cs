namespace _Project.Services
{
    public interface IServiceLocator
    {
        TService GetService<TService>() where TService : class, IService;
    }
}