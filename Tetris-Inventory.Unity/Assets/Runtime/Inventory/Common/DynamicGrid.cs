using System;
using UnityEngine;

namespace Runtime.Inventory.Common
{
    public sealed class DynamicGrid : Grid
    {
        private readonly int _maxWidth;
        private readonly int _maxHeight;
        private readonly int _initialWidth;
        private readonly int _initialHeight;

        public DynamicGrid(int maxWidth, int maxHeight, int initialWidth = 0, int initialHeight = 0) : base(initialWidth,
            initialHeight)
        {
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
            _initialWidth = initialWidth;
            _initialHeight = initialHeight;
        }

        public override bool TryAddItem(Item.Item item)
        {
            if (base.TryAddItem(item)) return true;
            ExpandToFit(item);
            return base.TryAddItem(item);
        }

        public override void Clear()
        {
            base.Clear();
            Cells = new Cell[_initialWidth, _initialHeight];
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                Cells[x, y] = new Cell(new Vector2Int(x, y));
        }

        private void ExpandToFit(Item.Item item)
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