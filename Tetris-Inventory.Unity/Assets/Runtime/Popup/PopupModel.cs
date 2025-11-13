using System.Collections.Generic;
using System;

namespace Runtime.Popup
{
    public sealed class PopupModel
    {
        public event Action<PopupData> OnPopupOpened;
        public event Action<PopupData> OnPopupClosed;
        
        private readonly Stack<PopupData> _popupStack = new();

        public bool HasActivePopup => _popupStack.Count > 0;
        
        public PopupData CurrentPopup => _popupStack.Count > 0 ? _popupStack.Peek() : null;

        public void Open(PopupData data)
        {
            _popupStack.Push(data);
            OnPopupOpened?.Invoke(data);
        }

        public void CloseCurrent(PopupData data)
        {
            if (_popupStack.Count == 0)
            {
                return;
            }
            
            var closed = _popupStack.Pop();
            OnPopupClosed?.Invoke(closed);
        }
        
        public void CloseAll()
        {
            while (_popupStack.Count > 0)
            {
                var popup = _popupStack.Pop();
                OnPopupClosed?.Invoke(popup);
            }
        }
    }
}