using System;
using System.Collections.Generic;
using Runtime.Inventory.Common;

namespace Runtime.Inventory.ItemGeneration
{
    public interface IItemGenerationPresenter
    {
        event Action<IEnumerable<Item>> OnItemGenerated;
    }
}