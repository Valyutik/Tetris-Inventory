using UnityEngine;

namespace Runtime.InventorySystem.Config
{
    public static class ItemConfigLoader
    {
        public static ItemConfig[] LoadAll()
        {
            return Resources.LoadAll<ItemConfig>(RuntimeConstants.Item.configPath);
        }
    }
}