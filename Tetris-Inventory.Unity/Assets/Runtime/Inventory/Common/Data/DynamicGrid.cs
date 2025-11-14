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

        public DynamicGrid(int maxWidth, int maxHeight, int initialWidth = 1, int initialHeight = 1) 
            : base(initialWidth, initialHeight)
        {
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
            _initialWidth = 0;
            _initialHeight = 0;
        }

        public override bool TryAddItem(ItemModel itemModel)
        {
            if (base.TryAddItem(itemModel))
            {
                return true;
            }
            
            ExpandToFit(itemModel);
            
            return base.TryAddItem(itemModel);
        }

        public override void Clear()
        {
            base.Clear();
            
            Cells = new Cell[_initialWidth, _initialHeight];
            
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Cells[x, y] = new Cell(new Vector2Int(x, y));
                }
            }
        }

        private void ExpandToFit(ItemModel itemModel)
        {
            var newWidth = Width;
            var newHeight = Height;

            if (Width < _maxWidth)
            {
                newWidth = Math.Min(_maxWidth, Width + Math.Max(1, itemModel.Width));
            }
    
            if (Height < _maxHeight)
            {
                newHeight = Math.Min(_maxHeight, Height + Math.Max(1, itemModel.Height));
            }

            if (newWidth == Width && newHeight == Height)
            {
                return;
            }

            var newCells = new Cell[newWidth, newHeight];
    
            for (var y = 0; y < newHeight; y++)
            {
                for (var x = 0; x < newWidth; x++)
                {
                    if (x < Width && y < Height)
                    {
                        newCells[x, y] = Cells[x, y];
                    }
                    else
                    {
                        newCells[x, y] = new Cell(new Vector2Int(x, y));
                    }
                }
            }
            
            Cells = newCells;
        }
    }
}