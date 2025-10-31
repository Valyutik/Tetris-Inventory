namespace _Project.Services
{
    public interface IInputService : IService
    {
        IDeviceInputHandler InputHandler { get; }
    }
}