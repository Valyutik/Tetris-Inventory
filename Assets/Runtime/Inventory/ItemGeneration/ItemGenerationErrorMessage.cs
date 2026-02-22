using UnityEngine;

namespace Runtime.Inventory.ItemGeneration
{
    [CreateAssetMenu(fileName = "ItemGenerationErrorMessage", menuName = "ItemGeneration/ItemGenerationErrorMessage")]
    public sealed class ItemGenerationErrorMessage : ScriptableObject   
    {
        [Header("Stash is not empty")]
        public string StashNotEmptyTitle;
        [TextArea] public string StashNotEmptyMessage;
        
        [Space]
        [Header("No space for items")]
        public string InventoryHasNoSpaceTitle;
        [TextArea] public string InventoryHasNoSpaceMessage;
    }
}