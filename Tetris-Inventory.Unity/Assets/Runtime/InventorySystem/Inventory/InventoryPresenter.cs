using Runtime.InventorySystem.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryPresenter : IInventoryPresenter
    {
        public Vector2Int SelectedPosition { get; private set; }

        private readonly InventoryModel _model;
        
        private readonly InventoryView _view;

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

                    visualElement.RegisterCallback<PointerDownEvent>(_ => SelectedPosition = targetPosition);
                    
                    visualElement.RegisterCallback<PointerUpEvent>(_ => SelectedPosition =  targetPosition);
                    
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

                    var color = _model.IsCellOccupied(targetPosition) ? Color.gray : Color.red; 
                    
                    _view.RepaintCell(visualElement, color);
                }
            }
        }
    }
}