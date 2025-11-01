using _Project.Develop.Shared._Scripts.View;
using _Project.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Develop.Alexey_Pasechnik.Scripts
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        public IInventoryHandler InventoryHandler { get; private set; }

        [SerializeField] private int previewCellsCount;
        [SerializeField] private int inventoryCellsCount;

        private VisualElement _root;
        private UIDocument _document;

        //HACK: Затычка
        private void Awake()
        {
            Initialize(GetComponent<InventoryHandler>(), GetComponent<UIDocument>());
        }

        public void Initialize(IInventoryHandler inventoryHandler, UIDocument document)
        {
            InventoryHandler = inventoryHandler;
            InventoryHandler.OnRequestCreateItem += ShowItem;

            _document = document;
            _root = _document.rootVisualElement;

            InitializePreview();
            InitializeGrid();
        }

        private void InitializePreview()
        {
            var previewGrid = _root.Q<VisualElement>("ItemPreview");

            if (previewGrid == null) return;

            CreateGrid(previewCellsCount, previewGrid);
        }

        private void InitializeGrid()
        {
            var inventoryGrid = _root.Q<VisualElement>("Inventory");

            if (inventoryGrid == null) return;

            CreateGrid(inventoryCellsCount, inventoryGrid);
        }

        //TODO: Вынести в отдельный класс
        private void CreateGrid(int cellCount, VisualElement container)
        {
            for (var i = 0; i < cellCount; i++)
            {
                var cell = new VisualElement();
                cell.AddToClassList("cell");
                container.Add(cell);
            }
        }

        private void ShowItem()
        {
            Debug.Log("ShowItem");
        }

        public void DrawInventory(bool[,] array)
        {
            Debug.Log("DrawInventory");
        }
    }
}