using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Inventory.Common
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

        public bool TryAddItem(ItemModel itemModel, Vector2Int position)
        {
            if (!CanPlaceItem(itemModel, position))
                return false;
            
            ApplyPlacement(itemModel, position);
            itemModel.AnchorPosition = position;
            return true;
        }
        
        public virtual bool TryAddItem(ItemModel itemModel)
        {
            for (var y = 0; y <= Height - itemModel.Height; y++)
            for (var x = 0; x <= Width - itemModel.Width; x++)
            {
                var position = new Vector2Int(x, y);
                
                if (!CanPlaceItem(itemModel, position)) continue;
                ApplyPlacement(itemModel, position);
                itemModel.AnchorPosition = position;
                return true;
            }
            return false;
        }
        
        public void RemoveItem(ItemModel itemModel)
        {
            foreach (var cell in Cells)
            {
                if (cell.ItemModel == itemModel)
                    cell.Clear();
            }
        }
        
        public Cell GetCell(int x, int y) => Cells[x, y];

        public ItemModel GetItem(Vector2Int position)
        {
            return IsInsideBounds(position) ? Cells[position.x, position.y].ItemModel : null;
        }

        public bool CanPlaceItem(ItemModel itemModel, Vector2Int position)
        {
            for (var dy = 0; dy < itemModel.Height; dy++)
            for (var dx = 0; dx < itemModel.Width; dx++)
            {
                if (!itemModel.Shape[dx, dy])
                    continue;

                var x = position.x + dx;
                var y = position.y + dy;

                if (!IsInsideBounds(new Vector2Int(x, y)))
                    return false;
            }

            return GetOccupiedCells(itemModel, position).All(cell => cell.IsEmpty);
        }

        public virtual void Clear()
        {
            foreach (var cell in Cells)
                cell.Clear();
        }

        private void ApplyPlacement(ItemModel itemModel, Vector2Int position)
        {
            foreach (var occupiedTiles in GetOccupiedCells(itemModel, position))
            {
                occupiedTiles.SetItem(itemModel);
            }
        }

        private IEnumerable<Cell> GetOccupiedCells(ItemModel itemModel, Vector2Int position)
        {
            for (var dy = 0; dy < itemModel.Height; dy++)
            for (var dx = 0; dx < itemModel.Width; dx++)
            {
                if (!itemModel.Shape[dx, dy])
                    continue;

                yield return Cells[position.x + dx, position.y + dy];
            }
        }
        
        private bool IsInsideBounds(Vector2Int position)
            => position is { x: >= 0, y: >= 0 } && position.x < Width && position.y < Height;
    }
}