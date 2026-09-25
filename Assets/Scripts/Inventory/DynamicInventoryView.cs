using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class DynamicInventoryView : InventoryView
    {
        private InventorySlotPool _pool;
        
        private readonly List<PooledSlot> _activeSlot = new();

        [Inject]
        private void Construct(InventorySlotPool pool)
        {
            _pool = pool;
        }
        public override void CreateSlots()
        {
            slots = inventory.container.Items;

            for (int i = 0; i < slots.Length; i++)
            {
                var pooled = _pool.Rent(transform);
                
                InventorySlotBinder.Bind(pooled.GameObject,
                    pooled.View,
                    slots[i],
                    this,
                    inventory.database);
                
                _activeSlot.Add(pooled);
            }
        }
    }
}