using System.Collections.Generic;
using System.Linq;
using Shared.Model;

namespace TI.Sergei_Lind.Runtime.InventorySystem
{
    public class TileMap
    {
        public int Width { get; }
        public int Height { get; }

        private readonly Tile[,] _tiles;
        
        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new Tile[width, height];
            for (var y = 0; y < _tiles.GetLength(1); y++)
            {
                for (var x = 0; x < _tiles.GetLength(0); x++)
                {
                    _tiles[x, y] = new Tile(new TilePosition(x, y));
                }
            }
        }

        public bool TryAddItem(ItemInstance instance, TilePosition anchoredPosition)
        {
            if (!CanPlaceItem(instance.Item, anchoredPosition))
                return false;
            
            ApplyPlacement(instance, anchoredPosition);
            instance.SetAnchorPosition(anchoredPosition);
            return true;
        }
        
        public void RemoveItem(ItemInstance instance)
        {
            foreach (var tile in _tiles)
            {
                if (tile.Item == instance)
                    tile.Clear();
            }

            instance.ClearAnchorPosition();
        }
        
        public Tile GetTile(int x, int y) => _tiles[x, y];

        public ItemInstance GetItem(TilePosition position)
        {
            return _tiles[position.X, position.Y].Item;
        }

        public void Clear()
        {
            foreach (var tile in _tiles)
                tile.Clear();
        }

        private void ApplyPlacement(ItemInstance instance, TilePosition anchoredPosition)
        {
            foreach (var occupiedTiles in GetOccupiedTiles(instance.Item, anchoredPosition))
            {
                occupiedTiles.SetItem(instance);
            }
        }

        private bool CanPlaceItem(Item item, TilePosition anchoredPosition)
        {
            for (var dy = 0; dy < item.Height; dy++)
            for (var dx = 0; dx < item.Width; dx++)
            {
                if (!item.Shape[dx, dy])
                    continue;

                var x = anchoredPosition.X + dx;
                var y = anchoredPosition.Y + dy;

                if (!IsInsideBounds(new TilePosition(x, y)))
                    return false;
            }

            return GetOccupiedTiles(item, anchoredPosition).All(tile => tile.IsEmpty);
        }

        private IEnumerable<Tile> GetOccupiedTiles(Item item, TilePosition anchoredPosition)
        {
            for (var dy = 0; dy < item.Height; dy++)
            for (var dx = 0; dx < item.Width; dx++)
            {
                if (!item.Shape[dx, dy])
                    continue;

                yield return _tiles[anchoredPosition.X + dx, anchoredPosition.Y + dy];
            }
        }
        
        private bool IsInsideBounds(TilePosition position)
            => position is { X: >= 0, Y: >= 0 } && position.X < Width && position.Y < Height;
    }
}