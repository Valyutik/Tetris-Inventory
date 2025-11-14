using System.Collections.Generic;
using Runtime.Inventory.Common;
using UnityEngine;

namespace Runtime.Inventory.ItemGeneration
{
    [CreateAssetMenu(fileName = "ItemGenerationConfig", menuName = "ItemGeneration/ItemGenerationConfig")]
    public sealed class ItemGenerationConfig : ScriptableObject
    {
        [Tooltip("Configurations of items that can be generated")]
        public List<ItemConfig> ItemConfigs = new();

        [Tooltip("The default number of items to generate.")]
        public int DefaultCount = 3;
        
    }
}