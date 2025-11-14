using System.Threading.Tasks;
using Runtime.Inventory.ItemGeneration;
using Runtime.Inventory.ItemRotation;
using Runtime.Systems.ContentManager;
using Runtime.Inventory.ItemTooltip;
using Runtime.Inventory.DragAndDrop;
using Runtime.Inventory.DeleteArea;
using Runtime.Inventory.Common;
using Runtime.Inventory.Stash;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using Runtime.Utilities;
using Runtime.Input;
using Runtime.Inventory.Core;
using Runtime.Popup;
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
        
        private DragDropView _dragDropView;

        private InventoryPresenter _inventoryPresenter;
        private StashPresenter _stashPresenter;
        private ItemGenerationPresenter _itemGenerationPresenter;
        private PopupPresenter _popupPresenter;
        private DragDropPresenter _dragDropPresenter;
        private DeleteAreaPresenter _deleteAreaPresenter;
        
        private ItemRotationPresenter _itemRotationPresenter;
        
        private InventoryModelStorage _modelStorage;

        private async void Start()
        {
            await InitializeModelStorage();

            _itemConfigs = await AddressablesLoader.LoadAllAsync<ItemConfig>("items");
            InitializeUI();
            InitializeInput();
            await InitializeItemGeneration();
            InitializeStash();
            InitializeInventory();
            InitializeDeleteSystem();
            InitializeItemRotation();
            InitializeDragAndDrop();
            InitializeItemTooltip();
            InitializePopup();
        }

        private async Task InitializeModelStorage()
        {
            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);
            var stashModel = new InventoryModel(new DynamicGrid(_stashMaxSize.x, _stashMaxSize.y));
            var itemGenerationModel = new ItemGenerationModel(
                await AddressablesLoader.LoadAsync<ItemGenerationConfig>("item_generation_config"),
                _itemConfigs);

            _modelStorage = new InventoryModelStorage(inventoryModel, stashModel, itemGenerationModel);
        }

        private void OnDestroy()
        {
            _playerControls?.Disable();
            _itemRotationPresenter.Dispose();
            _popupPresenter.Disable();
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
            _inventoryPresenter = new InventoryPresenter(inventoryView, _modelStorage.CoreInventoryModel, _menuContent.MenuRoot);
            _inventoryPresenter.Enable();
        }

        private void InitializeStash()
        {
            var stashView = new InventoryView(_stashAsset);
            _stashPresenter = new StashPresenter(stashView,
                _modelStorage.StashInventoryModel,
                _menuContent.MenuRoot,
                _itemGenerationPresenter);
            
            _stashPresenter.Enable();
        }

        private async Task InitializeItemGeneration()
        {
            var itemGenerationView = new ItemGenerationView(_menuContent.MenuRoot, _createButtonAsset);

            _itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView,
                _modelStorage.ItemGenerationModel,
                new ItemGenerationRules(_modelStorage.CoreInventoryModel, _modelStorage, await AddressablesLoader.LoadAsync<ItemGenerationErrorMessage>("item_generation_error_message")));
        }

        private void InitializeDeleteSystem()
        {
            var deleteAreaView = new DeleteAreaView(_menuContent.MenuRoot, _deleteConfirmationAsset, _popupContent);
            
            _deleteAreaPresenter = new DeleteAreaPresenter(deleteAreaView, _modelStorage);
            
            _deleteAreaPresenter.Enable();
        }

        private void InitializeItemRotation()
        {
            _itemRotationPresenter = new ItemRotationPresenter(_playerControls, _modelStorage);
            
            _itemRotationPresenter.Enable();
        }

        private void InitializeDragAndDrop()
        {
            _dragDropView = new DragDropView(_document.rootVisualElement);
            
            _dragDropPresenter = new DragDropPresenter(_dragDropView, _modelStorage.DragDropModel);

            _modelStorage.DragDropModel.RegisterInventory(_modelStorage.CoreInventoryModel);
            
            _modelStorage.DragDropModel.RegisterInventory(_modelStorage.StashInventoryModel);
            
            _dragDropPresenter.Enable();
        }

        private void InitializeItemTooltip()
        {
            var itemTooltipView = new ItemTooltipView(_popupContent);
            
            var itemTooltipPresenter = new ItemTooltipPresenter(itemTooltipView, _modelStorage);
            
            itemTooltipPresenter.Enable();
        }

        private void InitializePopup()
        {
            var popupView = new PopupView(_popupContent);
            _popupPresenter = new PopupPresenter(_modelStorage.PopupModel, popupView);
            _popupPresenter.Enable();
        }
    }
}