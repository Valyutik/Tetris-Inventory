using Runtime.Inventory.ItemGeneration;
using Runtime.Inventory.ItemRotation;
using Runtime.Systems.ContentManager;
using Runtime.Inventory.ItemTooltip;
using Runtime.Inventory.DragAndDrop;
using Runtime.Inventory.DeleteArea;
using Runtime.Inventory.Common;
using Runtime.Inventory.Stash;
using UnityEngine.UIElements;
using Runtime.Input;
using Runtime.Popup;
using UnityEngine;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [Header("Grid sizes")]
        [SerializeField] private Vector2Int _inventorySize;
        [SerializeField] private Vector2Int _stashInitialSize;
        
        [Header("UI Documents")] 
        [SerializeField] private UIDocument _menuDocument;
        [SerializeField] private UIDocument _popupDocument;
        
        [Header("UI Elements")] 
        [SerializeField] private VisualTreeAsset _inventoryAsset;
        [SerializeField] private VisualTreeAsset _stashAsset;
        [SerializeField] private VisualTreeAsset _createButtonAsset;
        [SerializeField] private VisualTreeAsset _deleteConfirmationAsset;

        [Header("Item Generation")]
        [SerializeField] private ItemGenerationConfig _generationConfig;
        [SerializeField] private ItemGenerationErrorMessage _generationErrorMessage;
        
        private PlayerControls _playerControls;

        private MenuContent _menuContent;
        private PopupContent _popupContent;
        
        private DragDropView _dragDropView;

        private InventoryPresenter _inventoryPresenter;
        private StashPresenter _stashPresenter;
        private ItemGenerationPresenter _itemGenerationPresenter;
        private PopupPresenter _popupPresenter;
        private DragDropPresenter _dragDropPresenter;
        private DeleteAreaPresenter _deleteAreaPresenter;
        
        private ItemRotationPresenter _itemRotationPresenter;
        
        private ModelStorage _modelStorage;

        private void Start()
        {
            InitializeModelStorage();
            
            InitializeUI();
            InitializeInput();
            InitializeStash();
            InitializeItemGeneration();
            InitializeInventory();
            InitializeDeleteSystem();
            InitializeItemRotation();
            InitializeDragAndDrop();
            InitializeItemTooltip();
            InitializePopup();
        }

        private void InitializeModelStorage()
        {
            var inventoryModel = new InventoryModel(_inventorySize.x, _inventorySize.y);
            var stashModel = new InventoryModel(_stashInitialSize.x, _stashInitialSize.y);
            var itemGenerationModel = new ItemGenerationModel(_generationConfig);

            _modelStorage = new ModelStorage(itemGenerationModel);
            
            _modelStorage.InventoryStorageModel.RegisterInventory(InventoryType.Core, inventoryModel);
            
            _modelStorage.InventoryStorageModel.RegisterInventory(InventoryType.Stash, stashModel);
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
            
            _inventoryPresenter = new InventoryPresenter(inventoryView, _modelStorage.InventoryStorageModel.Get(InventoryType.Core));
            
            _inventoryPresenter.Enable();
        }

        private void InitializeStash()
        {
            var stashView = new InventoryView(_stashAsset, _menuContent.MenuRoot);
            _stashPresenter = new StashPresenter(stashView, _modelStorage.InventoryStorageModel.Get(InventoryType.Stash), _modelStorage);
            
            _stashPresenter.Enable();
        }

        private void InitializeItemGeneration()
        {
            var itemGenerationView = new ItemGenerationView(_menuContent.MenuRoot, _createButtonAsset);

            var itemGenerationRules = new ItemGenerationRules(_modelStorage.InventoryStorageModel.Get(InventoryType.Core), _modelStorage, _generationErrorMessage);
            
            _itemGenerationPresenter = new ItemGenerationPresenter(itemGenerationView, _modelStorage.ItemGenerationModel, itemGenerationRules);
            
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