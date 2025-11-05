using Runtime.InventorySystem.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryPresenter
    {
        private readonly InventoryModel _model;
        
        private readonly InventoryView _view;

        private Item _cachedItem;
        
        private Vector2Int _cachedPosition;

        private VisualElement[,] _cells;
        
        public InventoryPresenter(InventoryView view, InventoryModel model)
        {
            _view = view;
            
            _model = model;
            
            Init();
        }

        private void Init()
        {
            _view.SetUpGrid(_model.Width, _model.Height);
            
            _cells = new VisualElement[_model.Width, _model.Height];
            
            for (var y = 0; y < _model.Height; y++)
            {
                for (var x = 0; x < _model.Width; x++)
                {
                    var visualElement = _view.CreateCell();
                    
                    var targetPosition = new Vector2Int(x, y);

                    visualElement.RegisterCallback<PointerDownEvent>(_ => TakeItem(targetPosition));
                    
                    visualElement.RegisterCallback<PointerUpEvent>(_ => PlaceItem(targetPosition));
                    
                    _cells[x, y] = visualElement;
                }
            }
            
            UpdateView();
        }

        public void TakeItem(Vector2Int position)
        {
            var item =  _model.GetItem(position);
            
            if (item == null || _cachedItem != null) return;
            
            _cachedItem = item;
            
            _cachedPosition = position;
            
            _model.TryRemoveItem(position);
            
            UpdateView();
        }

        public void PlaceItem(Vector2Int position)
        {
            if (_cachedItem == null) return;
            
            var success = _model.TryPlaceItem(_cachedItem, position);

            if (!success) _model.TryPlaceItem(_cachedItem, _cachedPosition);
            
            _cachedItem = null;
            
            UpdateView();
        }

        private void UpdateView()
        {
            for (var y = 0; y < _model.Width; y++)
            {
                for (var x = 0; x < _model.Height; x++)
                {
                    var targetPosition = new Vector2Int(x, y);
                    
                    var visualElement = _cells[x, y];

                    var color = _model.IsCellOccupied(targetPosition) ? Color.gray : Color.red; 
                    
                    _view.RepaintCell(visualElement, color);
                }
            }
        }
    }
}