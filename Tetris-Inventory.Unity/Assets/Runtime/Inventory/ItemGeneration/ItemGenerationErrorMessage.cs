using UnityEngine;

namespace Runtime.Inventory.ItemGeneration
{
    [CreateAssetMenu(fileName = "ItemGenerationErrorMessage", menuName = "ItemGeneration/ItemGenerationErrorMessage")]
    public sealed class ItemGenerationErrorMessage : ScriptableObject   
    {
        [Header("Stash is not empty")]
        public string stashNotEmptyTitle;
        [TextArea] public string stashNotEmptyMessage;
        
        [Space]
        [Header("No space for items")]
        public string inventoryHasNoSpaceTitle;
        [TextArea] public string inventoryHasNoSpaceMessage;
    }
}