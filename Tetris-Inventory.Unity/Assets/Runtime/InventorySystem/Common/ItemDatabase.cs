#nullable enable
using System.Collections.Generic;
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

        public Item? TryGetItemById(string id)
        {
            return _itemsById.GetValueOrDefault(id);
        }

        public IReadOnlyCollection<Item> GetAllItems()
        {
            return _itemsById.Values.ToList();
        }
    }
}