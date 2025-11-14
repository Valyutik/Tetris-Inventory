using System;
using Runtime.Inventory.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropPresenter : IDisposable
    {
        private readonly DragDropModel _model;

        private readonly DragDropView _view;

        public DragDropPresenter(DragDropView view, DragDropModel model)
        {
            _view = view;
            
            _model = model;
        }

        public void Enable()
        {
            _view.Root.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _view.Root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            _view.Root.RegisterCallback<PointerMoveEvent>(OnPointerMove);

            foreach (var inventory in _model.Inventories)
            {
                inventory.OnSelectCell +=  OnSelectCell;
            }
            
            _model.OnAddInventory += OnAddInventory;
            _model.OnRemoveInventory += OnRemoveInventory;
            _model.OnRotateItem += OnRotateItem;
        }

        public void Dispose()
        {
            _view.Root.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _view.Root.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            _view.Root.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            
            _model.OnAddInventory -= OnAddInventory;
            _model.OnRemoveInventory -= OnRemoveInventory;
            _model.OnRotateItem -= OnRotateItem;
        }

        private void OnRemoveInventory(InventoryModel inventory)
        {
            inventory.OnSelectCell -= OnSelectCell;
        }

        private void OnAddInventory(InventoryModel inventory)
        {
            inventory.OnSelectCell -= OnSelectCell;
        }

        private void OnInsideGrid(bool isGridArea)
        {
            if (!isGridArea)
            {
                _model.CurrentPosition = new Vector2Int(-1, -1);
            }
        }

        private void OnRotateItem(Item item)
        {
            _view.Drag(_model.CurrentItem);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (_model.CurrentInventory == null || _model.CurrentItem != null) return;
            
            var success = _model.CurrentInventory.TryRemoveItem(_model.CurrentPosition, out var item);
            
            if (!success) return;

            _model.CurrentItem = item;

            _model.CurrentItem.CacheShape();
            
            _model.StartPosition = item.AnchorPosition;
                        
            _model.StartInventory = _model.CurrentInventory;
            
            _view.Drag(_model.CurrentItem, evt.position);
            
            _model.CurrentInventory.OnPointerInGridArea += OnInsideGrid;
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_model.CurrentInventory == null || _model.CurrentItem == null) return;

            var success = _model.CurrentInventory.TryPlaceItem(_model.CurrentItem, _model.CurrentPosition);

            if (!success)
            {
                _model.CurrentItem.RestoreShape();
                _model.StartInventory.TryPlaceItem(_model.CurrentItem, _model.StartPosition);
            }
            
            _model.CurrentItem = null;
            
            _view.Drop();
            
            _model.CurrentInventory.OnPointerInGridArea -= OnInsideGrid;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            _view.Move(evt.position);

            IndicatePlacementProjection();
        }
        
        //TODO: Вынести в отдельную фичу
        private void IndicatePlacementProjection()
        {
            if (_model.CurrentInventory == null || _model.CurrentItem == null)
            {
                return;
            }
            
            var canPlace = _model.CurrentInventory.CanPlaceItem(_model.CurrentItem, _model.CurrentPosition);
            
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