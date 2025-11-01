using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TI.Sergei_Lind.Runtime.InventorySystem
{
    public class Grid
    {
        public int Width { get; }
        public int Height { get; }

        private readonly Cell[,] _cells;
        
        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new Cell[width, height];
            for (var y = 0; y < _cells.GetLength(1); y++)
            {
                for (var x = 0; x < _cells.GetLength(0); x++)
                {
                    _cells[x, y] = new Cell(new Vector2Int(x, y));
                }
            }
        }

        public bool TryAddItem(Item instance, Vector2Int anchoredPosition)
        {
            if (!CanPlaceItem(instance, anchoredPosition))
                return false;
            
            ApplyPlacement(instance, anchoredPosition);
            instance.SetAnchorPosition(anchoredPosition);
            return true;
        }
        
        public void RemoveItem(Item instance)
        {
            foreach (var tile in _cells)
            {
                if (tile.Item == instance)
                    tile.Clear();
            }

            instance.ClearAnchorPosition();
        }
        
        public Cell GetCell(int x, int y) => _cells[x, y];

        public Item GetItem(Vector2Int position)
        {
            return IsInsideBounds(position) ? _cells[position.y, position.x].Item : null;
        }

        public void Clear()
        {
            foreach (var tile in _cells)
                tile.Clear();
        }

        private void ApplyPlacement(Item instance, Vector2Int anchoredPosition)
        {
            foreach (var occupiedTiles in GetOccupiedCells(instance, anchoredPosition))
            {
                occupiedTiles.SetItem(instance);
            }
        }

        private bool CanPlaceItem(Item item, Vector2Int anchoredPosition)
        {
            for (var dy = 0; dy < item.Height; dy++)
            for (var dx = 0; dx < item.Width; dx++)
            {
                if (!item.Shape[dy, dx])
                    continue;

                var x = anchoredPosition.x + dx;
                var y = anchoredPosition.y + dy;

                if (!IsInsideBounds(new Vector2Int(x, y)))
                    return false;
            }

            return GetOccupiedCells(item, anchoredPosition).All(tile => tile.IsEmpty);
        }

        private IEnumerable<Cell> GetOccupiedCells(Item item, Vector2Int anchoredPosition)
        {
            for (var dy = 0; dy < item.Height; dy++)
            for (var dx = 0; dx < item.Width; dx++)
            {
                if (!item.Shape[dy, dx])
                    continue;

                yield return _cells[anchoredPosition.x + dx, anchoredPosition.y + dy];
            }
        }
        
        private bool IsInsideBounds(Vector2Int position)
            => position is { x: >= 0, y: >= 0 } && position.x < Width && position.y < Height;
    }
}