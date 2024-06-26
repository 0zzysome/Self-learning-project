using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Button removeButton;
    public TMP_Text amountText;
    public TMP_Text worthText;
    public Image icon;
    Item item;
    public void AddItem(Item newitem) 
    {
        item = newitem;   
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        amountText.enabled = true;
        amountText.text = item.amount.ToString();
        worthText.enabled = true;
        
        worthText.text =item.totalWorth.ToString();
        //Debug.Log(item.amount +" number of items with the name "+ item.name);
    }
    public void ClearSlot() 
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        amountText.enabled = false;
        worthText.enabled = false;
    }
    public void OnRemoveButton()
    {
        Inventory.Instance.Drop(item);

    }
    public void UseItem() 
    {
        if (item != null) 
        {
            item.UseInInventory();
        }
    }
}
