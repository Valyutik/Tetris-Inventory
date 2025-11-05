using System.Collections.Generic;
using Runtime.InventorySystem.Common;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationModel
    {
        private readonly List<Item> _items;

        public ItemGenerationModel(List<Item> items)
        {
            _items = items;
        }
        
        public Item GetRandomItem()
        {
            return _items[UnityEngine.Random.Range(0, _items.Count)];
        }
    }
}