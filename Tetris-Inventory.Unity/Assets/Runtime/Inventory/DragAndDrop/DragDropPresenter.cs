using Runtime.Inventory.Item.Extensions;
using Runtime.Inventory.Common;
using Runtime.Inventory.Item;
using UnityEngine.UIElements;
using Runtime.Core;
using Runtime.Systems;
using UnityEngine;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropPresenter : IPresenter
    {
        private readonly DragDropModel _model;

        private readonly DragDropView _view;

        private readonly InventoryModel _inventoryModel;

        private readonly InventoryModel _stashInventoryModel;

        private readonly IAudioService _audioService;

        public DragDropPresenter(DragDropView view, ModelStorage modelStorage, IAudioService audioService)
        {
            _view = view;

            _model = modelStorage.DragDropModel;

            _audioService = audioService;

            _inventoryModel = modelStorage.InventoryStorageModel.Get(InventoryType.Core);

            _stashInventoryModel = modelStorage.InventoryStorageModel.Get(InventoryType.Stash);
        }

        public void Enable()
        {
            _view.Root.RegisterCallback<PointerDownEvent>(OnPointerDown);

            _view.Root.RegisterCallback<PointerUpEvent>(OnPointerUp);

            _view.Root.RegisterCallback<PointerMoveEvent>(OnPointerMove);

            _inventoryModel.OnSelectCell += OnSelectCell;

            _stashInventoryModel.OnSelectCell += OnSelectCell;

            _inventoryModel.OnChangeEnabled += OnChangeInventoryEnabled;

            _stashInventoryModel.OnChangeEnabled += OnChangeInventoryEnabled;

            _model.OnRotateItem += OnRotateItem;
        }

        public void Disable()
        {
            _view.Root.UnregisterCallback<PointerDownEvent>(OnPointerDown);

            _view.Root.UnregisterCallback<PointerUpEvent>(OnPointerUp);

            _view.Root.UnregisterCallback<PointerMoveEvent>(OnPointerMove);

            _inventoryModel.OnSelectCell += OnSelectCell;

            _stashInventoryModel.OnSelectCell += OnSelectCell;

            _inventoryModel.OnChangeEnabled -= OnChangeInventoryEnabled;

            _stashInventoryModel.OnChangeEnabled -= OnChangeInventoryEnabled;

            _model.OnRotateItem -= OnRotateItem;
            
            _inventoryModel.OnPointerInGridArea -= OnPointerInOrOutGrid;
            _stashInventoryModel.OnPointerInGridArea -= OnPointerInOrOutGrid;
        }

        private void OnPointerInOrOutGrid(bool isInsideGrid)
        {
            if (!isInsideGrid)
            {
                _model.CurrentPosition = new Vector2Int(-1, -1);
            }
        }

        private void OnRotateItem(ItemModel itemModel)
        {
            _view.Drag(_model.CurrentItemModel.ToView());
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (_model.CurrentInventory == null || _model.CurrentItemModel != null || !_model.CurrentInventory.Enabled)
            {
                return;
            }

            var success = _model.CurrentInventory.TryRemoveItem(_model.CurrentPosition, out var item);

            if (!success)
            {
                return;
            }

            _model.CurrentItemModel = item;

            _model.CurrentItemModel.CacheShape();

            _model.StartPosition = item.AnchorPosition;

            _model.StartInventory = _model.CurrentInventory;

            _view.Drag(_model.CurrentItemModel.ToView(), evt.position);

            _audioService.PlayDragSound();

            _inventoryModel.OnPointerInGridArea += OnPointerInOrOutGrid;
            _stashInventoryModel.OnPointerInGridArea += OnPointerInOrOutGrid;
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_model.CurrentInventory == null || _model.CurrentItemModel == null)
            {
                return;
            }

            var success = _model.CurrentInventory.TryPlaceItem(_model.CurrentItemModel, _model.CurrentPosition);

            if (!success)
            {
                _model.CurrentItemModel.RestoreShape();
                _model.StartInventory.TryPlaceItem(_model.CurrentItemModel, _model.StartPosition);
            }

            _model.CurrentItemModel = null;

            _view.Drop();

            _audioService.PlayDragSound();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            _view.Move(evt.position);

            IndicatePlacementProjection();
        }

        private void IndicatePlacementProjection()
        {
            if (_model.CurrentInventory != null && _model.CurrentItemModel != null)
            {
                var canPlace = _model.CurrentInventory.CanPlaceItem(_model.CurrentItemModel, _model.CurrentPosition);

                var canStack = _model.CurrentInventory.CanStackAt(_model.CurrentPosition, _model.CurrentItemModel);

                if (canStack)
                {
                    _view.DrawCanStack();
                }
                else if (canPlace || _model.CanProjectionPlacementInteract)
                {
                    _view.DrawCanPlace();
                }
                else
                {
                    _view.DrawCannotPlace();
                }
            }
        }

        private void OnChangeInventoryEnabled(bool value, InventoryModel target)
        {
            if (_model.CurrentInventory == target)
            {
                _model.CurrentPosition = _model.StartPosition;
                _model.CurrentInventory = _model.StartInventory;

                _view.DrawCannotPlace();
            }
        }

        private void OnSelectCell(Vector2Int position, InventoryModel target)
        {
            _model.CurrentInventory = target;

            _model.CurrentPosition = position;
        }
    }
}