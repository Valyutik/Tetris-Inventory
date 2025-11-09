using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.InventorySystem.ItemGeneration
{
    [CreateAssetMenu(fileName = "ItemGenerationConfig", menuName = "Items/ItemGenerationConfig")]
    public sealed class ItemGenerationConfig : ScriptableObject
    {
        [Tooltip("Configurations of items that can be generated")]
        public List<ItemConfig> itemConfigs = new();

        [Tooltip("The default number of items to generate.")]
        public int defaultCount = 3;

        [Tooltip("If true, all items from the database can be used.")]
        public bool useAllItemsFromDatabase = true;
    }
}