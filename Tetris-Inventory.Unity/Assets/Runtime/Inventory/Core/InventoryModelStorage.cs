using Runtime.Inventory.Common;
using Runtime.Inventory.DragAndDrop;
using Runtime.Inventory.ItemGeneration;
using Runtime.Popup;

namespace Runtime.Inventory.Core
{
    public class InventoryModelStorage
    {
        public InventoryModel CoreInventoryModel { get; private set; }
        
        public InventoryModel StashInventoryModel { get; private set; }
        
        public DragDropModel DragDropModel { get; private set; }
        
        public PopupModel PopupModel { get; private set; }
        
        public ItemGenerationModel ItemGenerationModel { get; private set; }
        
        public InventoryModelStorage(InventoryModel coreInventoryModel,  InventoryModel stashInventoryModel, ItemGenerationModel itemGenerationModel)
        {
            CoreInventoryModel = coreInventoryModel;
            StashInventoryModel = stashInventoryModel;
            ItemGenerationModel = itemGenerationModel;
            
            DragDropModel = new DragDropModel();
            PopupModel = new PopupModel();
        }
    }
}