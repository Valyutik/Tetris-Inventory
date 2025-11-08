using UnityEngine;
using System;

namespace Runtime.InventorySystem.Common
{
    public sealed class DynamicGrid : Grid
    {
        private readonly int _maxWidth;
        private readonly int _maxHeight;
        
        public DynamicGrid(int maxWidth, int maxHeight, int width = 0, int height = 0) : base(width,
            height)
        {
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
        }

        public override bool TryAddItem(Item item)
        {
            if (base.TryAddItem(item)) return true;
            ExpandToFit(item);
            return base.TryAddItem(item);
        }
        
        private void ExpandToFit(Item item)
        {
            var newWidth = Width;
            var newHeight = Height;

            if (Width < _maxWidth)
            {
                newWidth = Math.Min(_maxWidth, Width + item.Width);
                newHeight = Math.Max(Height, item.Height);
            }
            else if (Height < _maxHeight)
            {
                newWidth = Width;
                newHeight = Math.Min(_maxHeight, Height + item.Height);
            }

            if (newWidth == Width && newHeight == Height)
                return;

            var newCells = new Cell[newWidth, newHeight];
            for (var y = 0; y < newHeight; y++)
            for (var x = 0; x < newWidth; x++)
            {
                if (x < Width && y < Height)
                    newCells[x, y] = Cells[x, y];
                else
                    newCells[x, y] = new Cell(new Vector2Int(x, y));
            }

            Cells = newCells;
        }
    }
}