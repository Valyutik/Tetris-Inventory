using Runtime.Systems.ContentManager;
using UnityEngine.UIElements;
using Runtime.Utilities;
using Runtime.Input;
using Runtime.Inventory.Common;
using Runtime.Inventory.DeleteArea;
using Runtime.Inventory.DeleteConfirmation;
using Runtime.Inventory.DragAndDrop;
using Runtime.Inventory.ItemGeneration;
using Runtime.Inventory.ItemRotation;
using Runtime.Inventory.ItemTooltip;
using Runtime.Inventory.Stash;
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
        [SerializeField] private VisualTreeAsset _createButtonAsset;

        [Header("Popup")] 
        [SerializeField] private VisualTreeAsset _deleteConfirmationAsset;
        [SerializeField] private UIDocument _popupUIDocument;

        private PlayerControls _playerControls;

        private MenuContent _menuContent;
        private PopupContent _popupContent;
            
        private ItemConfig[] _itemConfigs;
        
        private InventoryModel _inventoryModel;
        private InventoryPresenter _inventoryPresenter;
        
        private InventoryModel _stashModel;
        private StashPresenter _stashPresenter;
        
        private ItemGenerationModel _itemGenerationModel;
        private ItemGenerationPresenter _itemGenerationPresenter;
        
        private ItemRotationHandler _itemRotationHandler;
        
        private DragDropPresenter _dragDropPresenter;
        private DeleteAreaPresenter _deleteAreaPresenter;
        private DeleteConfirmationPresenter _deleteConfirmationPresenter;

        private async void Start()
        {
            _inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);
            _stashModel = new InventoryModel(new DynamicGrid(_stashMaxSize.x, _stashMaxSize.y));
            _itemConfigs = await AddressablesLoader.LoadAllAsync<ItemConfig>("items");

            _itemGenerationModel = new ItemGenerationModel(
                await AddressablesLoader.LoadAsync<ItemGenerationConfig>("item_generation_config"),
                _itemConfigs);
            
            InitializeUI();
            InitializeInput();
            InitializeItemGeneration();
            InitializeStash();
            InitializeInventory();
            InitializeDeleteSystem();
            InitializeItemRotation();
            InitializeDragAndDrop();
            InitializeItemTooltip();
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
            _inventoryPresenter = new InventoryPresenter(inventoryView, _inventoryModel, _menuContent.MenuRoot);
            _inventoryPresenter.Enable();
        }

        private void InitializeStash()
        {
            var stashView = new InventoryView(_stashAsset);
            _stashPresenter = new StashPresenter(stashView,
                _stashModel,
                _menuContent.MenuRoot,
                _itemGenerationPresenter);
            
            _stashPresenter.Enable();
        }

        private void InitializeItemGeneration()
        {
            var itemGenerationView = new ItemGenerationView(_menuContent.MenuRoot, _createButtonAsset);

            _itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView,
                _itemGenerationModel,
                new ItemGenerationRules(_inventoryModel, _stashModel));
        }

        private void InitializeDeleteSystem()
        {
            var deleteAreaView = new DeleteAreaView(_menuContent.MenuRoot);
            _deleteAreaPresenter = new DeleteAreaPresenter(deleteAreaView);
            var deleteConfirmationView = new DeleteConfirmationView(_deleteConfirmationAsset);
            _deleteConfirmationPresenter = new DeleteConfirmationPresenter(deleteConfirmationView, _popupContent);
        }

        private void InitializeItemRotation()
        {
            _itemRotationHandler = new ItemRotationHandler(_playerControls, () => _dragDropPresenter.CurrentItem);
        }

        private void InitializeDragAndDrop()
        {
            _dragDropPresenter = new DragDropPresenter(_deleteAreaPresenter, _deleteConfirmationPresenter, _itemRotationHandler, _document.rootVisualElement);

            _dragDropPresenter.RegisterInventory(_inventoryModel);
            
            _dragDropPresenter.RegisterInventory(_stashModel);
            
            _dragDropPresenter.Enable();
        }

        private void InitializeItemTooltip()
        {
            var itemTooltipView = new ItemTooltipView(_popupContent);
            
            var itemTooltipPresenter = new ItemTooltipPresenter(itemTooltipView, _inventoryModel, _stashModel);
            
            itemTooltipPresenter.Enable();
        }
    }
}