using System.Collections.Generic;
using Runtime.Inventory.Item;
using System.Linq;
using UnityEngine;

namespace Runtime.Inventory.Common.Data
{
    public class Grid
    {
        public int Width => _cells.GetLength(0);
        public int Height => _cells.GetLength(1);

        private readonly Cell[,] _cells;
        
        public Grid(int initialWidth, int initialHeight)
        {
            _cells = new Cell[initialWidth, initialHeight];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    _cells[x, y] = new Cell();
                }
            }
        }

        public bool TryAddItem(ItemModel itemModel, Vector2Int position)
        {
            if (CanPlaceItem(itemModel, position))
            {
                ApplyPlacement(itemModel, position);
                itemModel.AnchorPosition = position;
                return true;
            }

            return false;
        }
        
        public bool TryAddItem(ItemModel itemModel)
        {
            for (var y = 0; y <= Height - itemModel.Height; y++)
            {
                for (var x = 0; x <= Width - itemModel.Width; x++)
                {
                    var position = new Vector2Int(x, y);

                    if (CanPlaceItem(itemModel, position))
                    {
                        ApplyPlacement(itemModel, position);
                        itemModel.AnchorPosition = position;
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        public void RemoveItem(ItemModel itemModel)
        {
            foreach (var cell in _cells)
            {
                if (cell.ItemModel == itemModel)
                    cell.Clear();
            }
        }
        
        public Cell GetCell(int x, int y) => _cells[x, y];

        public ItemModel GetItem(Vector2Int position)
        {
            return IsInsideBounds(position) ? _cells[position.x, position.y].ItemModel : null;
        }

        public bool CanPlaceItem(ItemModel itemModel, Vector2Int position)
        {
            for (var dy = 0; dy < itemModel.Height; dy++)
            {
                for (var dx = 0; dx < itemModel.Width; dx++)
                {
                    if (!itemModel.Shape[dx, dy])
                        continue;

                    var x = position.x + dx;
                    var y = position.y + dy;

                    if (IsInsideBounds(new Vector2Int(x, y))) continue;
                    return false;
                }
            }

            return GetOccupiedCells(itemModel, position).All(cell => cell.IsEmpty);
        }

        public void Clear()
        {
            foreach (var cell in _cells)
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
            {
                for (var dx = 0; dx < itemModel.Width; dx++)
                {
                    if (itemModel.Shape[dx, dy])
                        yield return _cells[position.x + dx, position.y + dy];
                }
            }
        }
        
        private bool IsInsideBounds(Vector2Int position)
        {
            return position is { x: >= 0, y: >= 0 } && position.x < Width && position.y < Height;
        }
    }
}