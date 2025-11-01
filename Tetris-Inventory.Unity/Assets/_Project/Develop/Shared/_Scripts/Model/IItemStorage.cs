using Shared.Model;

namespace _Project.Model
{
    public interface IItemStorage
    {
        bool CanPlaceItem(TilePosition target, TilePosition anchorItemPosition, bool[,] flags);
        
        void PlaceItem(TilePosition target, TilePosition anchorItemPosition, bool[,] flags);
    }
}