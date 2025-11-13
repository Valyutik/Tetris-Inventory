using Runtime.Systems.ContentManager;
using UnityEngine.UIElements;

namespace Runtime.Popup
{
    public sealed class PopupView
    {
        public VisualElement Root { get; }
        public TextElement Title { get; }
        public TextElement Message { get; }
        
        public PopupView(PopupContent popupContent)
        {
            Root = popupContent.PopupRoot.Q<VisualElement>(PopupConstants.Root);
            
            Title = Root.Q<TextElement>(PopupConstants.Title);
            Message = Root.Q<TextElement>(PopupConstants.Message);
        }

        public void Show()
        {
            Root.AddToClassList(PopupConstants.Visible);
            Root.RemoveFromClassList(PopupConstants.Invisible);
        }

        public void Hide()
        {
            Root.AddToClassList(PopupConstants.Invisible);
            Root.RemoveFromClassList(PopupConstants.Visible);
        }
    }
}