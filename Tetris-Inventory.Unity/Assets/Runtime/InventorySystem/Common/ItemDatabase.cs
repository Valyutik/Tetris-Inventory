#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Utilities;
using System.Linq;

namespace Runtime.InventorySystem.Common
{
    public sealed class ItemDatabase
    {
        private readonly Dictionary<string, Item> _itemsById;
    
        public ItemDatabase(IEnumerable<ItemConfig> configs)
        {
            _itemsById = new Dictionary<string, Item>();
            foreach (var config in configs)
            {
                var item = ItemConfigAdapter.ToModel(config);
                _itemsById[item.Id] = item;
            }
        }
        
        public static async Task<ItemDatabase> CreateAsync(string label)
        {
            var configs = await AddressablesLoader.LoadAllAsync<ItemConfig>(label);

            return new ItemDatabase(configs);
        }

        public Item? TryGetItemById(string id)
        {
            return _itemsById.GetValueOrDefault(id);
        }

        public IReadOnlyCollection<Item> GetAllItems()
        {
            return _itemsById.Values.ToList();
        }
        
        public Item? CreateItemInstance(string id)
        {
            var template = TryGetItemById(id);
            if (template != null)
            {
                return new Item(
                    id: template.Id,
                    name: template.Name,
                    description: template.Description,
                    color: template.Color,
                    shape: template.Shape
                );
            }

            return null;
        }
    }
}