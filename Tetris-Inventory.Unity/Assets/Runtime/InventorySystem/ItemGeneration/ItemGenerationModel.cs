using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationModel
    {
        private readonly ItemGenerationConfig _config;
        private readonly ItemConfig[] _availableConfigs;

        public ItemGenerationModel(ItemGenerationConfig config, IEnumerable<ItemConfig> allItems)
        {
            _config = config;
            _availableConfigs = config.useAllItemsFromDatabase
                ? allItems.ToArray()
                : config.itemConfigs.ToArray();
        }
        
        public IEnumerable<Item> GetRandomItems()
        {
            for (var i = 0; i < _config.defaultCount; i++)
            {
                var randomConfig = _availableConfigs[UnityEngine.Random.Range(0, _availableConfigs.Length)];
                yield return ItemConfigAdapter.ToModel(randomConfig);
            }
        }
    }
}