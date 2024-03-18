﻿using UnityEngine;
using System.Collections;
using FastCampus.InventorySystem.Inventory;
using UnityEngine.UI;
using FastCampus.InventorySystem.Items;
using UnityEditor.Media;
using UnityEngine.PlayerLoop;

public class PlayerStatsUI : MonoBehaviour
{
    public InventoryObject equipment;
    public StatsObject stats;

    public Text[] attributeTexts;

    private void OnEnable()
    {
        stats.OnChangedStats += OnChangedStats;

        if (equipment != null && stats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdate += OnEquipItem;
            }
        }

        UpdateAttributeTexts();
    }

    private void OnDisable()
    {
        stats.OnChangedStats -= OnChangedStats;

        if (equipment != null && stats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdate += OnEquipItem;
            }
        }
    }

    private void UpdateAttributeTexts()
    {
        attributeTexts[0].text = stats.GetModifiedValue(AttributeType.Agility).ToString("n0");
        attributeTexts[1].text = stats.GetModifiedValue(AttributeType.Intellect).ToString("n0");
        attributeTexts[2].text = stats.GetModifiedValue(AttributeType.Stamina).ToString("n0");
        attributeTexts[3].text = stats.GetModifiedValue(AttributeType.Strength).ToString("n0");
    }

    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        Debug.Log("OnRemoveItem");

        if (slot.parent.type == InterfaceType.Equipment)
        {
            foreach (ItemBuff buff in slot.item.buffs)
            {
                foreach (Attribute attribute in stats.attributes)
                {
                    if (attribute.type == buff.stat)
                    {
                        attribute.value.RemoveModifier(buff);
                    }
                }
            }
        }
    }

    public void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        Debug.Log("OnEquipItem");

        if (slot.parent.type == InterfaceType.Equipment)
        {
            foreach (ItemBuff buff in slot.item.buffs)
            {
                foreach (Attribute attribute in stats.attributes)
                {
                    if (attribute.type == buff.stat)
                    {
                        attribute.value.AddModifier(buff);
                    }
                }
            }
        }
    }

    public void OnChangedStats(StatsObject statsObject)
    {
        Debug.Log("OnChangedStats");
        UpdateAttributeTexts();
    }
}
