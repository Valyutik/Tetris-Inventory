using System.Collections.Generic;
using System.Linq;
using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UIDocument _mainUIDocument;
        [SerializeField] private Vector2Int _inventorySize;
        [SerializeField] private List<ItemConfig> _itemConfigs;
        
        [Header("Popup")]
        [SerializeField] private VisualTreeAsset _deleteConfirmationAsset;
        [SerializeField] private UIDocument _popupUIDocument;

        private readonly List<Item> _itemsList = new();

        private void Start()
        {
            var inventoryView = new InventoryView(_mainUIDocument);

            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);
            
            foreach (var item in _itemConfigs.Select(itemConfig => new Item(itemConfig.id, itemConfig.name, 
                         itemConfig.description, itemConfig.color,
                         itemConfig.GetShapeMatrix())))
            {
                _itemsList.Add(item);
            }
            
            //TODO: Testing place item in grid
            inventoryModel.TryPlaceItem(_itemsList[0], new Vector2Int(0, 0));
            inventoryModel.TryPlaceItem(_itemsList[1], new Vector2Int(2, 0));
            
            var inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel);

            var deleteArea = new DeleteAreaView(_mainUIDocument.rootVisualElement.Q<Button>("DeleteButton"));

            var deleteConfirmation = new DeleteConfirmationView(_popupUIDocument, _deleteConfirmationAsset);
            
            var gameLoop = new GameLoop(inventoryPresenter, deleteArea, deleteConfirmation);
            
            gameLoop.Run();
        }
    }
}