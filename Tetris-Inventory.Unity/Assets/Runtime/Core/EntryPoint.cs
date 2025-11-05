using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        
        [SerializeField] private UIDocument _document;
        
        [SerializeField] private Vector2Int _inventorySize;
        
        private void Start()
        {
            var inventoryView = new InventoryView(_document);

            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);
            
            var item = new Item("0", "Test", "Description", new bool[,] 
            {   { true, true }, 
                { true, false } 
            });
            
            var item2 = new Item("0", "Test", "Description", new bool[,] 
            {   { true, true, true }, 
                { false, false, false } 
            });
            
            inventoryModel.TryPlaceItem(item, new Vector2Int(2, 0));
            inventoryModel.TryPlaceItem(item2, new Vector2Int(0, 0));
            
            var inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel);

            var deleteArea = new DeleteAreaView(_document.rootVisualElement.Q<Button>("DeleteButton"));
            
            var gameLoop = new GameLoop(inventoryPresenter, deleteArea);
            
            gameLoop.Run();
        }
    }
}