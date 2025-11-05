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
            
            var inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel);

            var gameLoop = new GameLoop(inventoryPresenter);
            
            gameLoop.Run();
        }
    }
}