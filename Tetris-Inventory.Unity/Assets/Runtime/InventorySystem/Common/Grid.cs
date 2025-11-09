using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.InventorySystem.Common
{
    public class Grid
    {
        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);
        
        protected Cell[,] Cells;
        
        public Grid(int initialWidth, int initialHeight)
        {
            Cells = new Cell[initialWidth, initialHeight];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Cells[x, y] = new Cell(new Vector2Int(x, y));
                }
            }
        }

        public bool TryAddItem(Item item, Vector2Int position)
        {
            if (!CanPlaceItem(item, position))
                return false;
            
            ApplyPlacement(item, position);
            item.AnchorPosition = position;
            return true;
        }
        
        public virtual bool TryAddItem(Item item)
        {
            for (var y = 0; y <= Height - item.Height; y++)
            for (var x = 0; x <= Width - item.Width; x++)
            {
                var position = new Vector2Int(x, y);
                
                if (!CanPlaceItem(item, position)) continue;
                ApplyPlacement(item, position);
                item.AnchorPosition = position;
                return true;
            }
            return false;
        }
        
        public void RemoveItem(Item item)
        {
            foreach (var cell in Cells)
            {
                if (cell.Item == item)
                    cell.Clear();
            }
        }
        
        public Cell GetCell(int x, int y) => Cells[x, y];

        public Item GetItem(Vector2Int position)
        {
            return IsInsideBounds(position) ? Cells[position.x, position.y].Item : null;
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

            return GetOccupiedCells(item, position).All(cell => cell.IsEmpty);
        }

        public virtual void Clear()
        {
            foreach (var cell in Cells)
                cell.Clear();
        }

        private void ApplyPlacement(Item item, Vector2Int position)
        {
            foreach (var occupiedTiles in GetOccupiedCells(item, position))
            {
                occupiedTiles.SetItem(item);
            }
        }

        private IEnumerable<Cell> GetOccupiedCells(Item item, Vector2Int position)
        {
            for (var dy = 0; dy < item.Height; dy++)
            for (var dx = 0; dx < item.Width; dx++)
            {
                if (!item.Shape[dx, dy])
                    continue;

                yield return Cells[position.x + dx, position.y + dy];
            }
        }
        
        private bool IsInsideBounds(Vector2Int position)
            => position is { x: >= 0, y: >= 0 } && position.x < Width && position.y < Height;
    }
}