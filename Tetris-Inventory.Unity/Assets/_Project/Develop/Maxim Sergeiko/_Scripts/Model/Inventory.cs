using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace _Project.Model
{
    public class Inventory : Tilemap, IInventory
    {
        private readonly Item[,] _itemMap;
        
        private readonly List<Item> _placedItems = new List<Item>();
        
        public Inventory(int width, int height) : base(width, height)
        { 
            _itemMap = new Item[width, height];
        }

        public bool CanPlaceItem(TilePosition target, Item item)
        {
            for (int x = 0; x < item.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < item.Tiles.GetLength(1); y++)
                {
                    if (item.Tiles[x, y] == Tile.Empty)
                        continue; 

                    int invX = target.X + x;
                    int invY = target.Y + y;

                    if (invX < 0 || invX >= Tiles.GetLength(0) || invY < 0 || invY >= Tiles.GetLength(1))
                        return false;

                    if (Tiles[invX, invY] != Tile.Empty)
                        return false;
                }
            }
            return true;
        }

        public void PlaceItem(TilePosition target, Item item)
        {
            if (!CanPlaceItem(target, item))
                throw new InvalidOperationException("Cannot place item at the target position.");

            for (int x = 0; x < item.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < item.Tiles.GetLength(1); y++)
                {
                    if (item.Tiles[x, y] == Tile.Empty)
                        continue;

                    int invX = target.X + x;
                    int invY = target.Y + y;

                    Tiles[invX, invY] = Tile.Exists;
                    _itemMap[invX, invY] = item;
                }
            }

            if (!_placedItems.Contains(item))
                _placedItems.Add(item);
        }

        public void RemoveItem(TilePosition targetPosition) => RemoveItem(GetItemAt(targetPosition));

        public void RemoveItem(Item item)
        {
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    if (_itemMap[x, y] == item)
                    {
                        Tiles[x, y] = Tile.Empty;
                        _itemMap[x, y] = null;
                    }
                }
            }
            _placedItems.Remove(item);
        }

        public void LogMatrixState()
        {
            int width = Tiles.GetLength(0);
            int height = Tiles.GetLength(1);
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < height; y++) // цикл снизу вверх
            {
                for (int x = 0; x < width; x++)
                {
                    sb.Append(Tiles[x, y] == Tile.Empty ? "X" : "#");
                }
                sb.AppendLine();
            }

            Debug.Log(sb.ToString());
        }

        private Item GetItemAt(TilePosition pos)
        {
            if (pos.X < 0 || pos.X >= Tiles.GetLength(0) || pos.Y < 0 || pos.Y >= Tiles.GetLength(1))
                return null;
            return _itemMap[pos.X, pos.Y];
        }
    }
}