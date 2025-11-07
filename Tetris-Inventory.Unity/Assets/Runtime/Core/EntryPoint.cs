using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.ItemGeneration;
using Runtime.InventorySystem.ContentManager;
using Runtime.InventorySystem.DragAndDrop;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Stash;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Vector2Int _inventorySize;
        
        [Header("UI Elements")]
        [SerializeField] private VisualTreeAsset _inventory;
        [SerializeField] private VisualTreeAsset _stash;
        [SerializeField] private VisualTreeAsset _createItemButton;
        [SerializeField] private VisualTreeAsset _deleteItemButton;
        
        [Header("Popup")]
        [SerializeField] private VisualTreeAsset _deleteConfirmationAsset;
        [SerializeField] private UIDocument _popupUIDocument;
        
        private InventoryPresenter _inventoryPresenter;
        private StashPresenter _stashPresenter;
        private DragDropHandler _dragDropHandler;
        private ItemGenerationPresenter _itemGenerationPresenter;
    
        private void Start()
        {
            InitializeUI();
            InitializeInventory();
            InitializeStash();
            InitializeItemGeneration();
            InitializeDeleteSystem();
            InitializeDragAndDrop();
        }
        
        private void InitializeUI()
        {
            var contentView = new ContentView(_document);
            contentView.AddElement(_stash);
            contentView.AddElement(_createItemButton);
            contentView.AddElement(_deleteItemButton);
            contentView.AddElement(_inventory);
        }

        private void InitializeInventory()
        {
            var inventoryView = new InventoryView(_document.rootVisualElement.Q<VisualElement>("InventoryGrid"));
            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);
            _inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel);
        }

        private void InitializeStash()
        {
            var stashView = new InventoryView(_document.rootVisualElement.Q<VisualElement>("StashGrid"));
            var stashModel = new StashModel();
            _stashPresenter = new StashPresenter(stashView, stashModel);
        }

        private void InitializeItemGeneration()
        {
            var itemDatabase = new ItemDatabase(ItemConfigLoader.LoadAll());
            var itemGenerationModel = new ItemGenerationModel(itemDatabase.GetAllItems().ToList());
            var itemGenerationView = new ItemGenerationView(_document.rootVisualElement);

            _itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView, itemGenerationModel);
            _itemGenerationPresenter.OnItemGenerated += _stashPresenter.SetItem;
        }

        private void InitializeDeleteSystem()
        {
            var deleteArea = new DeleteAreaView(_document.rootVisualElement.Q<Button>("DeleteButton"));
            var deleteConfirmation = new DeleteConfirmationView(_popupUIDocument, _deleteConfirmationAsset);

            _dragDropHandler = new DragDropHandler(_inventoryPresenter, _stashPresenter, deleteArea, deleteConfirmation);
        }

        private void InitializeDragAndDrop()
        {
            _dragDropHandler.Init(_document.rootVisualElement);
        }
    }
}