using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using System;

namespace Runtime.InventorySystem.ItemGeneration
{
    public interface IItemGenerationPresenter
    {
        event Action<IEnumerable<Item>> OnItemGenerated;
    }
}