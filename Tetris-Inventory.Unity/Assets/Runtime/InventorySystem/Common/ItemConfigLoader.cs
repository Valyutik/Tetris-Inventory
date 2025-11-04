using UnityEngine;

namespace Runtime.InventorySystem.Common
{
    public static class ItemConfigLoader
    {
        public static ItemConfig[] LoadAll()
        {
            return Resources.LoadAll<ItemConfig>(InventoryConstants.Item.configPath);
        }
    }
}