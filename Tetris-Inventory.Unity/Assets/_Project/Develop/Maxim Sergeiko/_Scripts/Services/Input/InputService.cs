namespace _Project.Services
{
    public class InputService : IInputService
    {
        public IDeviceInputHandler InputHandler { get; private set; }

        public InputService(IDeviceInputHandler inputHandler) => InputHandler = inputHandler;

        public void SwitchInputHandler(IDeviceInputHandler handler) => InputHandler = handler;
    }
}