using System;
using System.Collections.Generic;
using Runtime.Inventory.Item;
using Runtime.Inventory.Item.Extensions;
using Random = UnityEngine.Random;

namespace Runtime.Inventory.ItemGeneration
{
    public sealed class ItemGenerationModel
    {
        public event Action<IReadOnlyList<ItemModel>> OnItemGenerated;
        
        private readonly ItemGenerationConfig _config;
        
        public ItemGenerationModel(ItemGenerationConfig config)
        { 
            _config = config;
        }

        public void GenerateItem(List<ItemModel> items)
        {
            OnItemGenerated?.Invoke(items);
        }

        public IReadOnlyList<ItemModel> GetRandomItems()
        {
            var items = new List<ItemModel>();
            
            for (var i = 0; i < _config.DefaultCount; i++)
            {
                var randomItemConfig = _config.ItemConfigs[Random.Range(0, _config.ItemConfigs.Count)];
                
                var item = randomItemConfig.ToModel();
                
                items.Add(item);
            }

            return items;
        }
    }
}