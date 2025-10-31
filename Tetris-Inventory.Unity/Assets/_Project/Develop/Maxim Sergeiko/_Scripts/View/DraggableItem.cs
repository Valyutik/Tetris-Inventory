using UnityEngine;
using UnityEngine.UIElements;

public class DraggableItem : IImageElement
{
    public bool Active
    {
        get => _root.style.display == DisplayStyle.Flex;
        
        private set => _root.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private readonly VisualElement _root;

    public DraggableItem(VisualElement root) => _root = root;

    public void SetImage(Sprite sprite, Vector2 size)
    {
        //TODO: Add setting image
    }
    
    public void Show() => Active = true;

    public void SetPosition(Vector2 position)
    {
        _root.style.left = position.x - _root.resolvedStyle.width / 2f;
        
        _root.style.bottom = position.y - _root.resolvedStyle.height / 2f;
    }

    public void Hide() => Active = false;
}