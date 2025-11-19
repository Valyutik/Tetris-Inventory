using Runtime.Inventory.Common;
using Runtime.Inventory.DragAndDrop;
using Runtime.Inventory.ItemGeneration;
using Runtime.Popup;

namespace Runtime.Core
{
    public class ModelStorage
    {
        public InventoryStorageModel InventoryStorageModel { get; set; }        
        public InventoryModel CoreInventoryModel { get; private set; }
        
        public InventoryModel StashInventoryModel { get; private set; }
        
        public DragDropModel DragDropModel { get; private set; }
        
        public PopupModel PopupModel { get; private set; }
        
        public ItemGenerationModel ItemGenerationModel { get; private set; }
        
        public ModelStorage(InventoryModel coreInventoryModel,  InventoryModel stashInventoryModel, ItemGenerationModel itemGenerationModel)
        {
            CoreInventoryModel = coreInventoryModel;
            StashInventoryModel = stashInventoryModel;
            ItemGenerationModel = itemGenerationModel;
            
            DragDropModel = new DragDropModel();
            PopupModel = new PopupModel();
            InventoryStorageModel = new InventoryStorageModel();
        }
    }
}