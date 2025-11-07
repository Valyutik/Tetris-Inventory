using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.ItemGeneration;
using Runtime.InventorySystem.ContentManager;
using Runtime.InventorySystem.DragAndDrop;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Stash;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Vector2Int _inventorySize;
        [SerializeField] private List<ItemConfig> _itemConfigs;
        
        [Header("UI Elements")]
        [SerializeField] private VisualTreeAsset _inventory;
        [SerializeField] private VisualTreeAsset _stash;
        [SerializeField] private VisualTreeAsset _createItemButton;
        [SerializeField] private VisualTreeAsset _deleteItemButton;
        
        [Header("Popup")]
        [SerializeField] private VisualTreeAsset _deleteConfirmationAsset;
        [SerializeField] private UIDocument _popupUIDocument;
        
        private StashPresenter _stashPresenter;
    
        private void Start()
        {
            var contentView = new ContentView(_document);
            
            contentView.AddElement(_stash);
            contentView.AddElement(_createItemButton);
            contentView.AddElement(_deleteItemButton);
            contentView.AddElement(_inventory);
            
            var itemDatabase = new ItemDatabase(ItemConfigLoader.LoadAll());
            
            var inventoryView = new InventoryView(_document);

            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);

            foreach (var item in _itemConfigs.Select(itemConfig => new Item(itemConfig.id, itemConfig.name,
                         itemConfig.description, itemConfig.color,
                         itemConfig.GetShapeMatrix())))
            {
                inventoryModel.TryPlaceItem(item, new Vector2Int(2, 0));
            }

            var inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel);
            
            var stashView = new StashView(_document.rootVisualElement);
            var stashModel = new StashModel();
            _stashPresenter = new StashPresenter(stashView, stashModel);
            _stashPresenter.Initialize();

            var itemGenerationModel = new ItemGenerationModel(itemDatabase.GetAllItems().ToList());
            var itemGenerationView = new ItemGenerationView(_document.rootVisualElement);
            var itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView, itemGenerationModel);
            itemGenerationPresenter.OnItemGenerated += _stashPresenter.ShowItem;
            
            var deleteArea = new DeleteAreaView(_document.rootVisualElement.Q<Button>("DeleteButton"));

            var deleteConfirmation = new DeleteConfirmationView(_popupUIDocument, _deleteConfirmationAsset);
            
            var gameLoop = new DragDropHandler(inventoryPresenter, _stashPresenter, deleteArea, deleteConfirmation);

            gameLoop.Init(_document.rootVisualElement);
        }
    }
}