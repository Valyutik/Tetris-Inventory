using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.ItemGeneration;
using Runtime.InventorySystem.ItemRotation;
using Runtime.InventorySystem.DragAndDrop;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Stash;
using Runtime.Systems.ContentManager;
using UnityEngine.UIElements;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Vector2Int _inventorySize;

        [Header("UI Elements")] 
        [SerializeField] private VisualTreeAsset _inventoryAsset;
        [SerializeField] private VisualTreeAsset _stashAsset;

        [Header("Popup")] 
        [SerializeField] private VisualTreeAsset _deleteConfirmationAsset;
        [SerializeField] private UIDocument _popupUIDocument;

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
            InitializeStash();
            InitializeInventory();
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
            var menuContent = new MenuContent(_document);
            var popupContent = new PopupContent(_popupUIDocument);
        }

        private void InitializeInput()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        private void InitializeInventory()
        {
            var inventoryView = new InventoryView(_inventoryAsset);
            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);
            _inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel);
        }

        private void InitializeStash()
        {
            var stashView = new InventoryView(_stashAsset);
            var stashModel = new InventoryModel(new DynamicGrid(7, 7));
            _stashPresenter = new StashPresenter(stashView, stashModel);
        }

        private async void InitializeItemGeneration()
        {
            var itemConfigs = await AddressablesLoader.LoadAllAsync<ItemConfig>("items");

            var itemGenerationModel = new ItemGenerationModel(
                await AddressablesLoader.LoadAsync<ItemGenerationConfig>("item_generation_config"),
                itemConfigs);

            var itemGenerationView = new ItemGenerationView();

            _itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView,
                itemGenerationModel,
                new ItemGenerationRules(_inventoryPresenter, _stashPresenter));
            _itemGenerationPresenter.OnItemGenerated += _stashPresenter.SetItems;
        }

        private void InitializeDeleteSystem()
        {
            var deleteAreaView = new DeleteAreaView();
            var deleteAreaPresenter = new DeleteAreaPresenter(deleteAreaView);
            var deleteConfirmationView = new DeleteConfirmationView(_deleteConfirmationAsset);
            var deleteConfirmationPresenter = new DeleteConfirmationPresenter(deleteConfirmationView);

            _dragDropPresenter = new DragDropPresenter(deleteAreaPresenter, deleteConfirmationPresenter);

            _dragDropPresenter.RegisterInventory(_inventoryPresenter);

            _dragDropPresenter.RegisterInventory(_stashPresenter);
        }

        private void InitializeItemRotation()
        {
            _itemRotationHandler = new ItemRotationHandler(_playerControls, () => _dragDropPresenter.CurrentItem);
        }

        private void InitializeDragAndDrop()
        {
            _dragDropPresenter.Init(_document.rootVisualElement);
            _itemRotationHandler.OnItemRotated += _dragDropPresenter.UpdateItem;
        }
    }
}