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
        private readonly ItemConfig[] _availableConfigs;

        public void ItemGenerated(List<Item> items)
        {
            OnItemGenerated?.Invoke(items);
        }
        
        public ItemGenerationModel(ItemGenerationConfig config, IEnumerable<ItemConfig> allItems)
        {
            _config = config;
            _availableConfigs = config.UseAllItemsFromDatabase
                ? allItems.ToArray()
                : config.ItemConfigs.ToArray();
        }
        
        public IReadOnlyList<Item> GetRandomItems()
        {
            var items = new List<Item>();
            
            for (var i = 0; i < _config.DefaultCount; i++)
            {
                var randomConfig = _availableConfigs[UnityEngine.Random.Range(0, _availableConfigs.Length)];
                
                var item = ItemConfigAdapter.ToModel(randomConfig);
                
                items.Add(item);
            }

            return items;
        }
    }
}