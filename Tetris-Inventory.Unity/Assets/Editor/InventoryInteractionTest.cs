using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Core
{
    public class InventoryInteractionTest : MonoBehaviour
    {
        [SerializeField] private Vector2Int _inventorySize;
        
        [SerializeField] private UIDocument _document;
        
        private InventoryModel _model;
        
        private InventoryView _view;
        
        private InventoryPresenter _presenter;
        
        private void Awake()
        {
            _model = new InventoryModel(_inventorySize.x, _inventorySize.y);
            
            var item = new Item("0", "Test", "Description", new bool[,] 
                {   { true, true }, 
                    { true, false } 
                });
            
            var item2 = new Item("0", "Test", "Description", new bool[,] 
            {   { true, true, true }, 
                { false, false, false } 
            });
            
            _model.TryPlaceItem(item, new Vector2Int(2, 0));
            _model.TryPlaceItem(item2, new Vector2Int(0, 2));

            _view = new InventoryView(_document);

            _presenter = new InventoryPresenter(_view, _model);
            
        }
    }
}