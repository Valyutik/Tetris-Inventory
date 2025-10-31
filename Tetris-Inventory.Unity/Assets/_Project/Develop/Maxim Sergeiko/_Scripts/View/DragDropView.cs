using UnityEngine;
using UnityEngine.UIElements;

public class DragDropView : IDragDropView
{
    private VisualElement _layout;
    
    private readonly DraggableItem  _draggableItem;
    
    public DragDropView(VisualElement layout, VisualElement draggable)
    {
        _layout = layout;
        
        _layout.Add(draggable);
        
        _draggableItem = new DraggableItem(draggable);

        _draggableItem.Hide();
    }

    public void Drag(Vector2 actualPosition)
    {
        if (!_draggableItem.Active) _draggableItem.Show();
        
        _draggableItem.SetPosition(actualPosition);
    }

    public void Drop() => _draggableItem.Hide();
}