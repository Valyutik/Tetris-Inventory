using System;
using UnityEngine;

namespace _Project.Services
{
    public interface IDeviceInputHandler : IDisposable
    {
        public event Action OnPressed;

        public event Action OnReleased;

        public event Action<Vector2> OnChangePointerPosition;

        public Vector2 PointerPosition { get; }
    }
}