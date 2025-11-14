using System;
using System.Collections.Generic;
using Runtime.Inventory.Common;
using System.Linq;
using Runtime.Inventory.Extensions;

namespace Runtime.Inventory.ItemGeneration
{
    public sealed class ItemGenerationModel
    {
        public event Action<IReadOnlyList<ItemModel>> OnItemGenerated;
        
        private readonly ItemGenerationConfig _config;
        
        private readonly ItemConfig[] _availableItems;

        public ItemGenerationModel(ItemGenerationConfig config)
        { 
            _config = config;
            
            _availableItems = config.ItemConfigs.ToArray();
        }

        public void ItemGenerated(List<ItemModel> items)
        {
            OnItemGenerated?.Invoke(items);
        }

        public IReadOnlyList<ItemModel> GetRandomItems()
        {
            var items = new List<ItemModel>();
            
            for (var i = 0; i < _config.DefaultCount; i++)
            {
                var randomItemConfig = _availableItems[UnityEngine.Random.Range(0, _availableItems.Length)];
                
                var item = randomItemConfig.ToModel();
                
                items.Add(item);
            }

            return items;
        }
    }
}