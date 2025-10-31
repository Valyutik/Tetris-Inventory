using UnityEngine;

public interface IDragDropView
{
    void Drag(Vector2 actualPosition);

    void Drop();
}