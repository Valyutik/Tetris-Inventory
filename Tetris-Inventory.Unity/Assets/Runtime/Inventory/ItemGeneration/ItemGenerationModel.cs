using System;
using System.Collections.Generic;
using Runtime.Inventory.Common;
using System.Linq;

namespace Runtime.Inventory.ItemGeneration
{
    public sealed class ItemGenerationModel
    {
        public event Action<IReadOnlyList<Item>> OnItemGenerated;
        
        private readonly ItemGenerationConfig _config;
        private readonly ItemConfig[] _availableItems;

        public ItemGenerationModel(ItemGenerationConfig config)
        { 
            _config = config;
            
            _availableItems = config.ItemConfigs.ToArray();
        }

        public void ItemGenerated(List<Item> items)
        {
            OnItemGenerated?.Invoke(items);
        }

        public IReadOnlyList<Item> GetRandomItems()
        {
            var items = new List<Item>();
            
            for (var i = 0; i < _config.DefaultCount; i++)
            {
                var randomConfig = _availableItems[UnityEngine.Random.Range(0, _availableItems.Length)];
                
                var item = ItemConfigAdapter.ToModel(randomConfig);
                
                items.Add(item);
            }

            return items;
        }
    }
}