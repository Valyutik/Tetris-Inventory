using Shared.Model;

namespace TI.Sergei_Lind.Runtime.InventorySystem
{
    public sealed class Tile
    {
        public TilePosition Position { get; private set; }
        public ItemInstance Item { get; private set; }
        public bool IsEmpty => Item == null;
        
        public Tile(TilePosition position)
        {
            Position = position;
        }

        public void SetItem(ItemInstance item)
        {
            Item = item;
        }
        
        public void Clear()
        {
            Item = null;
        }
    }
}