﻿using FastCampus.InventorySystem.Items;
using FastCampus.InventorySystem.UIs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FastCampus.InventorySystem.Inventory
{
    [Serializable]
    public class InventorySlot
    {
        #region Variables

        public ItemType[] AllowedItems = new ItemType[0];

        [NonSerialized]
        public InventoryObject parent;
        [NonSerialized]
        public GameObject slotUI;

        [NonSerialized]
        public Action<InventorySlot> OnPreUpdate;
        [NonSerialized]
        public Action<InventorySlot> OnPostUpdate;

        public Item item;
        public int amount;

        #endregion Variables

        #region Properties
        public ItemObject ItemObject
        {
            get
            {
                return item.id >= 0 ? UIManager.Instance.itemDatabase.itemObjects[item.id] : null;
            }
        }

        #endregion Properties

        #region Methods
        public InventorySlot() => UpdateSlot(new Item(), 0);

        public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);

        public void RemoveItem() => UpdateSlot(new Item(), 0);

        public void AddAmount(int value) => UpdateSlot(item, amount += value);

        public void UpdateSlot(Item item, int amount)
        {
            if (amount <= 0)
            {
                item = new Item();
            }

            OnPreUpdate?.Invoke(this);
            this.item = item;
            this.amount = amount;
            OnPostUpdate?.Invoke(this);
        }

        public bool CanPlaceInSlot(ItemObject itemObject)
        {
            if (AllowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
            {
                return true;
            }

            foreach (ItemType itemType in AllowedItems)
            {
                if (itemObject.type == itemType)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Methods
    }
}
