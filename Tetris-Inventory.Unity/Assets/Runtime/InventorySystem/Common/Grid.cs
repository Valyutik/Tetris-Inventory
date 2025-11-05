using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.InventorySystem.Common
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

        public bool TryAddItem(Item item, Vector2Int position)
        {
            if (!CanPlaceItem(item, position))
                return false;
            
            ApplyPlacement(item, position);
            item.SetAnchorPosition(position);
            return true;
        }
        
        public void RemoveItem(Item item)
        {
            foreach (var cell in _cells)
            {
                if (cell.Item == item)
                    cell.Clear();
            }

            item.ClearAnchorPosition();
        }
        
        public Cell GetCell(int x, int y) => _cells[x, y];

        public Item GetItem(Vector2Int position)
        {
            return IsInsideBounds(position) ? _cells[position.x, position.y].Item : null;
        }

        public void Clear()
        {
            foreach (var cell in _cells)
                cell.Clear();
        }

        private void ApplyPlacement(Item item, Vector2Int position)
        {
            foreach (var occupiedTiles in GetOccupiedCells(item, position))
            {
                occupiedTiles.SetItem(item);
            }
        }

        public bool CanPlaceItem(Item item, Vector2Int position)
        {
            for (var dy = 0; dy < item.Height; dy++)
            for (var dx = 0; dx < item.Width; dx++)
            {
                if (!item.Shape[dx, dy])
                    continue;

                var x = position.x + dx;
                var y = position.y + dy;

                if (!IsInsideBounds(new Vector2Int(x, y)))
                    return false;
            }

            return GetOccupiedCells(item, position).All(tile => tile.IsEmpty);
        }

        private IEnumerable<Cell> GetOccupiedCells(Item item, Vector2Int position)
        {
            for (var dy = 0; dy < item.Height; dy++)
            for (var dx = 0; dx < item.Width; dx++)
            {
                if (!item.Shape[dx, dy])
                    continue;

                yield return _cells[position.x + dx, position.y + dy];
            }
        }
        
        private bool IsInsideBounds(Vector2Int position)
            => position is { x: >= 0, y: >= 0 } && position.x < Width && position.y < Height;
    }
}