using Runtime.InventorySystem.Common;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ItemConfig))]
    public sealed class ItemConfigEditor : UnityEditor.Editor
    {
        private HelpBox _warningBox;

        public override VisualElement CreateInspectorGUI()
        {
            var config = (ItemConfig)target;
            var root = new VisualElement();

            AddIdField(config, root);

            AddNameField(config, root);

            AddDescriptionField(root, config);

            AddColorField(config, root);
            
            AddStackField(config, root);

            var shapeLabel = new Label(" Shape")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            
            root.Add(shapeLabel);

            var widthField = new IntegerField("Width")
            {
                value = config.width
            };
            
            var heightField = new IntegerField("Height")
            {
                value = config.height
            };
            
            widthField.RegisterValueChangedCallback(evt =>
            {
                config.width = Mathf.Max(1, evt.newValue);
                UpdateShapeGrid(root, config);
                EditorUtility.SetDirty(config);
            });
            
            heightField.RegisterValueChangedCallback(evt =>
            {
                config.height = Mathf.Max(1, evt.newValue);
                UpdateShapeGrid(root, config);
                EditorUtility.SetDirty(config);
            });
            
            root.Add(widthField);
            root.Add(heightField);

            var gridContainer = new VisualElement
            {
                name = "ShapeGrid"
            };
            
            root.Add(gridContainer);

            var clearButton = new Button(HandleClearButton)
            {
                text = "Clear Shape"
            };

            root.Add(clearButton);

            UpdateShapeGrid(root, config);
            
            _warningBox = new HelpBox("", HelpBoxMessageType.Warning)
            {
                style =
                {
                    display = DisplayStyle.None
                }
            };
            
            root.Add(_warningBox);
            
            ValidateShape(config);

            UpdateShapeGrid(root, config);

            return root;

            void HandleClearButton()
            {
                for (var i = 0; i < config.flatShape.Count; i++) config.flatShape[i] = false;

                UpdateShapeGrid(root, config);
                EditorUtility.SetDirty(config);
            }
        }

        private static void AddStackField(ItemConfig config, VisualElement root)
        {
            var isStackableField = new Toggle("Is Stackable")
            {
                value = config.isStackable
            };
            
            var maxStackField = new IntegerField("Max Stack")
            {
                value = config.MaxStack
            };
            
            root.Add(isStackableField);
            root.Add(maxStackField);

            isStackableField.RegisterValueChangedCallback(evt =>
            {
                config.isStackable = evt.newValue;
                EditorUtility.SetDirty(config);
            });

            maxStackField.RegisterValueChangedCallback(evt =>
            {
                config.MaxStack = evt.newValue;
                EditorUtility.SetDirty(config);
            });
        }

        private void ValidateShape(ItemConfig config)
        {
            var hasAny = config.flatShape.Any(c => c);
            if (!hasAny)
            {
                _warningBox.text = "Shape must contain at least one filled cell.";
                _warningBox.style.display = DisplayStyle.Flex;
            }
            else
            {
                _warningBox.style.display = DisplayStyle.None;
            }
        }

        private void AddColorField(ItemConfig config, VisualElement root)
        {
            var colorField = new ColorField("Color")
            {
                value = config.color
            };
            
            colorField.RegisterValueChangedCallback(evt =>
            {
                config.color = evt.newValue;
                EditorUtility.SetDirty(config);
            });
            
            root.Add(colorField);
        }

        private void AddDescriptionField(VisualElement root, ItemConfig config)
        {
            var descLabel = new Label(" Description");
            
            root.Add(descLabel);

            var descriptionField = new TextField
            {
                value = config.description, multiline = true,
            };
            
            descriptionField.RegisterValueChangedCallback(evt =>
            {
                config.description = evt.newValue;
                EditorUtility.SetDirty(config);
            });
            
            root.Add(descriptionField);
        }

        private void AddNameField(ItemConfig config, VisualElement root)
        {
            var nameField = new TextField("Display Name")
            {
                value = config.displayName
            };
            
            nameField.RegisterValueChangedCallback(evt =>
            {
                config.displayName = evt.newValue;
                EditorUtility.SetDirty(config);
            });
            
            root.Add(nameField);
        }

        private void AddIdField(ItemConfig config, VisualElement root)
        {
            var idField = new TextField("ID")
            {
                value = config.id
            };
            
            idField.RegisterValueChangedCallback(evt =>
            {
                config.id = evt.newValue;
                EditorUtility.SetDirty(config);
            });
            
            root.Add(idField);
        }

        private void UpdateShapeGrid(VisualElement root, ItemConfig config)
        {
            var totalCells = config.width * config.height;

            while (config.flatShape.Count < totalCells)
            {
                config.flatShape.Add(false);
            }

            while (config.flatShape.Count > totalCells)
            {
                config.flatShape.RemoveAt(config.flatShape.Count - 1);
            }

            var gridContainer = root.Q<VisualElement>("ShapeGrid");
            gridContainer.Clear();

            for (var y = 0; y < config.height; y++)
            {
                var row = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row
                    }
                };

                for (var x = 0; x < config.width; x++)
                {
                    var index = y * config.width + x;

                    var toggle = new Toggle
                    {
                        value = config.flatShape[index],
                        style =
                        {
                            width = 20,
                            height = 20
                        }
                    };
                    toggle.RegisterValueChangedCallback(evt =>
                    {
                        config.flatShape[index] = evt.newValue;
                        EditorUtility.SetDirty(config);
                        ValidateShape(config);
                    });

                    row.Add(toggle);
                }

                gridContainer.Add(row);
            }
        }
    }
}