using Shared.Model;

namespace TI.Sergei_Lind.Runtime.InventorySystem
{
    public class ItemInstance
    {
        public Item Item { get; }
        public string InstanceId { get; }
        public TilePosition? Position { get; private set; }
        
        public ItemInstance(Item item, string instanceId)
        {
            Item = item;
            InstanceId = instanceId;
        }

        public void SetAnchorPosition(TilePosition position)
        {
            Position = position;
        }

        public void ClearAnchorPosition()
        {
            Position = null;
        }
    }
}