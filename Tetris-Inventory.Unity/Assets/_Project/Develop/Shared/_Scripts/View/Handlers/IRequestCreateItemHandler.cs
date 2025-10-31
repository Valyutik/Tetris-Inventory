using System;

namespace _Project.Services
{
    public interface IRequestCreateItemHandler
    {
        event Action OnRequestCreateItem;
    }
}