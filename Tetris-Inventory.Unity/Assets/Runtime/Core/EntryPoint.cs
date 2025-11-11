using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.ItemGeneration;
using Runtime.InventorySystem.ItemRotation;
using Runtime.InventorySystem.DragAndDrop;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using Runtime.Systems.ContentManager;
using Runtime.InventorySystem.Stash;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Vector2Int _inventorySize;
        [SerializeField] private Vector2Int _stashMaxSize;

        [Header("UI Elements")] 
        [SerializeField] private VisualTreeAsset _inventoryAsset;
        [SerializeField] private VisualTreeAsset _stashAsset;

        [Header("Popup")] 
        [SerializeField] private VisualTreeAsset _deleteConfirmationAsset;
        [SerializeField] private UIDocument _popupUIDocument;

        private PlayerControls _playerControls;

        private MenuContent _menuContent;
        private PopupContent _popupContent;
            
        private InventoryPresenter _inventoryPresenter;
        private StashPresenter _stashPresenter;
        private DragDropPresenter _dragDropPresenter;
        private ItemGenerationPresenter _itemGenerationPresenter;
        private ItemRotationHandler _itemRotationHandler;

        private async void Start()
        {
            InitializeUI();
            InitializeInput();
            InitializeStash();
            InitializeInventory();
            await InitializeItemGeneration();
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
            _menuContent = new MenuContent(_document);
            _popupContent = new PopupContent(_popupUIDocument);
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
            _inventoryPresenter = new InventoryPresenter(inventoryView, inventoryModel, _menuContent.MenuRoot);
        }

        private void InitializeStash()
        {
            var stashView = new InventoryView(_stashAsset);
            var stashModel = new InventoryModel(new DynamicGrid(_stashMaxSize.x, _stashMaxSize.y));
            _stashPresenter = new StashPresenter(stashView,
                stashModel,
                _menuContent.MenuRoot,
                _itemGenerationPresenter);
        }

        private async Task InitializeItemGeneration()
        {
            var itemConfigs = await AddressablesLoader.LoadAllAsync<ItemConfig>("items");

            var itemGenerationModel = new ItemGenerationModel(
                await AddressablesLoader.LoadAsync<ItemGenerationConfig>("item_generation_config"),
                itemConfigs);

            var itemGenerationView = new ItemGenerationView(_menuContent.MenuRoot);

            _itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView,
                itemGenerationModel,
                new ItemGenerationRules(_inventoryPresenter, _stashPresenter));
        }

        private void InitializeDeleteSystem()
        {
            var deleteAreaView = new DeleteAreaView(_menuContent.MenuRoot);
            var deleteAreaPresenter = new DeleteAreaPresenter(deleteAreaView);
            var deleteConfirmationView = new DeleteConfirmationView(_deleteConfirmationAsset);
            var deleteConfirmationPresenter = new DeleteConfirmationPresenter(deleteConfirmationView);

            _dragDropPresenter = new DragDropPresenter(deleteAreaPresenter,
                deleteConfirmationPresenter,
                _itemRotationHandler, _document.rootVisualElement);

            _dragDropPresenter.RegisterInventory(_inventoryPresenter);

            _dragDropPresenter.RegisterInventory(_stashPresenter);
        }

        private void InitializeItemRotation()
        {
            _itemRotationHandler = new ItemRotationHandler(_playerControls, () => _dragDropPresenter.CurrentItem);
        }

        private void InitializeDragAndDrop()
        {
            _dragDropPresenter.Init();
        }
    }
}