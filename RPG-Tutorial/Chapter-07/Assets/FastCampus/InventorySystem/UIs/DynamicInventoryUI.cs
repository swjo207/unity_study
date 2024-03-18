﻿using FastCampus.InventorySystem.Inventory;
using FastCampus.InventorySystem.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FastCampus.InventorySystem.UIs
{
    public class DynamicInventoryUI : InventoryUI
    {
        #region Variables
        [SerializeField]
        protected GameObject slotPrefab;

        [SerializeField]
        protected Vector2 start;

        [SerializeField]
        protected Vector2 size;

        [SerializeField]
        protected Vector2 space;

        
        [Min(1), SerializeField]
        protected int numberOfColumn = 4;

        #endregion Variables

        #region Methods

        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, Inventory.InventorySlot>();

            for (int i = 0; i < inventoryObject.Slots.Length; ++i)
            {
                GameObject go = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

                AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnter(go); });
                AddEvent(go, EventTriggerType.PointerExit, delegate { OnExit(go); });
                AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
                AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
                AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });

                inventoryObject.Slots[i].slotUI = go;
                slotUIs.Add(go, inventoryObject.Slots[i]);
                go.name += ": " + i;
            }
        }

        public Vector3 CalculatePosition(int i)
        {
            float x = start.x + ((space.x + size.x) * (i % numberOfColumn));
            float y = start.y + (-(space.y + size.y) * (i / numberOfColumn));

            return new Vector3(x, y, 0f);
        }

        protected override void OnRightClick(InventorySlot slot)
        {
            inventoryObject.UseItem(slot);
        }

        #endregion Methods
    }
}
