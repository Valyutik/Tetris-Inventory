using System;
using Runtime.Inventory.Common;
using Runtime.Inventory.Core;
using Runtime.Inventory.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropPresenter : IPresenter
    {
        private readonly DragDropModel _model;

        private readonly DragDropView _view;
        
        private readonly InventoryModel _inventoryModel;

        private readonly InventoryModel _stashInventoryModel;
        
        public DragDropPresenter(DragDropView view, DragDropModel model, ModelStorage modelStorage)
        {
            _view = view;
            
            _model = model;
            
            _inventoryModel =  modelStorage.CoreInventoryModel;
            
            _stashInventoryModel = modelStorage.StashInventoryModel;
        }

        public void Enable()
        {
            _view.Root.RegisterCallback<PointerDownEvent>(OnPointerDown);
            
            _view.Root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            _view.Root.RegisterCallback<PointerMoveEvent>(OnPointerMove);

            _inventoryModel.OnSelectCell += OnSelectCell;
            
            _stashInventoryModel.OnSelectCell += OnSelectCell;

            _model.OnRotateItem += OnRotateItem;
        }

        public void Disable()
        {
            _view.Root.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            
            _view.Root.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            
            _view.Root.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            
            _inventoryModel.OnSelectCell += OnSelectCell;
            
            _stashInventoryModel.OnSelectCell += OnSelectCell;

            _model.OnRotateItem -= OnRotateItem;
        }

        private void OnInsideGrid(bool isGridArea)
        {
            if (!isGridArea)
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
            if (_model.CurrentInventory == null || _model.CurrentItemModel != null) return;
            
            var success = _model.CurrentInventory.TryRemoveItem(_model.CurrentPosition, out var item);
            
            if (!success) return;

            _model.CurrentItemModel = item;

            _model.CurrentItemModel.CacheShape();
            
            _model.StartPosition = item.AnchorPosition;
                        
            _model.StartInventory = _model.CurrentInventory;
            
            _view.Drag(_model.CurrentItemModel.ToView(), evt.position);
            
            _model.CurrentInventory.OnPointerInGridArea += OnInsideGrid;
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_model.CurrentInventory == null || _model.CurrentItemModel == null) return;

            var success = _model.CurrentInventory.TryPlaceItem(_model.CurrentItemModel, _model.CurrentPosition);

            if (!success)
            {
                _model.CurrentItemModel.RestoreShape();
                _model.StartInventory.TryPlaceItem(_model.CurrentItemModel, _model.StartPosition);
            }
            
            _model.CurrentItemModel = null;
            
            _view.Drop();
            
            _model.CurrentInventory.OnPointerInGridArea -= OnInsideGrid;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            _view.Move(evt.position);

            IndicatePlacementProjection();
        }
        
        private void IndicatePlacementProjection()
        {
            if (_model.CurrentInventory == null || _model.CurrentItemModel == null)
            {
                return;
            }
            
            var canPlace = _model.CurrentInventory.CanPlaceItem(_model.CurrentItemModel, _model.CurrentPosition);
            
            if (canPlace || _model.CanProjectionPlacementInteract)
            {
                _view.SetCanPlace();
            }
            else
            {
                _view.SetCannotPlace();
            }
        }

        private void OnSelectCell(Vector2Int position, InventoryModel target)
        {
            _model.CurrentInventory = target;
            
            _model.CurrentPosition = position;
        }
    }
}