using System;
using _Project.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Develop.Alexey_Pasechnik.Scripts
{
    public class InventoryHandler : MonoBehaviour, IInventoryHandler
    {
        public event Action OnRequestCreateItem;
        public event Action OnRequestTakeItem;
        public event Action OnRequestDeleteItem;
        public event Action OnRequestPlaceItem;

        private UIDocument _document;
        private VisualElement _root;

        //HACK: Затычка
        private void Awake()
        {
            Initialize(GetComponent<UIDocument>());
        }

        public void Initialize(UIDocument document)
        {
            _document = document;

            if (_document == null) return;

            _root = _document.rootVisualElement;

            InitializeCreateButton();
            InitializeDeleteButton();
        }

        private void InitializeCreateButton()
        {
            var createButton = _root.Q<Button>("CreateButton");
            createButton?.RegisterCallback<ClickEvent>(evt => OnRequestCreateItem?.Invoke());
        }

        private void InitializeDeleteButton()
        {
            var deleteButton = _root.Q<Button>("DeleteButton");
            deleteButton?.RegisterCallback<PointerOverEvent>(evt => OnRequestDeleteItem?.Invoke());
        }
    }
}