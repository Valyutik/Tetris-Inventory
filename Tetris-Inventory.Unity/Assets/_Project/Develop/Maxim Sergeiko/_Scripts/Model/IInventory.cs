namespace _Project.Model
{
    public interface IInventory
    {
        bool CanPlaceItem(TilePosition target, Item item);
        void PlaceItem(TilePosition target, Item item);
        void RemoveItem(TilePosition targetPosition);
        void RemoveItem(Item item);
    }
}