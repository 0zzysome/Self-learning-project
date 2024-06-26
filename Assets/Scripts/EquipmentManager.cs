using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    private void Awake()
    {
        Instance = this;

    }
    public float throwStrengthMult = 10f;
    public Item[] currentEquipment;
    public bool IsHoldingItem;
    public Transform handPosition;
    
    
    


    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.Instance;
        //makes the list of equipable items the same size as there can be items equiped
        int numSlots = System.Enum.GetNames(typeof(EquipSlot)).Length;
        currentEquipment = new Item[numSlots];
    }
    public void Equip(Item newitem) 
    {
        // makes shure the item position is not updated while this code is running 
        IsHoldingItem = false;
        int slotIndex = (int)newitem.equipSlot;
        Item olditem = null;
        // if there is an item already in that space 
        if (currentEquipment[slotIndex] != null)
        {
            //save the old item 
            olditem = currentEquipment[slotIndex];
            //put the item back in the inventory
            
            //makes it so the item doesnt stack when going back in inventory.
            olditem.stackable = false;
            //turns on collision for item
            olditem.ToggleIsTrigger(false);
            //turns item off so its no longer in the scene.
            olditem.objectRef.SetActive(false);
            //saves local scale so it does not change after unparenting
            Vector3 saveScale = olditem.objectRef.transform.localScale;
            //removes the object form the hand. aka unparents it. 
            olditem.objectRef.transform.parent = null;
            //applies correct scale after unparent 
            olditem.objectRef.transform.localScale = saveScale;
            // adds it to inventory
            inventory.Add(olditem);
            // sets it to stackable again after adding to inventory.
            olditem.stackable = true;
        }
        //equips item to list and correct slot 
        currentEquipment[slotIndex] = newitem;
        //HELD ITEM STUFF (its a mess)
        //makes shure you can see the held item
        currentEquipment[0].objectRef.SetActive(true);
        //makes it a trigger so it doesent interact with the player
        currentEquipment[0].ToggleIsTrigger(true);
        //saves scale for after parent 
        
        Vector3 scaleRef = currentEquipment[0].objectRef.transform.localScale;
        currentEquipment[0].objectRef.transform.position = handPosition.position;
        currentEquipment[0].objectRef.transform.parent = handPosition;
        currentEquipment[0].objectRef.transform.localScale = scaleRef;
        inventory.ItemWasChanged();
        // now updates the position of the item again
        IsHoldingItem = true;


    }
}
