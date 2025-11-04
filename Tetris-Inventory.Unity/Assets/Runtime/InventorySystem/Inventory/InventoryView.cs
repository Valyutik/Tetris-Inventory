using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private int inventoryGridWidth;
        [SerializeField] private int inventoryGridHeight;
        [SerializeField] private int previewGridWidth;
        [SerializeField] private int previewGridHeight;
        [SerializeField] private int cellSize;

        private VisualElement _root;
        
        private IInventoryHandler _inventoryHandler;
        private UIDocument _document;

        //HACK: Crutch
        private void Awake()
        {
            Initialize(GetComponent<InventoryHandler>(), GetComponent<UIDocument>());
        }

        public void Initialize(IInventoryHandler inventoryHandler, UIDocument document)
        {
            _inventoryHandler = inventoryHandler;
            _inventoryHandler.OnRequestCreateItem += ShowItem;

            _document = document;
            _root = _document.rootVisualElement;

            InitializePreview();
            InitializeInventory();
        }

        private void InitializePreview()
        {
            var preview = _root.Q<VisualElement>("ItemPreview");
            var previewGrid = preview.Q<VisualElement>("Grid");

            if (previewGrid == null) return;
            
            DrawEmptyGrid(previewGridWidth, previewGridHeight, previewGrid);
        }

        private void InitializeInventory()
        {
            var inventory =_root.Q<VisualElement>("InventoryModel");
            var inventoryGrid =inventory.Q<VisualElement>("Grid");

            if (inventoryGrid == null) return;

            DrawEmptyGrid(inventoryGridWidth, inventoryGridHeight, inventoryGrid);
        }

        private void DrawEmptyGrid(int width, int height, VisualElement container)
        {
            container.style.height = height * cellSize;
            container.style.width = width * cellSize;
            
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var cell = new VisualElement();
                    cell.AddToClassList("cell");
                    
                    cell.style.width = cellSize;
                    cell.style.height = cellSize;
                    
                    container.Add(cell);
                }
            }
        }

        //TODO: Implement the logic of displaying an item
        private void ShowItem()
        {
            Debug.Log("ShowItem");
        }
    }
}