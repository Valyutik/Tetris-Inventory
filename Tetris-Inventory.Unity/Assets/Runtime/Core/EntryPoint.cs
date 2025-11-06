using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private List<ItemConfig> _itemConfigs;

        private void Start()
        {
            var inventoryView = new InventoryView(_document);

            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);

            //TODO: Remove the attempt to place an item in the inventory from here
            foreach (var item in _itemConfigs.Select(itemConfig => new Item(itemConfig.id, itemConfig.name,
                         itemConfig.description, itemConfig.color,
                         itemConfig.GetShapeMatrix())))
            {
                inventoryModel.TryPlaceItem(item, new Vector2Int(2, 0));
            }

            var inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel);

            var deleteArea = new DeleteAreaView(_document.rootVisualElement.Q<Button>("DeleteButton"));
            
            var gameLoop = new GameLoop(inventoryPresenter, deleteArea);
            
            gameLoop.Run();
        }
    }
}