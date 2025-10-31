namespace _Project.Services
{
    public interface IInventoryHandler : IRequestCreateItemHandler, IRequestTakeItemHandler, IRequestDeleteItemHandler,
        IRequestPlaceItemHandler
    {

    }
}