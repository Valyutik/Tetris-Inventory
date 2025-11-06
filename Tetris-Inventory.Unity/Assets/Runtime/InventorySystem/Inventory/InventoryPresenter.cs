using System;
using Runtime.InventorySystem.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryPresenter : IInventoryPresenter
    {
        public event Action<Vector2Int, IInventoryPresenter> OnPointerEnterCell;
        
        private readonly InventoryModel _model;
        
        private readonly InventoryView _view;

        private VisualElement[,] _cells;

        public InventoryPresenter(InventoryView view, InventoryModel model)
        {
            _view = view;
            
            _model = model;
            
            CreateView();
        }

        private void CreateView()
        {
            _view.SetUpGrid(_model.Width, _model.Height);
            
            _cells = new VisualElement[_model.Width, _model.Height];
            
            for (var y = 0; y < _model.Height; y++)
            {
                for (var x = 0; x < _model.Width; x++)
                {
                    var visualElement = _view.CreateCell();
                    
                    var targetPosition = new Vector2Int(x, y);

                    visualElement.RegisterCallback<PointerEnterEvent>(_ => OnPointerEnterCell?.Invoke(targetPosition, this));
                    
                    _cells[x, y] = visualElement;
                }
            }
            
            UpdateView();
        }

        public bool TakeItem(Vector2Int position, out Item founded)
        {
            founded = _model.GetItem(position);

            _model.TryRemoveItem(founded);
            
            UpdateView();
            
            return founded != null;
        }

        public bool PlaceItem(Item item, Vector2Int position)
        {
            if (item == null) return false;
            
            var success = _model.CanPlaceItem(item, position);

            if (success) _model.TryPlaceItem(item, position);
            
            UpdateView();
            
            return success;
        }

        private void UpdateView()
        {
            for (var y = 0; y < _model.Width; y++)
            {
                for (var x = 0; x < _model.Height; x++)
                {
                    var targetPosition = new Vector2Int(x, y);
                    
                    var visualElement = _cells[x, y];

                    var item = _model.GetItem(targetPosition);
                    
                    var color = _model.IsCellOccupied(targetPosition) ? Color.gray : item.ItemColor; 
                    
                    _view.RepaintCell(visualElement, color);
                }
            }
        }
    }
}