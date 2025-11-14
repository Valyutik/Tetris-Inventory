using System.Linq;
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
using Runtime.Utilities;
using Runtime.Input;
using Runtime.Inventory.Core;
using Runtime.Popup;
using UnityEngine;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [Header("Grid sizes")]
        [SerializeField] private Vector2Int _inventorySize;
        [SerializeField] private Vector2Int _stashMaxSize;
        
        [Header("UI Documents")] 
        [SerializeField] private UIDocument _menuDocument;
        [SerializeField] private UIDocument _popupDocument;
        
        [Header("UI Elements")] 
        [SerializeField] private VisualTreeAsset _inventoryAsset;
        [SerializeField] private VisualTreeAsset _stashAsset;
        [SerializeField] private VisualTreeAsset _createButtonAsset;
        [SerializeField] private VisualTreeAsset _deleteConfirmationAsset;

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
        
        private ModelStorage _modelStorage;

        private async void Start()
        {
            await InitializeModelStorage();

            _itemConfigs = await AddressablesLoader.LoadAllAsync<ItemConfig>("items");
            InitializeUI();
            InitializeInput();
            InitializeStash();
            await InitializeItemGeneration();
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

            _modelStorage = new ModelStorage(inventoryModel, stashModel, itemGenerationModel);
        }

        private void OnDestroy()
        {
            _playerControls?.Disable();
            _itemRotationPresenter.Disable();
            _popupPresenter.Disable();
        }

        private void InitializeUI()
        {
            _menuContent = new MenuContent(_menuDocument);
            _popupContent = new PopupContent(_popupDocument);
        }

        private void InitializeInput()
        {
            _playerControls = new PlayerControls();
            
            _playerControls.Enable();
        }

        private void InitializeInventory()
        {
            var inventoryView = new InventoryView(_inventoryAsset, _menuContent.MenuRoot);
            
            _inventoryPresenter = new InventoryPresenter(inventoryView, _modelStorage.CoreInventoryModel);
            
            _inventoryPresenter.Enable();
        }

        private void InitializeStash()
        {
            var stashView = new InventoryView(_stashAsset, _menuContent.MenuRoot);
            _stashPresenter = new StashPresenter(stashView, _modelStorage.StashInventoryModel, _modelStorage);
            
            _stashPresenter.Enable();
        }

        private async Task InitializeItemGeneration()
        {
            var itemGenerationView = new ItemGenerationView(_menuContent.MenuRoot, _createButtonAsset);

            _itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView,
                _modelStorage.ItemGenerationModel,
                new ItemGenerationRules(_modelStorage.CoreInventoryModel, _modelStorage, await AddressablesLoader.LoadAsync<ItemGenerationErrorMessage>("item_generation_error_message")));
            
            _itemGenerationPresenter.Enable();
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
            _dragDropView = new DragDropView(_menuDocument.rootVisualElement);
            
            _dragDropPresenter = new DragDropPresenter(_dragDropView, _modelStorage.DragDropModel, _modelStorage);
            
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