using System.Collections.Generic;
using Runtime.Inventory.Common;
using System.Linq;

namespace Runtime.Inventory.ItemGeneration
{
    public sealed class ItemGenerationModel
    {
        private readonly ItemGenerationConfig _config;
        private readonly ItemConfig[] _availableConfigs;

        public ItemGenerationModel(ItemGenerationConfig config, IEnumerable<ItemConfig> allItems)
        {
            _config = config;
            _availableConfigs = config.UseAllItemsFromDatabase
                ? allItems.ToArray()
                : config.ItemConfigs.ToArray();
        }
        
        public IEnumerable<Item> GetRandomItems()
        {
            for (var i = 0; i < _config.DefaultCount; i++)
            {
                var randomConfig = _availableConfigs[UnityEngine.Random.Range(0, _availableConfigs.Length)];
                yield return ItemConfigAdapter.ToModel(randomConfig);
            }
        }
    }
}