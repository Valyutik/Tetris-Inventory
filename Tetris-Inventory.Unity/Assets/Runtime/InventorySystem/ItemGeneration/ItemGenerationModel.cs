using System.Collections.Generic;
using Runtime.InventorySystem.Common;
using System.Linq;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationModel
    {
        private readonly ItemDatabase _database;
        private readonly string[] _itemIds;

        public ItemGenerationModel(ItemDatabase database)
        {
            _database = database;
            _itemIds = _database.GetAllItems().Select(i => i.Id).ToArray();
        }
        
        public IEnumerable<Item> GetRandomItem(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var randomId = _itemIds[UnityEngine.Random.Range(0, _itemIds.Length)];
                yield return _database.CreateItemInstance(randomId);
            }
        }
    }
}