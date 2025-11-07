using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.ItemGeneration;
using Runtime.InventorySystem.ContentManager;
using Runtime.InventorySystem.ItemRotation;
using Runtime.InventorySystem.DragAndDrop;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Stash;
using UnityEngine.UIElements;
using Runtime.InventorySystem;
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
        
        [Header("Item Generation Settings")]
        [SerializeField] private int _numberOfItemsGenerated = 3;

        private PlayerControls _playerControls;
        
        private InventoryPresenter _inventoryPresenter;
        private StashPresenter _stashPresenter;
        private DragDropPresenter _dragDropPresenter;
        private ItemGenerationPresenter _itemGenerationPresenter;
        private ItemRotationHandler _itemRotationHandler;
    
        private void Start()
        {
            InitializeUI();
            InitializeInput();
            InitializeInventory();
            InitializeStash();
            InitializeItemGeneration();
            InitializeDeleteSystem();
            InitializeItemRotation();
            InitializeDragAndDrop();
        }

        private void OnDestroy()
        {
            _playerControls?.Disable();
            _itemRotationHandler.Dispose();
        }

        private void InitializeUI()
        {
            var contentView = new ContentView(_document);
            contentView.AddElement(_stash);
            contentView.AddElement(_createItemButton);
            contentView.AddElement(_deleteItemButton);
            contentView.AddElement(_inventory);
        }

        private void InitializeInput()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        private void InitializeInventory()
        {
            var inventoryView =
                new InventoryView(
                    _document.rootVisualElement.Q<VisualElement>(InventoryConstants.UI.Inventory.InventoryGrid));
            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);
            _inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel);
        }

        private void InitializeStash()
        {
            var stashView =
                new InventoryView(
                    _document.rootVisualElement.Q<VisualElement>(InventoryConstants.UI.Inventory.StashGrid));
            var stashModel = new InventoryModel(new DynamicGrid(7,7));
            _stashPresenter = new StashPresenter(stashView, stashModel);
        }

        private void InitializeItemGeneration()
        {
            var itemDatabase = new ItemDatabase(ItemConfigLoader.LoadAll());
            var itemGenerationModel = new ItemGenerationModel(itemDatabase);
            var itemGenerationView = new ItemGenerationView(_document.rootVisualElement);

            _itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView,
                itemGenerationModel,
                new ItemGenerationRules(_inventoryPresenter, _stashPresenter),
                _numberOfItemsGenerated);
            _itemGenerationPresenter.OnItemGenerated += _stashPresenter.SetItems;
        }

        private void InitializeDeleteSystem()
        {
            var deleteArea =
                new DeleteAreaView(_document.rootVisualElement.Q<Button>(InventoryConstants.UI.DeleteButton));
            var deleteConfirmation = new DeleteConfirmationView(_popupUIDocument, _deleteConfirmationAsset);

            _dragDropPresenter =
                new DragDropPresenter(_inventoryPresenter, _stashPresenter, deleteArea, deleteConfirmation);
        }

        private void InitializeDragAndDrop() => _dragDropPresenter.Init(_document.rootVisualElement, _itemRotationHandler);

        private void InitializeItemRotation()
        {
            _itemRotationHandler = new ItemRotationHandler(_playerControls, () => _dragDropPresenter.CurrentItem);
        }
    }
}