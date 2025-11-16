using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Common
{
    public sealed class CellView
    {
        public VisualElement Element { get; }
        public Vector2Int Position { get; }

        public CellView(VisualElement element, Vector2Int position)
        {
            Element = element;
            Position = position;
        }
    }
}