using System;

namespace _Project.Services
{
    public interface IRequestTakeItemHandler
    {
        event Action OnRequestTakeItem;
    }
}